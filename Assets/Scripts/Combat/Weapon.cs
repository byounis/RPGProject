using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG Project/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject _equippedPrefab;
        [SerializeField] private AnimatorOverrideController _animatorOverride;
        
        [SerializeField] private float _weaponRange;
        [SerializeField] private float _timeBetweenAttacks;
        [SerializeField] private float _weaponDamage;
        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private Projectile _projectile;
        
        public float WeaponRange => _weaponRange;
        public float TimeBetweenAttacks => _timeBetweenAttacks;
        public float WeaponDamage => _weaponDamage;
        public bool HasProjectile => _projectile != null;

        private const string WeaponName = "Weapon";
        
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            
            if (_equippedPrefab != null)
            {
                var transform = GetTransform(rightHand, leftHand);
                var weapon = Instantiate(_equippedPrefab, transform);
                weapon.name = WeaponName;
            }
            
            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
                return;
            }
            
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(WeaponName) ?? leftHand.Find(WeaponName);

            if (oldWeapon != null)
            {
                oldWeapon.name = "DestroyingWeapon";
                Destroy(oldWeapon.gameObject);
            }
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return _isRightHanded ? rightHand : leftHand;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            var transform = GetTransform(rightHand, leftHand);
            var projectileInstance = Instantiate(_projectile, transform.position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }
    }
}