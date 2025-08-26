using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;
using static UnityEngine.GraphicsBuffer;
using Type = GameManager.Type;

public class Pkmn : MonoBehaviour
{
    public static Pkmn Instance;

    public PkmnSO basePkmn;
    public string pkmnName;

    [HideInInspector]
    public Sprite pkmnSprite;
    public int lvl;
    public int xp;
    [HideInInspector]
    public int xpToReach = 40;
    [HideInInspector]
    public Type type1;
    [HideInInspector]
    public Type type2;
    
    [Serializable]
    public struct BaseStats
    {
        [HideInInspector]
        public int mHp;
        public int hp;
        public int atk;
        public int def;
        public int sAtk;
        public int sDef;
        public int spd;
    }
    [Serializable]
    public struct DndBaseStats
    {
        public int con;
        public int str;
        public int cha;
        public int intel;
        public int wis;
        public int dex;
    }
    [Serializable]
    public struct ExtraStats
    {
        public int proficiencyBonus;
        public int baseDC;
        [HideInInspector]
        public int mPp;
        public int pp;
        public int hitDiceNumber;
        public int hitDice;
        public int lvl1MoveSlots;
        public int lvl2MoveSlots;
        public int lvl3MoveSlots;
        public bool isMega;
    }
    [Serializable]
    public struct Proficiencies
    {
        bool acrobatics;
        bool animalHandling;
        bool arcana;
        bool athletics;
        bool deception;
        bool history;
        bool insight;
        bool intimidation;
        bool investigation;
        bool medicine;
        bool nature;
        bool perception;
        bool performance;
        bool persuasion;
        bool religion;
        bool sleightOfHand;
        bool stealth;
        bool survival;
    }

    [HideInInspector]
    public BaseStats baseStats;
    public DndBaseStats dndStats;
    [HideInInspector]
    public DndBaseStats dndMod;
    [HideInInspector]
    public ExtraStats extraStats;
    [HideInInspector]
    public BaseStats stats;
    [HideInInspector]
    public BaseStats statsMod;

    [Serializable]
    public struct LearnableAbilities
    {
        public AbilitySO ability;
        public int lvlRequired;
    }

    public List<int> lvl1Moves;
    public List<int> lvl2Moves;
    public List<int> lvl3Moves;

    [Serializable]
    public struct Item
    {
        public string itemName;
        public int quantity;
    }
    public List<Item> inventory;

    [HideInInspector]
    public bool action = true;
    [HideInInspector]
    public bool bonusAction = true;
    [HideInInspector]
    public bool reaction = true;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    public void InitializePkmn(CreationHandler creation, string name)
    {
        basePkmn = creation.pkmn;
        pkmnName = name;
        lvl = creation.lvl;
        dndStats = creation.dndStats;
        lvl1Moves = creation.lvl1Moves;
        lvl2Moves = creation.lvl2Moves;
        lvl3Moves = creation.lvl3Moves;

        SortMoves(lvl1Moves);
        SortMoves(lvl2Moves);
        SortMoves(lvl3Moves);

        SetPkmn();

        CalculateModifiers();
        CalculateStats();
        CalculateExtraStats();
        CalculateStatsModifiers();
    }

    public void InitializePkmn(CharacterData character, PkmnSO pkmn, string name)
    {
        basePkmn = pkmn;
        pkmnName = name;
        lvl = character.level;
        dndStats = character.stats;
        lvl1Moves = character.lvl1Moves;
        lvl2Moves = character.lvl2Moves;
        lvl3Moves = character.lvl3Moves;

        SortMoves(lvl1Moves);
        SortMoves(lvl2Moves);
        SortMoves(lvl3Moves);

        int i = 0;
        while(i < character.itemsName.Count)
        {
            Item item = new Item();
            item.itemName = character.itemsName[i];
            item.quantity = character.itemsQuantity[i];

            inventory.Add(item);
            i++;
        }

        SetPkmn();

        CalculateModifiers();
        CalculateStats();
        CalculateExtraStats();
        CalculateStatsModifiers();

        stats.hp = character.currentHP;
        extraStats.pp = character.currentPP;
        xp = character.currentXP;
    }

