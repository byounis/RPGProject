using System;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private RectTransform _foreground;
        [SerializeField] private Canvas _rootCanvas;
        
        

        private void Update()
        {
            var healthFraction = _health.GetFraction();
            if (Mathf.Approximately(healthFraction, 0f) || Mathf.Approximately(healthFraction, 1f))
            {
                _rootCanvas.enabled = false;
                return;
            }
            
            _rootCanvas.enabled = true;
            _foreground.localScale = new Vector3(healthFraction, 1, 1);
        }
    }
}