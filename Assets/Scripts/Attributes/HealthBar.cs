using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private RectTransform _foreground;
        
        private void Update()
        {
            _foreground.localScale = new Vector3(_health.GetFraction(), 1, 1);
        }
    }
}