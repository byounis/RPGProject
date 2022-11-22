using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private string _playerName;
        
        private Dialogue _currentDialogue;
        private DialogueNode _currentNode;
        private bool _isChoosing;
        private AIConversant _currentConversant;
        
        public event Action OnConversationUpdated;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            _currentConversant = newConversant;
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            TriggerEnterAction();
            OnConversationUpdated.Invoke();
        }

        public void Quit()
        {
            TriggerExitAction();
            _currentConversant = null;
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
            return FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            _currentNode = chosenNode;
            TriggerEnterAction();
            _isChoosing = false;
            Next();
        }

        public void Next()
        {
            var numberOfPlayerResponses = FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode)).Count();

            if (numberOfPlayerResponses > 0)
            {
                _isChoosing = true;
                TriggerExitAction();
                OnConversationUpdated.Invoke();
                return;
            }
            
            _isChoosing = false;
            var childNodes = FilterOnCondition(_currentDialogue.GetAIChildren(_currentNode)).ToArray();
            TriggerExitAction();
            _currentNode = childNodes[Random.Range(0, childNodes.Length)];
            TriggerEnterAction();
            OnConversationUpdated.Invoke();
        }

        public bool HasNext()
        {
            return _currentNode.GetChildren().Count > 0;
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void TriggerEnterAction()
        {
            if (_currentNode != null)
            {
                TriggerAction(_currentNode.GetEnterAction());
            }
        }
        
        private void TriggerExitAction()
        {
            if (_currentNode != null)
            {
                TriggerAction(_currentNode.GetExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (action == string.Empty)
            {
                return;
            }
            
            _currentConversant.TriggerAction(action);
        }

        public string GetCurrentConversantName()
        {
            return _isChoosing ? _playerName : _currentConversant.ConversantName;
        }
    }
}