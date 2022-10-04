using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;
        private Text _text;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            var target = _fighter.GetTarget();
            if (target == null)
            {
                _text.text = "N/A";
                return;
            }
            
            var healthPoints = target.HealthPoints;
            var maxHealthPoints = target.MaxHealthPoints;
            
            _text.text = $"{healthPoints:0}/{maxHealthPoints:0}";
        }
    }
}