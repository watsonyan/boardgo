using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace game.logic
{
    public class Action//property
    {
        string type;
        LinkedList<string> arguments;

        public Action(string type)
        {
            this.type = type;
            arguments = new LinkedList<string>();
        }

        public Action(string type, string arg)
        {
            this.type = type;
            arguments = new LinkedList<string>();
            AddArugment(arg);
        }

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public string Arg
        {
            get
            {
                if (arguments.First != null)
                    return arguments.First.Value;
                return string.Empty;
            }
            set
            {
                if (arguments.First != null)
                    arguments.First.Value = value;
            }
        }

        public LinkedListNode<string> Args
        {
            get
            {
                return arguments.First;
            }
        }
        


        public void AddArugment(string arg)
        {
            arguments.AddLast(arg);
        }

        public void ToggleArgument(string arg)
        {
            var node = arguments.First;
            while (node != null)
            {
                string v = node.Value;
                if (v.Equals(arg))
                {
                    arguments.Remove(node);
                    return;
                }
            }
            arguments.AddLast(arg);
        }

        public bool ContainsArg(string arg)
        {
            return arguments.Contains(arg);
        }

        public bool isRelevant()
        {
            if (type.Equals("GN")
                || type.Equals("AP")
                || type.Equals("FF")
                || type.Equals("GM")
                || type.Equals("N")
                || type.Equals("SZ")
                || type.Equals("PB")
                || type.Equals("BR")
                || type.Equals("PW")
                || type.Equals("WR")
                || type.Equals("HA")
                || type.Equals("KM")
                || type.Equals("RE")
                || type.Equals("DT")
                || type.Equals("TM")
                || type.Equals("US")
                || type.Equals("CP")
                || type.Equals("BL")
                || type.Equals("WL")
                || type.Equals("C")
                )
                return false;
            else return true;
        }

        public string Export()
        {
            StringBuilder res = new StringBuilder("");

            res.Append(this.Type);
            LinkedListNode<string> argsNode = this.Args;
            while (argsNode != null)
            {
                //ExportSgf(actionNode.Value);
                res.Append("[");
                res.Append(argsNode.Value);
                res.Append("]");
                argsNode = argsNode.Next;
            }

            return res.ToString();
        }
    }
}

