using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Move", order = 1)]
[Serializable]
public class MoveSO : ScriptableObject
{
    [Header("Info")]
    public string moveName;
    public string description;
    public string extraEffect;
    public int pps;

    [Header("Typing and class")]
    public GameManager.Type type;
    public GameManager.MoveCategory moveClass;
    public bool makesContact;

    [Header("Precision & Saving Throw")]
    public bool precision;
    public string savingThrowType;
    public GameManager.DndStatsType scallingSavingThrowType;
    public GameManager.DndStatsType secondaryScallingSavingThrowType;
    public GameManager.MoveCategory combatScallingStat;
    public float savingThrowMultiplier = 1;

    [Header("Effects")]
    public bool useHitDice;
    public int dmgDices;
    public int dmgDiceType;
    public int dmgBonus;
    public GameManager.DndStatsType dmgStatBonus;
    public string duration;

    [Header("Area and Range")]
    public int range;
    public int area;

    [Header("Action Type")]
    public bool isAction;
    public bool isBonusAction;
    public bool isReaction;
    public bool hasPriority;
}
