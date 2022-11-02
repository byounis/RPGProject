using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue _currentDialogue;

        private DialogueNode _currentNode;
        private bool _isChoosing;

        private void Awake()
        {
            _currentNode = _currentDialogue.GetRootNode();
        }

        public bool IsChoosing()
        {
            return _isChoosing;
        }

        public string GetText()
        {
            if (_currentDialogue == null)
            {
                return string.Empty;
            }
            
            return _currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return _currentDialogue.GetPlayerChildren(_currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;
            Next();
        }

        public void Next()
        {
            var numberOfPlayerResponses = _currentDialogue.GetPlayerChildren(_currentNode).Count();

            if (numberOfPlayerResponses > 0)
            {
                _isChoosing = true;
                return;
            }
            
            _isChoosing = false;
            var childNodes = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            _currentNode = childNodes[Random.Range(0, childNodes.Length)];
        }

        public bool HasNext()
        {
            return _currentNode.GetChildren().Count > 0;
        }
    }
}