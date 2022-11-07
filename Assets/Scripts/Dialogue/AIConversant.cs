using System;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Dialogue _dialogue;
        
        private DialogueTrigger[] _dialogueTriggers;

        private void Start()
        {
            _dialogueTriggers = GetComponents<DialogueTrigger>();
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (_dialogue == null)
            {
                return false;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, _dialogue);
            }
            
            return true;
        }

        public void TriggerAction(string action)
        {
            foreach (var dialogueTrigger in _dialogueTriggers)
            {
                dialogueTrigger.Trigger(action);
            }
        }
    }
}