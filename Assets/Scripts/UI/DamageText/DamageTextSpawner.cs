using System;
using UnityEngine;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;
        
        private void Start()
        {
            Spawn(12);
        }

        public void Spawn(float damageAmount)
        {
            var damageTextInstance = Instantiate(_damageTextPrefab, transform);
        }
    }
}