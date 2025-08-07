using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pkmn;
[CreateAssetMenu(fileName = "Pkmn", menuName = "Pkmn", order = 0)]
public class PkmnSO : ScriptableObject
{
    public Sprite pkmnSprite;
    public Pkmn.BaseStats baseStats;
    public GameManager.Type type1;
    public GameManager.Type type2;

    public List<LearnableAbilities> abilities = new List<LearnableAbilities>();

    public List<MoveSO> lvl1LearnableMoves = new List<MoveSO>();
    public List<MoveSO> lvl2LearnableMoves = new List<MoveSO>();
    public List<MoveSO> lvl3LearnableMoves = new List<MoveSO>();

    public PkmnSO megaEvo;
    public Sprite megastoneSprite;
    public bool isMega;
}
