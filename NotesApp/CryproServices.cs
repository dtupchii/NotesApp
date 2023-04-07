using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace NotesApp
{
    static class CryptoServices
    {
        static int count = 0;
        static public Dictionary<char, string> Encrypt(string text)
        {
            char[] chars = text.ToCharArray();
            List<Node> nodes = new List<Node>();
            Dictionary<char, string> encoded = new Dictionary<char, string>();

            if (chars.Length > 0)
            {
                //Counting each character
                foreach (char c in chars)
                {
                    if (nodes.Where(n => n.Character == c).FirstOrDefault() == null)
                    {
                        nodes.Add(new Node { Character = c, Frequency = 1 });
                    }
                    else
                    {
                        Node node = nodes.Where(f => f.Character == c).FirstOrDefault();
                        node.Frequency++;
                    }
                }

                count = nodes.Count;

                //Building a tree
                while (nodes.Count >= 2)
                {
                    nodes = nodes.OrderBy(f => f.Frequency).ToList();

                    Node root = new Node() { Frequency = nodes[0].Frequency + nodes[1].Frequency, Left = nodes[0], Right = nodes[1] };
                    nodes[0].Root = root;
                    nodes[1].Root = root;
                    nodes.RemoveAt(0);
                    nodes.RemoveAt(0);
                    nodes.Insert(0, root);
                }

                //Encoding
                Encode(nodes[0], ref encoded);
            }
            return encoded;
        }

        static public void Encode(Node r, ref Dictionary<char, string> encoded)
        {
            start:
            string code = "";

            while (r.Root != null)
            {
                r = r.Root;
            }

            while (r.Left != null && r.Right != null)
            {
                if (r.Left != null && !r.Left.Marked)
                {
                    code += "0";
                    r = r.Left;
                }
                else if (r.Right != null && !r.Right.Marked)
                {
                    code += "1";
                    r = r.Right;
                }
                else if (r.Left.Marked && r.Right.Marked)
                {
                    r.Marked = true;
                    goto start;
                }
            }
            r.Marked = true;
            if (r.Root != null && r.Root.Left.Marked && r.Root.Right.Marked)
            {
                r.Root.Marked = true;
            }

            encoded.Add(r.Character, code);

            if (encoded.Count < count)
                goto start;
            //Encode(r, ref encoded);
        }

        static public string Decrypt(Dictionary<char, string> csp, string text)
        {
            string[] letters = text.Split(' ');
            string res = "";

            for (int i = 0; i < letters.Length - 1; i++)
            {
                res += csp.Where(a => a.Value == letters[i]).FirstOrDefault().Key;
            }

            return res;
        }
    }
}
