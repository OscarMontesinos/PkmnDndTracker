using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Xml;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Pkmn pkmn;

    public class EquipmentItem
    {
        public string eqName;
        public string description;

        public int modifier;
        public GameManager.Type type;
        public GameManager.MoveCategory moveClass;
        public bool makesContact;
    }

    public class ConsumableItem
    {
        public string eqName;
        public string description;

        public int uses;
        public int currentUses;
        public bool isRechargeable;
    }

    public List<EquipmentItem> equipmentInventory;
    public List<ConsumableItem> consumablesInventory;

    public void AddItem(EquipmentSO item)
    {
        if(item.eqType == GameManager.EquipmentType.Equipment)
        {
            EquipmentItem newItem = new EquipmentItem();
            newItem.eqName = item.eqName;
            newItem.description = item.description;
            newItem.modifier = item.modifier;
            newItem.type = item.type;
            newItem.moveClass = item.moveClass;
            equipmentInventory.Add(newItem);
        }
        else
        {
            ConsumableItem newItem = new ConsumableItem();
            newItem.eqName = item.eqName;
            newItem.description = item.description;
            newItem.uses = item.uses;
            newItem.currentUses = item.uses;
            newItem.isRechargeable = item.isRechargeable;
            consumablesInventory.Add(newItem);
        }
    }
    public void RemoveItem(EquipmentItem item)
    {
       equipmentInventory.Remove(item);
    }

    public void RemoveItem(ConsumableItem item)
    {
       consumablesInventory.Remove(item);
    }

    public void UseConsumable(ConsumableItem item)
    {
        item.currentUses--;
        if(item.currentUses <= 0)
        {
            if (!item.isRechargeable)
            {
                RemoveItem(item);
            }
            else
            {
                item.currentUses = 0;
            }
        }
    }

    public void RechargeConsumables()
    {
        foreach(ConsumableItem item in consumablesInventory)
        {
            if (item.isRechargeable) item.currentUses = item.uses;
        }
    }

    public int GetAttackMod(MoveSO move)
    {
        int mod = 0;
        foreach (EquipmentItem item in equipmentInventory)
        {
            if (move.type == item.type)
            {
                mod += item.modifier;
            }

            if (move.moveClass == item.moveClass)
            {
                mod += item.modifier;
            }

            if(move.makesContact && item.makesContact)
            {
                mod += item.modifier;
            }
        }
        return mod;
    }
}
