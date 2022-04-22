using System.Collections.Generic;

namespace BlackCat.Stats
{
    public interface IModifierProvider
    {
        // Get First VALUE , then , PERCENTAGE , em macacês, ele pega primeiro o VALOR BONUS, e em segundo a Porcentagem BONUS
        IEnumerable<(float valueBonus,float percentageBonus)> GetAdditiveModifier(Stat stat);

    }
}
