using UnityEngine;

public class PkmnOnCreation : Pkmn
{
    public override void SetPkmn()
    {
        base.SetPkmn();

        CalculateModifiers();
        CalculateStats();
        CalculateExtraStats();
        CalculateStatsModifiers();
    }


}
