using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "RPG/Inventory/Equipable Item", fileName = "StatsEquipableItem")]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [Serializable]
        public struct Modifier
        {
            public Stat Stat;
            public float Value;
        }
        
        [SerializeField] private Modifier[] _additiveModifiers;
        [SerializeField] private Modifier[] _percentageModifiers;
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in _additiveModifiers)
            {
                if (modifier.Stat != stat)
                {
                    continue;
                }

                yield return modifier.Value;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in _percentageModifiers)
            {
                if (modifier.Stat != stat)
                {
                    continue;
                }

                yield return modifier.Value;
            }
        }
    }
}