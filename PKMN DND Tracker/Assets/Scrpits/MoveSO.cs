using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Move", order = 1)]
public class MoveSO : ScriptableObject
{
    public string moveName;
    public GameManager.Type type;
    public GameManager.MoveClass moveClass;
    public string precision;
    public string dmg;
    public int range;
    public int area;
    public int pps;
    public bool isAction;
    public bool isBonusAction;
    public bool isReaction;
    public bool hasPriority;
    public string savingThrowType;
    public string savingThrowMultiplier;
    public string duration;
    public string description;
}
