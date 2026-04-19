using UnityEngine;

public class PkmnOnCreation : Pkmn
{
    public override void SetPkmn()
    {
        base.SetPkmn();

        CalculateDndStats();
        CalculateModifiers();
        CalculateStats();
        CalculateExtraStats();
        CalculateStatsModifiers();
    }


}
