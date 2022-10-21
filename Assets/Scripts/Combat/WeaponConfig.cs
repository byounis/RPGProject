using System;
using GameDevTV.Inventories;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG Project/New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private AnimatorOverrideController _animatorOverride;
        
        [SerializeField] private float _weaponRange;
        [SerializeField] private float _percentageBonus;
        [SerializeField] private float _timeBetweenAttacks;
        [SerializeField] private float _weaponDamage;
        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private Projectile _projectile;
        
        public float WeaponRange => _weaponRange;
        public float TimeBetweenAttacks => _timeBetweenAttacks;
        public float WeaponDamage => _weaponDamage;
        public float PercentageBonus => _percentageBonus;
        public bool HasProjectile => _projectile != null;

        private const string WeaponName = "Weapon";
        
        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;
            
            if (_weapon != null)
            {
                var transform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(_weapon, transform);
                weapon.gameObject.name = WeaponName;
            }
            
            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
                return weapon;
            }
            
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
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