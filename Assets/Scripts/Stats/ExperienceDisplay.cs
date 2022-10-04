using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience _experience;
        private Text _text;

        private void Awake()
        {
            _experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            _text.text = $"{_experience.ExperiencePoints:0}";
        }
    }
}