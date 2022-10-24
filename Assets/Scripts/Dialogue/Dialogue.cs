using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "RPG/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> _dialogueNodes;

#if UNITY_EDITOR
        private void Awake()
        {
            if (_dialogueNodes.Count == 0)
            {
                _dialogueNodes.Add(new DialogueNode());
            }
        }
#endif

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return _dialogueNodes;
        }
    }
}