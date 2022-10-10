using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
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
        private Animator _animator;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int AttackAnimatorHash = Animator.StringToHash("Attack");
        private static readonly int StopAttackAnimatorHash = Animator.StringToHash("StopAttack");
        private LazyValue<WeaponConfig> _currentWeapon;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _currentWeapon = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
        }

        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(_defaultWeaponConfig);
            return _defaultWeaponConfig;
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            _currentWeapon.value = weaponConfig;
            AttachWeapon(weaponConfig);
        }

        private void AttachWeapon(WeaponConfig weaponConfig)
        {
            weaponConfig.Spawn(_rightHandTransform, _leftHandTransform, _animator);
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
            
            if (!InRange())
            {
                _mover.MoveTo(_target.transform.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private bool InRange()
        {
            return Vector3.Distance(_target.transform.position, transform.position) < _currentWeapon.value.WeaponRange;
        }
        
        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > _currentWeapon.value.TimeBetweenAttacks)
            {
                _animator.ResetTrigger(StopAttackAnimatorHash);
                _animator.SetTrigger(AttackAnimatorHash);
                _timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
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
            if (_currentWeapon.value.HasProjectile)
            {
                _currentWeapon.value.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject, damage);
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
                yield return _currentWeapon.value.WeaponDamage;
            }
        }
        
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.PercentageBonus;
            }
        }

        #region Saving
        
        public object CaptureState()
        {
            return _currentWeapon.value.name;
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

