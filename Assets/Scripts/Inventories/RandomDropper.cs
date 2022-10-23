using GameDevTV.Inventories;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far can the pickups be scattered from the dropper.")]
        [SerializeField] private float _scatterDistance = 1;
        [SerializeField] private DropLibrary _dropLibrary;
        
        private const int Attempts = 30;

        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();
            var drops = _dropLibrary.GetRandomDrops(baseStats.GetLevel());
            foreach (var drop in drops)
            {
                DropItem(drop.Item, drop.Number);
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