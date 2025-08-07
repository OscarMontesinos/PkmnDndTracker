using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public string chName;
    public string pkmnName;
    public int level;

    public int currentHP;
    public int currentPP;

    public Pkmn.DndBaseStats stats;

    public List<int> lvl1Moves;
    public List<int> lvl2Moves;
    public List<int> lvl3Moves;
}
