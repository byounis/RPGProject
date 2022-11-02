using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> _dialogueNodes = new List<DialogueNode>();
        [SerializeField] private Vector2 _newNodeOffset = new Vector2(250, 0);
        
        private readonly Dictionary<string, DialogueNode> _dialogueNodeLookup = new Dictionary<string, DialogueNode>();

        private void OnValidate()
        {
            _dialogueNodeLookup.Clear();
            foreach (var dialogueNode in GetAllNodes())
            {
                _dialogueNodeLookup.Add(dialogueNode.name, dialogueNode);
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return _dialogueNodes;
        }

        public DialogueNode GetRootNode()
        {
            return _dialogueNodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode dialogueNode)
        {
            foreach (var id in dialogueNode.GetChildren())
            {
                if (_dialogueNodeLookup.ContainsKey(id))
                {
                    yield return _dialogueNodeLookup[id];
                }
            }
        }
        
        
        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode dialogueNode)
        {
            foreach (var childNode in GetAllChildren(dialogueNode))
            {
                if (childNode.IsPlayerSpeaking())
                {
                    yield return childNode;
                }
            }
        }
        
        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode dialogueNode)
        {
            foreach (var childNode in GetAllChildren(dialogueNode))
            {
                if (!childNode.IsPlayerSpeaking())
                {
                    yield return childNode;
                }
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parentNode)
        {
            var newNode = MakeNode(parentNode);

            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");

            AddNode(newNode);
        }


        private static DialogueNode MakeNode()
        {
            return CreateInstance<DialogueNode>();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            _dialogueNodes.Remove(nodeToDelete);
            OnValidate();
            RemoveChildFromAllNodes(nodeToDelete);
            
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void RemoveChildFromAllNodes(DialogueNode nodeToDelete)
        {
            foreach (var node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }
        
        private void AddNode(DialogueNode newNode)
        {
            _dialogueNodes.Add(newNode);
            OnValidate();
        }

        private DialogueNode MakeNode(DialogueNode parentNode)
        {
            var newNode = MakeNode();
            newNode.name = Guid.NewGuid().ToString();

            if (parentNode != null)
            {
                parentNode.AddChild(newNode.name);
                newNode.SetIsPlayerSpeaking(!parentNode.IsPlayerSpeaking());
                newNode.SetPosition(parentNode.GetRect().position + _newNodeOffset);
            }

            return newNode;
        }
#endif

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (_dialogueNodes.Count == 0)
            {
                var newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != string.Empty)
            {
                foreach (var dialogueNode in _dialogueNodes)
                {
                    if (AssetDatabase.GetAssetPath(dialogueNode) == string.Empty)
                    {
                        AssetDatabase.AddObjectToAsset(dialogueNode, this);
                    }
                }
            }
#endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize(){}
    }
}