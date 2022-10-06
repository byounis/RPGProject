using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        enum CursorType
        {
            None,
            Movement,
            Combat,
            UI
        }

        [Serializable]
        struct CursorMapping
        {
            public CursorType CursorType;
            public Texture2D Texture;
            public Vector2 HotSpot;
        }

        [SerializeField] private CursorMapping[] _cursorMappings;
        
        private Mover _mover;
        private Fighter _fighter;
        private Camera _mainCamera;
        private Health _health;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }
    
        private void Update()
        {
            if (InteractWithUI())
            {
                return;
            }
            
            if (_health.HasDied)
            {
                SetCursor(CursorType.None);
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
            
            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            
            return false;
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

                SetCursor(CursorType.Combat);
                
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
            
            SetCursor(CursorType.Movement);

            return true;
        }

        private void SetCursor(CursorType cursorType)
        {
            var cursorMapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursorMapping.Texture, cursorMapping.HotSpot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (var cursorMapping in _cursorMappings)
            {
                if (cursorMapping.CursorType == cursorType)
                {
                    return cursorMapping;
                }
            }

            return default;
        }
        
        private Ray GetMouseRay()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}
