using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using GameDevTV.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private ActionScheduler _actionScheduler;
        [SerializeField] private Mover _mover;
        [SerializeField] private WeaponConfig _defaultWeaponConfig;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        
        private Health _target;
        private Equipment _equipment;
        private Animator _animator;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int AttackAnimatorHash = Animator.StringToHash("Attack");
        private static readonly int StopAttackAnimatorHash = Animator.StringToHash("StopAttack");
        private WeaponConfig _currentWeaponConfig;
        private LazyValue<Weapon> _currentWeapon;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _currentWeaponConfig = _defaultWeaponConfig;
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);

            _equipment = GetComponent<Equipment>();
            if (_equipment != null)
            {
                _equipment.equipmentUpdated += UpdateWeapon;
            }
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(_defaultWeaponConfig);
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            _currentWeaponConfig = weaponConfig;
            _currentWeapon.value = AttachWeapon(weaponConfig);
        }

        private void UpdateWeapon()
        {
            var weaponConfig = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;

            EquipWeapon(weaponConfig != null ? weaponConfig : _defaultWeaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig)
        {
            return weaponConfig.Spawn(_rightHandTransform, _leftHandTransform, _animator);
        }

        public Health GetTarget()
        {
            return _target;
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
                
            if (_target == null || _target.HasDied)
            {
                return;
            }
            
            if (!InRange(_target.transform))
            {
                _mover.MoveTo(_target.transform.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private bool InRange(Transform target)
        {
            return Vector3.Distance(target.transform.position, transform.position) < _currentWeaponConfig.WeaponRange;
        }
        
        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > _currentWeaponConfig.TimeBetweenAttacks)
            {
                _animator.ResetTrigger(StopAttackAnimatorHash);
                _animator.SetTrigger(AttackAnimatorHash);
                _timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null)
            {
                return false;
            }
            
            if (!InRange(combatTarget.transform) && !GetComponent<Mover>().CanMoveTo(combatTarget.transform.position))
            {
                return false;
            }
            
            return !combatTarget.GetComponent<Health>().HasDied;
        }

        //BUG: We keep attacking while a target is in the dying animation
        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Hit()
        {
            if (_target == null)
            {
                return;
            }

            var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (_currentWeapon.value != null)
            {
                _currentWeapon.value.OnHit();
            }
            
            if (_currentWeaponConfig.HasProjectile)
            {
                _currentWeaponConfig.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }

        public void Cancel()
        {
            StopAttack();
            _mover.Cancel();
            _target = null;
        }

        private void StopAttack()
        {
            _animator.ResetTrigger(AttackAnimatorHash);
            _animator.SetTrigger(StopAttackAnimatorHash);
        }
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.WeaponDamage;
            }
        }
        
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.PercentageBonus;
            }
        }

        #region Saving
        
        public object CaptureState()
        {
            return _currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            var weaponName = (string)state;
            var weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

        #endregion

    }
}

