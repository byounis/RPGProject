using System;
using RPG.Attributes;
using RPG.Combat;
using RPG.Helpers;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable]
        struct CursorMapping
        {
            public CursorType CursorType;
            public Texture2D Texture;
            public Vector2 HotSpot;
        }

        [SerializeField] private CursorMapping[] _cursorMappings;
        [SerializeField] private int _maxDistanceNavMeshProjection = 1;
        [SerializeField] private float _maxPathLength = 40f;
        
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

            if (InteractWithComponent())
            {
                return;
            }

            if (InteractWithMovement())
            {
                return;
            }
            
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            var hits = RaycastAllSorted();
            
            foreach (var hit in hits)
            {
                var raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
                
            }
            
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            var distances = new float[hits.Length];
            for (var index = 0; index < hits.Length; index++)
            {
                distances[index] = hits[index].distance;
            }

            Array.Sort(distances, hits);
            return hits;
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

        private bool InteractWithMovement()
        {
            // var ray = GetMouseRay();
            // var isHit = Physics.Raycast(ray, out var hitInfo);
            var isHit = RaycastNavMesh(out var target);

            if (!isHit)
            {
                return false;
            }
            
            if (Input.GetMouseButton(0))
            {
                _mover.StartMoveAction(target);
            }
            
            SetCursor(CursorType.Movement);

            return true;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = Vector3.zero;
            var ray = GetMouseRay();
            var isRaycastHit = Physics.Raycast(ray, out var hitInfo);

            if (!isRaycastHit)
            {
                return false;
            }

            var isNavMeshHit = NavMesh.SamplePosition(hitInfo.point, out var navMeshHit, _maxDistanceNavMeshProjection,
                NavMesh.AllAreas);
            if(!isNavMeshHit)
            {
                return false;
            }
            
            target = navMeshHit.position;

            var path = new NavMeshPath();
            var hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            if (!hasPath || path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }

            if (path.GetPathLength() > _maxPathLength)
            {
                return false;
            }

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
