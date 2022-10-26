using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> _dialogueNodes = new List<DialogueNode>();

        private readonly Dictionary<string, DialogueNode> _dialogueNodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake()
        {
            if (_dialogueNodes.Count != 0)
            {
                return;
            }
            
            var rootNode = new DialogueNode
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            
            _dialogueNodes.Add(rootNode);
        }
#else
        private void Awake()
        {
            OnValidate();
        }
#endif

        private void OnValidate()
        {
            _dialogueNodeLookup.Clear();
            foreach (var dialogueNode in GetAllNodes())
            {
                _dialogueNodeLookup.Add(dialogueNode.UniqueID, dialogueNode);
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
            foreach (var id in dialogueNode.Children)
            {
                if (_dialogueNodeLookup.ContainsKey(id))
                {
                    yield return _dialogueNodeLookup[id];
                }
            }
        }

        public void CreateNode(DialogueNode parentNode)
        {
            var newNode = new DialogueNode
            {
                UniqueID = Guid.NewGuid().ToString()
            };
            
            _dialogueNodes.Add(newNode);
            parentNode.Children.Add(newNode.UniqueID);
            
            OnValidate();
        }
    }
}