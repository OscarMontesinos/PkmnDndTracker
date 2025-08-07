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

    public int lvl1MoveCount;
    public List<int> lvl1Moves = new List<int>();
    public int lvl2MoveCount;
    public List<int> lvl2Moves = new List<int>();
    public int lvl3MoveCount;
    public List<int> lvl3Moves = new List<int>();
}
