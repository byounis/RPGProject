using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private string _action;
        [SerializeField] private UnityEvent _onTrigger;

        public void Trigger(string action)
        {
            if (action == _action)
            {
                _onTrigger.Invoke();
            }
        }
    }
}