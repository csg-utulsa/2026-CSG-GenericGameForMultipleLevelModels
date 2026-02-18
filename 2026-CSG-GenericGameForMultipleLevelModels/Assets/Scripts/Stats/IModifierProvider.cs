using System.Collections.Generic;
namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        //IEnumerable allows foreach loops to be used
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}