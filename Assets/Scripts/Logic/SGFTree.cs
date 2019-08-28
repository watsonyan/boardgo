using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace game.logic
{
    public class SGFTree
    {
        TreeNode root;

        readonly static int maxbuffer = 4096;
        char[] Buffer = new char[maxbuffer]; // the buffer for reading of files
        int BufferN;

        public SGFTree(TreeNode node)
        {
            root = node;
            root.IsMain = true;
        }

        public TreeNode Root
        {
            get
            {
                return root;
            }
        }

        static int lastnl = 0;
        char ReadChar(StreamReader reader)
        {
            int c;
            while (true)
            {
                c = reader.Read();
                if (c == -1) throw new IOException();
                if (c == 13)
                { if (lastnl == 10) lastnl = 0;
                    else
                    { lastnl = 13; return '\n';
                    }
                }
                else if (c == 10)
                { if (lastnl == 13) lastnl = 0;
                    else
                    { lastnl = 10; return '\n';
                    }
                }
                else
                { lastnl = 0;
                    return (char)c;
                }
            }
        }

        char ReadNext(StreamReader reader)
        { int c = ReadChar(reader);
            while (c == '\n' || c == '\t' || c == ' ')
            { if (c == -1) throw new IOException();
                c = ReadChar(reader);
            }
            return (char)c;
        }

        char ReadNode(TreeNode tree, StreamReader reader)
        {
            //bool sgf=GF.getParameter("sgfcomments",false);

            char c = ReadNext(reader);
            Action action;
            //Node n = new Node(((Node)p.content()).number());
            TreeNode node = new TreeNode(tree.EptTreeNum);
            string s;
            loop: while (true) // read all actions
            {
                BufferN = 0;
                while (true)
                {
                    if (c >= 'A' && c <= 'Z') Store(c);
                    // note only capital letters
                    else if (c == '(' || c == ';' || c == ')') goto goon;
                    // last property reached
                    // BufferN should be 0 then
                    else if (c == '[') break;
                    // end of porperty type, arguments starting
                    else if (c < 'a' || c > 'z') throw new IOException();
                    // this is an error
                    c = ReadNext(reader);
                }
                if (BufferN == 0) throw new IOException();
                s = new String(Buffer, 0, BufferN);
                //if (s.Equals("L")) a=new LabelAction(GF);
                //else if (s.equals("M")) a=new MarkAction(GF);
                //else 
                action = new Action(s);
                while (c == '[')
                {
                    BufferN = 0;
                    while (true)
                    {
                        c = ReadChar(reader);
                        if (c == '\\')
                        {
                            c = ReadChar(reader);
                            if (/*sgf &&*/ c == '\n')
                            {
                                if (BufferN > 1 && Buffer[BufferN - 1] == ' ') continue;
                                else c = ' ';
                            }
                        }
                        else if (c == ']') break;

                        Store(c);
                    }
                    c = ReadNext(reader); // prepare next argument
                    String s1;
                    if (BufferN > 0) s1 = new String(Buffer, 0, BufferN);
                    else s1 = "";
                    //if (!expand(action, s1))
                        action.AddArugment(s1);
                }
                // no more arguments
                node.AddAction(action);
                if (action.Type.Equals("B") || action.Type.Equals("W"))
                {
                    node.EptTreeNum = node.EptTreeNum + 1;
                }
            } // end of actions has been found
              // append node
            goon:
            node.SetMain(tree);
            if (tree.FirstAction == null)
                tree.SetNode(node);
            else
            {
                tree.AddChild(node);
                node.SetMain(tree);
                tree = node;
                if (tree.Parent != null && tree != tree.Parent.FirstChild)
                    tree.EptTreeNum = 2;
            }
            return c;
        }

        bool Expand(Action action, string arg)
        {
            String t = action.Type;
            if (!(t.Equals("MA") || t.Equals("SQ") || t.Equals("TR") ||
                 t.Equals("CR") || t.Equals("AW") || t.Equals("AB") ||
                  t.Equals("AE") || t.Equals("SL"))) return false;
            if (arg.Length != 5 || arg[2] != ':') return false;
            String s0 = arg.Substring(0, 2), s1 = arg.Substring(3);
            //int i0 = Field.i(s0), j0 = Field.j(s0);
            //int i1 = Field.i(s1), j1 = Field.j(s1);
            //if (i1 < i0 || j1 < j0) return false;
            //int i, j;
            //for (i = i0; i <= i1; i++)
            //    for (j = j0; j <= j1; j++)
            //    {
            //        a.addargument(Field.string(i, j));
            //    }
            return true;
        }

        void ReadNodes(TreeNode tree, StreamReader reader)
        {
            char c = ReadNext(reader);
            while (true)
            {
                if (c == ';')
                {
                    c = ReadNode(tree,reader);
                    if (tree.HasChildren()) tree = tree.LastChild;
                    continue;
                }
                else if (c == '(')
                {
                    ReadNodes(tree,reader);
                }
                else if (c == ')') break;
                c = ReadNext(reader);
            }
        }

        private void Store(char c)
        {
            try
            {
                Buffer[BufferN] = c;
                BufferN++;
            }
            catch (IndexOutOfRangeException e)
            {
                int newLength = Buffer.Length + maxbuffer;
                char[] newBuffer = new char[newLength];
                System.Array.Copy(Buffer, 0, newBuffer, 0, Buffer.Length);
                Buffer = newBuffer;
                Buffer[BufferN++] = c;
            }
        }

        public static List<SGFTree> Load(StreamReader reader)
        {
            List<SGFTree> list = new List<SGFTree>();
            bool lineStart = true;
            int c;
            while (true)
            {
                SGFTree tree = new SGFTree(new TreeNode(1));
                while (true)
                {
                    try
                    {
                        c = tree.ReadChar(reader);
                    }
                    catch (System.Exception ex)
                    {
                        goto goon;
                    }
                    if (lineStart && c == '(') break;
                    if (c == '\n') lineStart = true;
                    else lineStart = false;
                }
                tree.ReadNodes(tree.root, reader);
                list.Add(tree);
            }
            goon:
            return list;
        }
    }
}
