using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Inventories
{
    [CreateAssetMenu(fileName = "DropLibrary", menuName = "RPG/Inventory/Drop Library")]
    public class DropLibrary : ScriptableObject
    {
        [Serializable]
        class DropConfig
        {
            public InventoryItem Item;
            public float[] RelativeChance;
            public int[] MinimumNumber;
            public int[] MaximumNumber;

            public int GetRandomNumber(int level)
            {
                if (!Item.IsStackable())
                {
                    return 1;
                }
                
                var min = GetByLevel(MinimumNumber, level);
                var max = GetByLevel(MaximumNumber, level);
                return Random.Range(min, max);
            }
        }

        public struct Dropped
        {
            public InventoryItem Item;
            public int Number;
        }

        [SerializeField] private DropConfig[] _potentialDrops;
        [SerializeField] private float[] _dropChancePercentage;
        [SerializeField] private int[] _minimumDrops;
        [SerializeField] private int[] _maximumDrops;

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }

            for (var i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }
        
        private bool ShouldRandomDrop(int level)
        {
            var dropChance = GetByLevel(_dropChancePercentage, level);
            var randomRoll = Random.Range(0f, 100f);

            return randomRoll < dropChance;
        }
        
        private int GetRandomNumberOfDrops(int level)
        {
            var min = GetByLevel(_minimumDrops, level);
            var max = GetByLevel(_maximumDrops, level);

            return Random.Range(min, max);
        }
        
        private Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            var result = new Dropped()
            {
                Item = drop.Item,
                Number = drop.GetRandomNumber(level)
            };
            
            return result;
        }

        private DropConfig SelectRandomItem(int level)
        {
            var totalChance = GetTotalChance(level);
            var randomRoll = Random.Range(0f, totalChance);
            var chanceTotal = 0f;
            foreach (var drop in _potentialDrops)
            {
                chanceTotal += GetByLevel(drop.RelativeChance, level);
                if (chanceTotal > randomRoll)
                {
                    return drop;
                }
            }

            return null;
        }

        private float GetTotalChance(int level)
        {
            var total = 0f;
            foreach (var drop in _potentialDrops)
            {
                total += GetByLevel(drop.RelativeChance, level);
            }

            return total;
        }

        private static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0 || level <= 0)
            {
                return default;
            }

            return level > values.Length ? values[values.Length - 1] : values[level - 1];
        }
    }
}