    public void SortMoves(List<int> moves)
    {
        int i1 = 0;
        int i2 = 0;

        while(i1 <= moves.Count - 1)
        {
            while (i2 < moves.Count - 1)
            {
                if (moves[i2] > moves[i2 + 1])
                {
                    int save = moves[i2];
                    moves[i2] = moves[i2+1];
                    moves[i2+1] = save;
                }
                i2++;
            }
            i2 = 0;
            i1++;
        }
    }

    public virtual void SetPkmn()
    {
        pkmnSprite = basePkmn.pkmnSprite;
        baseStats = basePkmn.baseStats;
        type1 = basePkmn.type1;
        type2 = basePkmn.type2;
    }
    public void ResetPkmn()
    {
        SortMoves(lvl1Moves);
        SortMoves(lvl2Moves);
        SortMoves(lvl3Moves);

        SetPkmn();

        CalculateModifiers();
        CalculateStats();
        CalculateExtraStats();
        CalculateStatsModifiers();

        if (UIManager.Instance)
        {
            UIManager.Instance.SetPkmn();
        }
    }

    public void CalculateModifiers()
    {
        if (type1 == Type.Grass)
        {
            dndStats.wis += 2;

            if (type2 == Type.none && lvl >= 10)
            {
                dndStats.wis += 2;
            }
        }

        dndMod.con = (dndStats.con - 10) / 2;
        dndMod.str = (dndStats.str - 10) / 2;
        dndMod.cha = (dndStats.cha - 10) / 2;
        dndMod.intel = (dndStats.intel - 10) / 2;
        dndMod.wis = (dndStats.wis - 10) / 2;
        dndMod.dex = (dndStats.dex - 10) / 2;
    }

    #region PkmnStats
    public void CalculateStats()
    {
        if(stats.mHp == 0)
        {
            stats.mHp = 1;
            stats.hp = 1;
        }

        float hp = stats.hp;
        float mHp = stats.mHp;
        float hpPercentage = (hp / mHp) * 100;

        stats.mHp = CalculateStatHP(lvl);
        stats.hp = (int)(stats.mHp * hpPercentage) / 100;
        stats.atk = CalculateStatAtk(lvl);
        stats.def = CalculateStatDef(lvl);
        stats.sAtk = CalculateStatSAtk(lvl);
        stats.sDef = CalculateStatSDef(lvl);
        stats.spd = CalculateStatSpd(lvl);
    }

    public int CalculateStatHP(int lvl)
    {
        return (int)((baseStats.hp / 4) + ((baseStats.hp / 17.5f) * (lvl - 1)) + dndMod.con * 2);
    }

    public int CalculateStatAtk(int lvl)
    {
        return (int)((baseStats.atk / 5) + ((baseStats.atk *0.005f) * (lvl - 1)) + dndMod.str);
    }

    public int CalculateStatDef(int lvl)
    {
        return (int)((baseStats.def / 5.5f) + ((baseStats.def * 0.025f) * (lvl - 1)) + dndMod.con);
    }

    public int CalculateStatSAtk(int lvl)
    {
        if (dndMod.intel > dndMod.cha)
        {
            return (int)((baseStats.sAtk / 5) + ((baseStats.sAtk * 0.005f) * (lvl - 1)) + dndMod.intel);
        }
        else
        {
            return (int)((baseStats.sAtk / 5) + ((baseStats.sAtk * 0.005f) * (lvl - 1)) + dndMod.cha);
        }
    }

    public int CalculateStatSDef(int lvl)
    {
        return (int)((baseStats.sDef / 5.5f) + ((baseStats.sDef *0.0025) * (lvl - 1)) + dndMod.wis);
    }

    public int CalculateStatSpd(int lvl)
    {
        return (int)((baseStats.spd / 5) + ((baseStats.spd * 0.005f) * (lvl - 1)) + dndMod.dex);
    }

