using UnityEngine;

namespace RPG.UI
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;
        
        public void Spawn(float damageAmount)
        {
            var damageTextInstance = Instantiate(_damageTextPrefab, transform);
            damageTextInstance.SetText(damageAmount.ToString());
        }
    }
}