using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment", order = 4)]
public class EquipmentSO : ScriptableObject
{
    [Header("Info")]
    public string eqName;
    public string description;
    public GameManager.EquipmentType eqType;

    [Header("Equipable")]
    public GameManager.Type type;
    public GameManager.MoveCategory moveClass;
    public bool contactMoves;
    public int modifier;

    [Header("Consumable")]
    public int uses;
    public bool isRechargeable;
}