    public void CalculateStatsModifiers()
    {
        statsMod.atk = (stats.atk-10)/2;
        statsMod.sAtk = (stats.sAtk-10)/2;
        statsMod.spd = (stats.spd-10)/2;
    }
    public void CalculateExtraStats()
    {
        extraStats.proficiencyBonus = CalculateProfBonus(lvl);
        extraStats.baseDC = CalculateDC(lvl);
        extraStats.pp = CalculatePP(lvl);
        extraStats.mPp = extraStats.pp;

        extraStats.hitDice =CalculateHitDice(lvl);

        if (type1 == Type.Fighting || (type2 == Type.Fighting && lvl >= 10))
        {
            extraStats.hitDiceNumber = 1 + (lvl / 4);
        }
        else
        {
            extraStats.hitDiceNumber = 1;
        }

        extraStats.lvl1MoveSlots = CalculateLvl1MoveSlots(lvl);
        extraStats.lvl2MoveSlots = CalculateLvl2MoveSlots(lvl);
        extraStats.lvl3MoveSlots = CalculateLvl3MoveSlots(lvl);
    }

    public int CalculateProfBonus(int lvl)
    {
        return 2 + (lvl / 4);
    }

    public int CalculateDC(int lvl)
    {
        return 8 + extraStats.proficiencyBonus;
    }

    public int CalculatePP(int lvl)
    {
        return 45 + ((lvl - 1) * 4);
    }


    public int CalculateHitDice(int lvl)
    {
        switch (lvl)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                return 4;
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
               return 6;
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
                return 8;
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
                return 10;
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
                return 12;
            default:
                return 20;
        }
    }

    public int CalculateLvl1MoveSlots(int lvl)
    {
        int calc = (int)(lvl * 0.5f);
        return calc;
    }

    public int CalculateLvl2MoveSlots(int lvl)
    {
        int calc = (int)((float)(lvl / 3)) - 2;
        if (calc < 0) calc = 0;
        return calc;
    }

    public int CalculateLvl3MoveSlots(int lvl)
    {
        int calc = (int)(lvl * 0.25f) - 2;
        if (calc < 0) calc = 0;
        return calc;
    }
    #endregion

    public void ChangeHP(int val)
    {
        stats.hp += val;
        if(stats.hp > stats.mHp)
        {
            stats.hp = stats.mHp;
        }
        else if(stats.hp < 0)
        {
            stats.hp = 0;
        }
        UIManager.Instance.UpdateHp();
        CharacterManager.Instance.SafeInfo(this);
    }

    public void ChangePP(int val)
    {
        extraStats.pp += val;
        if(extraStats.pp > extraStats.mPp)
        {
            extraStats.pp = extraStats.mPp;
        }
        else if(extraStats.pp < 0)
        {
            extraStats.pp = 0;
        }
        UIManager.Instance.UpdatePp();
        CharacterManager.Instance.SafeInfo(this);
    }

    #region LEVELUP

    public void GainXP(int val)
    {
        xp += val;
        if (xp >= xpToReach)
        {
            LevelUp();
        }
        UIManager.Instance.UpdateXp();
        CharacterManager.Instance.SafeInfo(this);

    }

    public void LevelUp()
    {
        xp -= xpToReach;
        UIManager.Instance.StartLvlUpSequence();
    }

    #endregion

    public void ResetActionTokens()
    {
        action = true;
        bonusAction = true;
        reaction = true;
    }

    public void UseAction()
    {
        action = false;
    }
    public void UseBonusAction()
    {
        bonusAction = false;
    }
    public void UseReaction()
    {
        reaction = false;
    }

    public void MegaEvolve()
    {
        basePkmn = basePkmn.megaEvo;
        baseStats = basePkmn.baseStats;
        extraStats.isMega = basePkmn.isMega;

        CalculateStats();
        CalculateStatsModifiers();
        ResetPkmn();
    }
}
