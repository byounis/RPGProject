using GameDevTV.Inventories;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far can the pickups be scattered from the droppper.")]
        [SerializeField] private float _scatterDistance = 1;
        [SerializeField] private InventoryItem[] _dropLibrary;
        [SerializeField] private int _numberOfDrops = 2;
        
        private const int Attempts = 30;

        public void RandomDrop()
        {
            for (var i = 0; i < _numberOfDrops; i++)
            {
                var item = _dropLibrary[Random.Range(0, _dropLibrary.Length)];
                DropItem(item, 1);
            }
        }
        
        protected override Vector3 GetDropLocation()
        {
            for (var i = 0; i < Attempts; i++)
            {
                var randomPoint = transform.position + Random.insideUnitSphere * _scatterDistance;
                if (NavMesh.SamplePosition(randomPoint, out var navMeshHit, 0.1f, NavMesh.AllAreas))
                {
                    return navMeshHit.position;
                }
            }

            return transform.position;
        }
    }
}