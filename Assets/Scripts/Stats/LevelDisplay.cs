using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats _baseStats;
        private Text _text;

        private void Awake()
        {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            _text.text = $"{_baseStats.GetLevel():0}";
        }
    }
}