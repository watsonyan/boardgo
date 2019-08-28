using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.logic
{
    public class Field
    {
        int color;
        bool mark;
        TreeNode tree;
        int letter;
        string labelLetter;
        bool hasLabel;
        int marker;

        readonly static int az = 'z' - 'a';

        public Field()
        {
            color = 0;
            tree = null;
            letter = 0;
            hasLabel = false;

        }

        public int Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public bool Mark
        {
            get
            {
                return mark;
            }
            set
            {
                mark = value;
            }
        }

        public TreeNode Tree
        {
            get
            {
                return tree;
            }
            set
            {
                tree = value;
            }
        }

        public int Marker
        {
            get
            {
                return marker;
            }
            set
            {
                marker = value;
            }
        }

        public int Letter
        {
            get
            {
                return letter;
            }
            set
            {
                letter = value;
            }
        }

        public string LabelLetter
        {
            get
            {
                return labelLetter;
            }
            set
            {
                hasLabel = true;
                labelLetter = value;
            }
        }

        public bool HasLabel
        {
            get
            {
                return hasLabel;
            }
            set
            {
                hasLabel = value;
            }
        }

        public static string GetCoordinateStr(int i, int j)
        {
            char[] rlt = new char[2];
            rlt[0] = 'f';
            rlt[1] = 'f';

            return new string(rlt);
        }

        public static int i(string s)
        {
            if (s.Length < 2) return -1;
            char c = s[0];
            if (c < 'a') return c - 'A' + az + 1;
            return c - 'a';
        }

        public static int j(string s)
        {
            if (s.Length < 2) return -1;
            char c = s[1];
            if (c < 'a') return c - 'A' + az + 1;
            return c - 'a';
        }

    }
}
