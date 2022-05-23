using BlackCat.Inventories;
using BlackCat.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Inventories
{

    [CreateAssetMenu(menuName = ("Black Cat/InventorySystem/Equipable Item  With Stats"))]
    public class StatsEquipableItem : EquipableItem , IModifierProvider
    {
        [SerializeField]
        Modifier[] additiveModifiers;
        [SerializeField]
        Modifier[] percentageModifiers;

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            foreach (Modifier modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                    yield return modifier.value;
            }
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach (Modifier modifier in additiveModifiers)
            {
                if (modifier.stat == stat)
                    yield return modifier.value;
            }
        }
        [Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }
    }
}
