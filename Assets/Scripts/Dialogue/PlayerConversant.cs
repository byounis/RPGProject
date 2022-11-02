using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue _currentDialogue;

        private DialogueNode _currentNode;

        private void Awake()
        {
            _currentNode = _currentDialogue.GetRootNode();
        }

        public string GetText()
        {
            if (_currentDialogue == null)
            {
                return string.Empty;
            }
            
            return _currentNode.GetText();
        }

        public void Next()
        {
            var childNodes = _currentDialogue.GetAllChildren(_currentNode).ToArray();
            _currentNode = childNodes[Random.Range(0, childNodes.Length)];
        }

        public bool HasNext()
        {
            return _currentNode.GetChildren().Count > 0;
        }
    }
}