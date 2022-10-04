using System.Collections.Generic;
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
        [SerializeField] private Weapon _defaultWeapon;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        
        private Health _target;
        private Animator _animator;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int AttackAnimatorHash = Animator.StringToHash("Attack");
        private static readonly int StopAttackAnimatorHash = Animator.StringToHash("StopAttack");
        private Weapon _currentWeapon;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            // If saving system has not given us a weapon we spawn the default weapon
            if(_currentWeapon == null)
            {
                EquipWeapon(_defaultWeapon);
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            weapon.Spawn(_rightHandTransform, _leftHandTransform, _animator);
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
            return Vector3.Distance(_target.transform.position, transform.position) < _currentWeapon.WeaponRange;
        }
        
        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > _currentWeapon.TimeBetweenAttacks)
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
            if (_currentWeapon.HasProjectile)
            {
                _currentWeapon.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject, damage);
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
                yield return _currentWeapon.WeaponDamage;
            }
        }
        
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.PercentageBonus;
            }
        }

        #region Saving
        
        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            var weaponName = (string)state;
            var weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        #endregion

    }
}

