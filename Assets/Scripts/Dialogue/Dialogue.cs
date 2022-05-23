using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BlackCat.Dialogue
{



    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Black Cat/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject , ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] float CanvaxWidth = 4000f;
        [SerializeField] float CanvaxHeight = 4000f;
        [SerializeField] Vector2 newNodeOffset = new Vector2(250,0);

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
        private void Awake()
        {
#if UNITY_EDITOR

            if (nodes.Count == 0)
            {
               CreateNode();
            }
#endif
        }

        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
            }
        }
        public float GetCanvasWidth()
        {
            return CanvaxWidth; 
        }
        public float GetCanvasHeight()
        {
            return CanvaxHeight;
        }
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent)
        {
            foreach(string childId in parent.GetChildren())
            {
                if (nodeLookup.ContainsKey(childId))
                    yield return nodeLookup[childId];
            }                        
        }

#if UNITY_EDITOR
        public DialogueNode CreateNode(DialogueNode parent = null)
        {
            DialogueNode newNode = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            AddNode(newNode);
            return newNode;

        }




        public void DeleteNode(DialogueNode nodeToDelete)
        {
            if (nodes.Exists(p => p.name == nodeToDelete.name))
            {
                Undo.RecordObject(this, "Removing Dialogue Node");
                nodes.Remove(nodeToDelete);
                OnValidate();
                CleanDanglingChildren(nodeToDelete);
                Undo.DestroyObjectImmediate(nodeToDelete);

            }
        }
        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                parent.AddChild(newNode.name);               
                newNode.SetPosition(parent.GetRect().position + newNodeOffset);
            }

            return newNode;
        }
        private void AddNode(DialogueNode newNode)
        {
            nodes.Add(newNode);
            OnValidate();
        }
        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach(DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }
        public void OnAfterDeserialize()
        {

        }
    }
}
