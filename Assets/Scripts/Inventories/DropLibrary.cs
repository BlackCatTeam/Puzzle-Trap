using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Inventories
{
    [CreateAssetMenu(menuName = "Black Cat/InventorySystem/Drop Library")]
    public class DropLibrary : ScriptableObject
    {
        [SerializeField] DropConfig[] potentitalDrops;
        [SerializeField] float[] dropChancePercentage;
        [SerializeField] int[] minDrop;
        [SerializeField] int[] maxDrop;
        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }
        [System.Serializable]
        class DropConfig
        {
            public InventoryItem item;
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;
            public int GetRandomNumber(int level)
            {
                if (!item.IsStackable())
                {
                    return 1;

                }
                int min = GetByLevel(minNumber, level);
                int max = GetByLevel(maxNumber, level);
                return UnityEngine.Random.Range(min, max+1); 
            }

        }



        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }
            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        private bool ShouldRandomDrop(int level) => UnityEngine.Random.Range(0,100) < GetByLevel(dropChancePercentage, level);
        

        private int GetRandomNumberOfDrops(int level)
        {
            int min = GetByLevel(minDrop, level);
            int max = GetByLevel(maxDrop, level);
            return UnityEngine.Random.Range(min, max);
        }
        private Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);
            return result;
        }
        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = UnityEngine.Random.Range(0, totalChance);
            float chanceTotal = 0;
            foreach(var drop in potentitalDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChance,level);
                if (chanceTotal > randomRoll)
                    return drop;
            }
            return null;
        }

        private float GetTotalChance(int level)
        {
            float total = 0;
            foreach (var drop in potentitalDrops)
                total +=GetByLevel(drop.relativeChance,level);
            return total;
        }

        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
                return default;

            if (level > values.Length)
                return values[values.Length - 1];

            if (level <= 0)
                return default;

            return values[level - 1];
        }
    }
}

