using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health _health;
        private Text _text;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            _text.text = $"{_health.HealthPoints:0}/{_health.MaxHealthPoints:0}";
        }
    }
}