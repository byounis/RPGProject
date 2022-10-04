using JetBrains.Annotations;
using RPG.Combat;
using UnityEngine;

namespace RPG.Helpers
{
    public class AnimationEvents : MonoBehaviour
    { 
        private Fighter _fighter;

        private void Awake()
        {
            _fighter = GetComponentInParent<Fighter>();
        }

        // Animation Event
        [UsedImplicitly]
        public void Hit()
        {
            _fighter.Hit();
        }
        
        // Animation Event
        // Animation event is named shoot in the animation although everything else is hit
        [UsedImplicitly]
        public void Shoot()
        {
            _fighter.Hit();
        }
    }
}

