using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.logic
{
    public class Node
    {
        LinkedList<Action> actions;
        bool isMain;
        int eptTreeNum;//is the number of the next expected move in the game tree

        public Node(int num)
        {
            actions = new LinkedList<Action>();
            eptTreeNum = num;
            isMain = false;
        }

        //public Node(Node node)
        //{
        //    //actions = new LinkedList<Action>(node.Actions);
        //    //eptTreeNum = node.EptTreeNum;
        //    //isMain = node.IsMain;
        //}

        public bool IsMain
        {
            get
            {
                return isMain;
            }
            set
            {
                isMain = value;
            }
        }
        
        public int EptTreeNum
        {
            get
            {
                return eptTreeNum;
            }
            set
            {
                eptTreeNum = value;
            }
        }

        public LinkedListNode<Action> First
        {
            get
            {
                return actions.First;
            }
        }

        public Action FirstAction
        {
            get
            {   
                if (actions.First != null)
                {
                    return actions.First.Value;
                }
                return null;
            }
        }
        
        public Action LastAction
        {
            get
            {
                if (actions.Last != null)
                {
                    return actions.Last.Value;
                }
                return null;
            }
        }

        public LinkedList<Action> Actions
        {
            get
            {
                return actions;
            }
            set
            {
                actions = value;
            }
        }

        public void AddAction(Action action)
        {
            actions.AddLast(action);
        }

        public void ExpandAction(Action action)
        {
            Action targetAction = FindActionByType(action.Type);
            if (targetAction == null)
            {
                AddAction(action);
            }
            else
            {
                targetAction.AddArugment(action.Arg);
            }
        }

        public void ToggleAction(Action action)
        {
            Action targetAction = FindActionByType(action.Type);
            if (targetAction == null)
            {
                AddAction(action);
            }
            else
            {
                targetAction.ToggleArgument(action.Arg);
            }
        }

        public Action FindActionByType(string type)
        {
            var node = actions.First;
            while (node != null)
            {
                string v = node.Value.Type;
                if (v.Equals(type))
                {
                    return node.Value;
                }
                node = node.Next;
            }
            return null;
        }

        public LinkedListNode<Action> FindActionNode(Action action)
        {
            var node = actions.First;
            while (node != null)
            {
                string v = node.Value.Type;
                if (v.Equals(action.Type))
                {
                    return node;
                }
                node = node.Next;
            }
            return null;
        }

        public bool ContainsAction(string type, string arg)
        {
            Action targetAction = FindActionByType(type);
            if (targetAction == null)
            {
                return false;
            }
            return targetAction.ContainsArg(arg);
        }

        public void PrependAction(Action action)
        {
            actions.AddFirst(action);
        }

        public void InsertActionAfter(Action after, Action action)
        {
            var node = FindActionNode(after);
            actions.AddAfter(node, action);
        }

        public void RemoveAction(Action action)
        {
            var node = FindActionNode(action);
            actions.Remove(node);
        }

        public void SetAction(string type, string arg)
        {
            SetAction(type, arg, false);
        }

        public void SetAction(string type, string arg, bool front)
        {
            Action targetAction = FindActionByType(type);
            if (targetAction == null)
            {
                if (front)
                {
                    PrependAction(new Action(type, arg));
                }
                else
                {
                    AddAction(new Action(type, arg));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(arg))
                {
                    actions.Remove(targetAction);
                }
                else
                {
                    LinkedListNode<string> args = targetAction.Args;
                    if (args == null)
                    {
                        targetAction.AddArugment(arg);
                    }
                    else
                    {
                        targetAction.Arg = arg;
                    }
                }
            }
        }
             
    }
}
