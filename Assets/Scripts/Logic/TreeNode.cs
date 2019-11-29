using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace game.logic
{
    public class TreeNode : Node
    {
        LinkedList<TreeNode> children;
        TreeNode parent;


        public TreeNode(int num) : base(num)
        {
            children = new LinkedList<TreeNode>();
            parent = null;
        }

        public void SetNode(Node node)
        {
            this.EptTreeNum = node.EptTreeNum;
            this.Actions = node.Actions;
            this.IsMain = node.IsMain;
        }

        public TreeNode(Node node) : base(node)
        {
            children = new LinkedList<TreeNode>();
            parent = null;
        }

        public void SetMain(TreeNode node)
        {
            IsMain = false;
            try
            {
                if (node.IsMain)
                {
                    IsMain = (this == node.FirstChild);
                }
                else if(node.Parent == null)
                {
                    IsMain = true;
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        public TreeNode Parent
        {
            get
            {
                return parent;
            }
        }

        public TreeNode FirstChild
        {
            get
            {
                return children.First.Value;
            }
        }

        public TreeNode LastChild
        {
            get
            {
                return children.Last.Value;
            }
        }

        public LinkedList<TreeNode> Children
        {
            get
            {
                return children;
            }
        }

        public void AddChild(TreeNode tNode)
        {
            children.AddLast(tNode);
            tNode.parent = this;
        }

        public void InsertChild(TreeNode tNode)
        {
            if (!HasChildren())
            {
                AddChild(tNode);
            }
            else
            {
                // give t my children
                tNode.children = children;
                // make t my only child
                children = new LinkedList<TreeNode>();
                children.AddLast(tNode);
                tNode.parent = this;
                // fix the parents of all grandchildren
                var node = tNode.children.First;
                while (node != null)
                {
                    TreeNode h = node.Value;                    
                    h.parent = tNode;
                    node = node.Next;
                }
            }
        }

        /** remove the specific child tree (must be in the tree!!!) */
        public void Remove(TreeNode tNode)
        {
            if (tNode.Parent != this) return;
            children.Remove(tNode);
        }

        /** remove all children */
        public void Clear()
        {
            children.Clear();
        }

        public bool HasChildren()
        {
            return children.First != null;
        }

        public bool IsLastMain()
        {
            return !HasChildren() && IsMain;
        }
    }
}