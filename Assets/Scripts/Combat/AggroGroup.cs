using System;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField] private Fighter[] _fighters;
        [SerializeField] private bool _activateOnStart;

        private void Start()
        {
            Activate(_activateOnStart);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (var fighter in _fighters)
            {
                var target = fighter.GetComponent<CombatTarget>();
                if (target != null)
                {
                    target.enabled = shouldActivate;
                }
                
                fighter.enabled = shouldActivate;
            }
        }
    }
}