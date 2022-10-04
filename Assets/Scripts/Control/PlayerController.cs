using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private Fighter _fighter;
    
        private Camera _mainCamera;
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }
    
        private void Update()
        {
            if (_health.HasDied)
            {
                return;
            }
            
            if (InteractWithCombat())
            {
                return;
            }

            if (InteractWithMovement())
            {
                return;
            }
        }

        private bool InteractWithCombat()
        {
            var ray = GetMouseRay();
            var hits = Physics.RaycastAll(ray);

            foreach (var hit in hits)
            {
                if (!hit.transform.TryGetComponent<CombatTarget>(out var combatTarget))
                {
                    continue;
                }
                
                if(!_fighter.CanAttack(combatTarget.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(combatTarget.gameObject);
                }
                
                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            var ray = GetMouseRay();
            var isHit = Physics.Raycast(ray, out var hitInfo);

            if (!isHit)
            {
                return false;
            }
            
            if (Input.GetMouseButton(0))
            {
                _mover.StartMoveAction(hitInfo.point);
            }

            return true;

        }

        private Ray GetMouseRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
