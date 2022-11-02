using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        private Dialogue _currentDialogue;
        private DialogueNode _currentNode;
        private bool _isChoosing;
        
        public event Action OnConversationUpdated;

        public void StartDialogue(Dialogue newDialogue)
        {
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            OnConversationUpdated.Invoke();
        }

        public void Quit()
        {
            _currentDialogue = null;
            _currentNode = null;
            _isChoosing = false;
            OnConversationUpdated.Invoke();
        }

        public bool IsDialogueActive()
        {
            return _currentDialogue != null;
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
            _isChoosing = false;
            Next();
        }

        public void Next()
        {
            var numberOfPlayerResponses = _currentDialogue.GetPlayerChildren(_currentNode).Count();

            if (numberOfPlayerResponses > 0)
            {
                _isChoosing = true;
                OnConversationUpdated.Invoke();
                return;
            }
            
            _isChoosing = false;
            var childNodes = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            _currentNode = childNodes[Random.Range(0, childNodes.Length)];
            OnConversationUpdated.Invoke();
        }

        public bool HasNext()
        {
            return _currentNode.GetChildren().Count > 0;
        }
    }
}