using System;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] private Dialogue _currentDialogue;

        public string GetText()
        {
            if (_currentDialogue == null)
            {
                return string.Empty;
            }
            
            return _currentDialogue.GetRootNode().GetText();
        }
    }
}