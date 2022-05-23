using BlackCat.Inventories;
using BlackCat.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Inventories
{
    public class StatsEquipment : Equipment, IModifierProvider
    {

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;
                foreach (var modifier in item.GetPercentageModifier(stat))
                {
                    yield return modifier;
                }
            }
        }

        IEnumerable<float> IModifierProvider.GetAdditiveModifier(Stat stat)
        {
            foreach (var slot in GetAllPopulatedSlots())
            {
                var item = GetItemInSlot(slot) as IModifierProvider;
                if (item == null) continue;
                foreach (var modifier in item.GetAdditiveModifier(stat))
                {
                    yield return modifier;
                }
            }
        }
    }
}