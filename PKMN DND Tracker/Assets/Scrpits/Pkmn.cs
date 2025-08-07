using System;
using System.Collections;
using System.Collections.Generic;
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

        SetPkmn();

        CalculateModifiers();
        CalculateStats();
        CalculateExtraStats();
        CalculateStatsModifiers();
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
    void ResetPkmn()
    {
        SetPkmn();

        if (UIManager.Instance)
        {
            UIManager.Instance.SetPkmn();
        }
    }

    public void CalculateModifiers()
    {
        if (type1 == Type.Grass || (type2 == Type.Grass && lvl >= 10))
        {
            dndStats.wis += 2;
        }

        dndMod.con = (dndStats.con - 10) / 2;
        dndMod.str = (dndStats.str - 10) / 2;
        dndMod.cha = (dndStats.cha - 10) / 2;
        dndMod.intel = (dndStats.intel - 10) / 2;
        dndMod.wis = (dndStats.wis - 10) / 2;
        dndMod.dex = (dndStats.dex - 10) / 2;
    }
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

        stats.mHp = (int)((baseStats.hp / 4) + ((baseStats.hp / 17.5f) * (lvl - 1)) + dndMod.con * 2);
        stats.hp = (int)(stats.mHp * hpPercentage) / 100;
        stats.atk = (baseStats.atk / 5) + ((baseStats.atk / 200) * (lvl - 1)) + dndMod.str;
        stats.def = (int)(baseStats.def / 5.5f) + ((baseStats.def / 400) * (lvl - 1)) + dndMod.con;
        if(dndMod.intel > dndMod.cha)
        {
            stats.sAtk = (baseStats.sAtk / 5) + ((baseStats.sAtk / 200) * (lvl - 1)) + dndMod.intel;
        }
        else
        {
            stats.sAtk = (baseStats.sAtk / 5) + ((baseStats.sAtk / 200) * (lvl - 1)) + dndMod.cha;
        }
        stats.sDef = (int)(baseStats.sDef / 5.5f) + ((baseStats.sDef / 400) * (lvl - 1)) + dndMod.wis;
        stats.spd = (baseStats.spd / 5) + ((baseStats.spd / 200) * (lvl - 1)) + dndMod.dex; 
    }
    public void CalculateStatsModifiers()
    {
        statsMod.atk = (stats.atk-10)/2;
        statsMod.sAtk = (stats.sAtk-10)/2;
        statsMod.spd = (stats.spd-10)/2;
    }
    public void CalculateExtraStats()
    {
        extraStats.proficiencyBonus = 2 +(lvl / 4);
        extraStats.baseDC = 8 + extraStats.proficiencyBonus;
        extraStats.pp = 45 + ((lvl - 1) * 4);
        extraStats.mPp = extraStats.pp;
        switch (lvl)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                extraStats.hitDice = 4;
                break;
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                extraStats.hitDice = 6;
                break;
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
                extraStats.hitDice = 8;
                break;
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
                extraStats.hitDice = 10;
                break;
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
                extraStats.hitDice = 12;
                break;
            default:
                extraStats.hitDice = 20;
                break;
        }
        if(type1 == Type.Fighting || (type2 == Type.Fighting && lvl >= 10))
        {
            extraStats.hitDiceNumber = 1 + (lvl/4);
        }
        else
        {
            extraStats.hitDiceNumber = 1;
        }
        extraStats.lvl1MoveSlots = lvl/2;
        extraStats.lvl2MoveSlots = lvl/6;
        extraStats.lvl3MoveSlots = lvl/40;
    }

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
