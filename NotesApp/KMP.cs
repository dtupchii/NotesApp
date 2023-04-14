using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    class KMP
    {
        static public List<Note> Search(string example, List<Note> notes)
        {
            List<Note> result = new List<Note>();
            char[] charsOfExample = example.ToCharArray();

            foreach (Note note in notes)
            {
                char[] charsOfText = note.DecodedText.ToCharArray();
                for (int i = 0, j = 0; i < charsOfText.Length; i++)
                {
                    if (charsOfText[i] == charsOfExample[j])
                    {
                        j++;
                    }
                    else
                    {
                        j = 0;
                    }
                    if (j == charsOfExample.Length)
                    {
                        result.Add(note);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
