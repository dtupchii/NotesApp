using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NotesApp
{
    internal class FileIOServices
    {
        private readonly string path;
        public FileIOServices(string PATH) 
        {
            path = PATH;
        }

        public void WritingData (Dictionary<char, string> kvp)
        {
            using (StreamWriter writer = File.CreateText (path))
            {
                string output = JsonConvert.SerializeObject (kvp);
                writer.WriteLine (output);
            }
        }

        public Dictionary<char, string> ReadingData ()
        {
            var fileExists = File.Exists(path);
            if (!fileExists)
            {
                File.CreateText(path).Dispose();
                return new Dictionary<char, string>();
            }
            using (var reader = File.OpenText(path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<char, string>>(fileText);
            }
        }
    }
}
