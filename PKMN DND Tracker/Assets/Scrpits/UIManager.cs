using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static Pkmn;

public class UIManager : MonoBehaviour
{
    int currentScreen;
    public List<GameObject> screens;

    public static UIManager Instance;
    public Pkmn pkmn;
    public Image bg;
    public Image pkmnPortrait;
    public Image megastoneImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI lvlText;
    public TextMeshProUGUI xpText;
    public TMP_InputField resourceVal;
    public TextMeshProUGUI hpText;
    public Slider hpBar;
    public Image hpBarFill;
    public TextMeshProUGUI ppText;
    public Slider ppBar;
    public Slider xpBar;

    public GameObject typeShower;
    [HideInInspector]
    public GameObject type1Obj;
    [HideInInspector]
    public GameObject type2Obj;

    [HideInInspector]
    public int infoIndex;
    public List<GameObject> menusList;
    public TextMeshProUGUI infoHeaderText;

    public TextMeshProUGUI statsText;
    public TextMeshProUGUI statsModText;

    public TextMeshProUGUI dndText;
    public TextMeshProUGUI dndModsText;
    public TextMeshProUGUI dndOtherText;

    public TextMeshProUGUI dndProf1Text;
    public TextMeshProUGUI dndProf2Text;

    public GameObject abilitiesDestination;
    public GameObject abilitiesGO;

    public GameObject movesDestination;
    public GameObject moveGO;

    public GameObject itemsDestination;
    public GameObject itemGO;
    public GameObject itemCreatorGO;
    [HideInInspector]
    public int itemsMode = 1;
    public GameObject itemMenu;
    public GameObject itemCreatorMenu;
    public TMP_InputField itemNameField;
    public TMP_InputField itemQuantityField;

    public GameObject xpMenu;
    public TMP_InputField xpField;

    public GameObject lvlUpMenu;
    [HideInInspector]
    public int lvlUpIndex;
    public List<GameObject> lvlUpScreens;
    public TextMeshProUGUI statsChangesText;
    public GameObject lvl1UnlockableMoves;
    public GameObject lvl2UnlockableMoves;
    public GameObject lvl3UnlockableMoves;
    public GameObject lvl1UnlockableMovesContainer;
    public GameObject lvl2UnlockableMovesContainer;
    public GameObject lvl3UnlockableMovesContainer;
    public GameObject moveShower;
    public int moveLearnerIndex;

    public GameObject utilsMenu;

    public GameObject actionGO;
    public GameObject bonusActionGO;
    public GameObject reactionGO;

    public void SwipeScreen(int val)
    {
        currentScreen += val;
        if(currentScreen > screens.Count - 1)
        {
            currentScreen = 0;
        }
        else if(currentScreen < 0)
        {
            currentScreen = screens.Count - 1;
        }

        lvlUpMenu.SetActive(false);
        ShowInventory(true);

        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
        screens[currentScreen].SetActive(true);
    }
    
    public void SwitchScreen(int val)
    {
        currentScreen = val;
        SwipeScreen(0);
        lvlUpMenu.SetActive(false);
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if (Pkmn.Instance)
        {
            pkmn = Pkmn.Instance;
            SwitchScreen(0);
            SetPkmn();
        }
        else
        {
            SceneManager.LoadScene("Hub");
        }
    }

    public void SetPkmn()
    {
        nameText.text = pkmn.pkmnName;
        lvlText.text = pkmn.lvl.ToString();
        xpText.text = "Exp: " + pkmn.xp + " / " + pkmn.xpToReach;
        pkmnPortrait.sprite = pkmn.pkmnSprite;
        if (pkmn.basePkmn.megastoneSprite)
        {
            megastoneImage.sprite = pkmn.basePkmn.megastoneSprite;
        }
        else
        {
            megastoneImage.gameObject.SetActive(false);
        }

        if (type1Obj)
        {
            Destroy(type1Obj);
        }
        if (type2Obj)
        {
            Destroy(type2Obj);
        }

        foreach (GameManager.TypeVisuals visual in GameManager.Instance.typesVisuals)
        {
            if (pkmn.type1 == visual.type)
            {
                bg.color = visual.color;
                type1Obj = Instantiate(visual.image, typeShower.transform);
            }
        }
        if (pkmn.type2 != GameManager.Type.none)
        {
            foreach (GameManager.TypeVisuals visual in GameManager.Instance.typesVisuals)
            {
                if (pkmn.type2 == visual.type)
                {
                    type2Obj = Instantiate(visual.image, typeShower.transform);
                }
            }
        }

        int i = 0;
        while (i < abilitiesDestination.transform.childCount)
        {
            Destroy(abilitiesDestination.transform.GetChild(i).gameObject);
            i++;
        }

        i = 0;
        while (i < movesDestination.transform.childCount)
        {
            Destroy(movesDestination.transform.GetChild(i).gameObject);
            i++;
        }

        foreach (Pkmn.LearnableAbilities ability in pkmn.basePkmn.abilities)
        {
            if (pkmn.lvl >= ability.lvlRequired)
            {
                Instantiate(abilitiesGO, abilitiesDestination.transform).GetComponent<AbilityShower>().ActivateAbility(ability.ability);
            }
            else
            {
                Instantiate(abilitiesGO, abilitiesDestination.transform).GetComponent<AbilityShower>().LockAbility();
            }
        }

        foreach(Transform move in movesDestination.transform)
        {
            Destroy(move.gameObject);
        }

        foreach (int move in pkmn.lvl1Moves)
        {
            Instantiate(moveGO, movesDestination.transform).GetComponent<MovShower>().SetMove(pkmn.basePkmn.lvl1LearnableMoves[move], pkmn);
        }

        foreach (int move in pkmn.lvl2Moves)
        {
            Instantiate(moveGO, movesDestination.transform).GetComponent<MovShower>().SetMove(pkmn.basePkmn.lvl2LearnableMoves[move], pkmn);
        }

        foreach (int move in pkmn.lvl3Moves)
        {
            Instantiate(moveGO, movesDestination.transform).GetComponent<MovShower>().SetMove(pkmn.basePkmn.lvl3LearnableMoves[move], pkmn);
        }

        foreach (Transform item in itemsDestination.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (Item item in pkmn.inventory)
        {
            CreateItemShower(item);
        }

        UpdateHp();
        UpdatePp();
        UpdateXp();
        UpdateStats();
        UpdateDND();
        UpdateInfo(0);
        UpdateDNDProf();
    }
    private void Update()
    {
        if (pkmn)
        {
            UpdateActionTokens();

            if (pkmn.extraStats.isMega)
            {
                bg.color = MegaBGColorHandler.Instance.megaBGColor;
            }
        }
    }

    public void ChangePkmnHP(int val)
    {
        if (resourceVal.text != "")
        {
            Pkmn.Instance.ChangeHP(int.Parse(resourceVal.text) * val);
        }
        resourceVal.text = "1";
    }

    public void ChangePkmnPP(int val)
    {
        if (resourceVal.text != "")
        {
            Pkmn.Instance.ChangePP(int.Parse(resourceVal.text) * val);
        }
        resourceVal.text = "1";
    }

    public void GainXP(int val)
    {
        if (xpField.text != "")
        {
            SwitchScreen(0);
            ShowXPMenu(false);
            Pkmn.Instance.GainXP(int.Parse(xpField.text));
            xpField.text = "";
        }
    }
    public void ResetActionTokens()
    {
        Pkmn.Instance.ResetActionTokens();
    }

    public void UseAction()
    {
        Pkmn.Instance.UseAction();
    }
    public void UseBonusAction()
    {
        Pkmn.Instance.UseBonusAction();
    }
    public void UseReaction()
    {
        Pkmn.Instance.UseReaction();
    }

    public void UpdateHp()
    {
        float hp = pkmn.stats.hp;
        float mHp = pkmn.stats.mHp;
        hpText.text = "HP: " + pkmn.stats.hp.ToString() + " / " + pkmn.stats.mHp.ToString() + " | " + ((hp/mHp)*100).ToString("F0") + "%";
        hpBar.maxValue = pkmn.stats.mHp;   
        hpBar.value = pkmn.stats.hp;
        if(pkmn.stats.hp >= pkmn.stats.mHp / 2)
        {
            hpBarFill.color = GameManager.Instance.hpColors[0];
        }
        else if(pkmn.stats.hp >= pkmn.stats.mHp / 4)
        {
            hpBarFill.color = GameManager.Instance.hpColors[1];
        }
        else
        {
            hpBarFill.color = GameManager.Instance.hpColors[2];
        }
    }

    public void UpdatePp()
    {
        ppText.text = "PP: " + pkmn.extraStats.pp.ToString() + " / " + pkmn.extraStats.mPp.ToString();
        ppBar.maxValue = pkmn.extraStats.mPp;
        ppBar.value = pkmn.extraStats.pp;
    }

    public void UpdateXp()
    {
        xpBar.maxValue = pkmn.xpToReach;
        xpBar.value = pkmn.xp;
        lvlText.text = pkmn.lvl.ToString();
        xpText.text = "Exp: " + pkmn.xp + " / " + pkmn.xpToReach;
    }

    public void UpdateStats()
    {
        statsText.text = pkmn.stats.hp + "\n" + pkmn.stats.atk + "\n" + pkmn.stats.def + "\n" + pkmn.stats.sAtk + "\n" + pkmn.stats.sDef + "\n" +pkmn.stats.spd;
        statsModText.text = GetStatModText(pkmn.statsMod.atk) + "\n" + GetStatModText(pkmn.statsMod.sAtk) + "\n" + GetStatModText(pkmn.statsMod.spd);
    }

    public void UpdateDND()
    {
        dndText.text = pkmn.dndStats.con + "\n" + pkmn.dndStats.str + "\n" + pkmn.dndStats.cha + "\n" + pkmn.dndStats.intel + "\n" + pkmn.dndStats.wis + "\n" +
            pkmn.dndStats.dex;
        dndModsText.text = GetStatModText(pkmn.dndMod.con) + "\n" + GetStatModText(pkmn.dndMod.str) + "\n" + GetStatModText(pkmn.dndMod.cha) + "\n" +
            GetStatModText(pkmn.dndMod.intel) + "\n" + GetStatModText(pkmn.dndMod.wis) + "\n" + GetStatModText(pkmn.dndMod.dex);
        dndOtherText.text = pkmn.extraStats.proficiencyBonus + "\n" + pkmn.extraStats.baseDC + "\n" + pkmn.extraStats.hitDiceNumber + "d" + pkmn.extraStats.hitDice;
    }

    public string GetStatModText(int val)
    {
        if (val <= 0)
        {
            return "" + val;
        }
        else
        {
            return "+" + val;
        }
    }
    public void UpdateDNDProf()
    {
        dndProf1Text.text = pkmn.dndMod.dex + 
            "\n" + pkmn.dndMod.wis +
            "\n" + pkmn.dndMod.intel +
            "\n" + pkmn.dndMod.str +
            "\n" + pkmn.dndMod.cha +
            "\n"+ pkmn.dndMod.cha +
            "\n" + pkmn.dndMod.intel +
            "\n" + pkmn.dndMod.wis +
            "\n" + pkmn.dndMod.intel;

        dndProf2Text.text = pkmn.dndMod.wis +
            "\n"+ pkmn.dndMod.intel +
            "\n" + pkmn.dndMod.wis +
            "\n" + pkmn.dndMod.cha +
            "\n" + pkmn.dndMod.cha +
            "\n" + pkmn.dndMod.intel +
            "\n" + pkmn.dndMod.dex +
            "\n" + pkmn.dndMod.dex +
            "\n" + pkmn.dndMod.wis;
            
    }

    public void UpdateActionTokens()
    {
        actionGO.SetActive(pkmn.action);
        bonusActionGO.SetActive(pkmn.bonusAction);
        reactionGO.SetActive(pkmn.reaction);
    }

    public void UpdateInfo(int val)
    {
        infoIndex += val;
        if(infoIndex > menusList.Count-1)
        {
            infoIndex = 0;
        }
        else if(infoIndex < 0)
        {
            infoIndex = menusList.Count-1;
        }
        foreach(GameObject menu in menusList)
        {
            menu.SetActive(false);
        }
        menusList[infoIndex].SetActive(true);
        switch (infoIndex)
        {
            case 0:
                infoHeaderText.text = "ESTADÍSTICAS";
                break;
            case 1:
                infoHeaderText.text = "DND";
                break;
            case 2:
                infoHeaderText.text = "CAPACIDADES";
                break;
            case 3:
                infoHeaderText.text = "HABILIDADES";
                break;
            case 4:
                infoHeaderText.text = "MOVIMIENTOS";
                break;
        }
    }
    public void MegaEvolve()
    {
        pkmn.MegaEvolve();
    }

    public void Heal(float value)
    {
        pkmn.ChangeHP((int)(pkmn.stats.mHp * value));
        pkmn.ChangePP((int)(pkmn.extraStats.mPp * value));
        SwitchScreen(0);
    }

    public void ChangeItemMode(int val)
    {
        itemsMode = val;
    }

    public void ShowInventory(bool val)
    {
        itemMenu.SetActive(val);
        itemCreatorMenu.SetActive(!val);
    }

    public void ShowXPMenu(bool val)
    {
        utilsMenu.SetActive(!val);
        xpMenu.SetActive(val);
    }

    public void AddItem()
    {
        if (itemQuantityField.text != "" && itemNameField.text != "")
        {

            bool repeted = false;
            foreach (Pkmn.Item pkmnItem in pkmn.inventory)
            {
                if(pkmnItem.itemName == itemNameField.text)
                {
                    repeted = true;
                    break;
                }
            }

            if (!repeted)
            {
                Pkmn.Item item = new Pkmn.Item();
                item.itemName = itemNameField.text;
                item.quantity = int.Parse(itemQuantityField.text);
                pkmn.inventory.Add(item);
                CreateItemShower(item.itemName, item.quantity);
                CharacterManager.Instance.SafeInfo(pkmn);
            }
        }
    }

    public void CreateItemShower(Pkmn.Item item)
    {
        CreateItemShower(item.itemName, item.quantity);
    }

    public void CreateItemShower(string name, int quantity)
    {
        ShowInventory(true);
        Destroy(itemsDestination.transform.GetChild(itemsDestination.transform.childCount - 1).gameObject);
        Instantiate(itemGO, itemsDestination.transform).GetComponent<ItemShower>().SetUpValues(name, quantity, itemsDestination.transform.childCount);
        Instantiate(itemCreatorGO, itemsDestination.transform);
        itemNameField.text = "";
        itemQuantityField.text = "";
    }

    public void StartLvlUpSequence()
    {
        SetStatChangesText();
        SwitchScreen(0);
        screens[0].SetActive(false);
        lvlUpMenu.SetActive(true);
        pkmn.lvl++;
        pkmn.ResetPkmn();
        CreateLVLUPMoves();
        LvlUpNext(0);
    }

    public void LvlUpNext(int val)
    {
        if (val > lvlUpScreens.Count-1)
        {
            SwitchScreen(0);

            pkmn.ResetPkmn();
            SetPkmn();

            CharacterManager.Instance.SafeInfo(pkmn);
        }
        else
        {
            foreach (GameObject screen in lvlUpScreens)
            {
                screen.SetActive(false);
            }
            lvlUpScreens[val].SetActive(true);


            lvl1UnlockableMoves.SetActive(false);
            lvl2UnlockableMoves.SetActive(false);
            lvl3UnlockableMoves.SetActive(false);

            if (val == 1)
            {
                if (pkmn.CalculateLvl1MoveSlots(pkmn.lvl) > pkmn.lvl1Moves.Count && pkmn.basePkmn.lvl1LearnableMoves.Count >= pkmn.CalculateLvl1MoveSlots(pkmn.lvl))
                {
                    lvl1UnlockableMoves.SetActive(true);
                }
                else if (pkmn.CalculateLvl2MoveSlots(pkmn.lvl) > pkmn.lvl2Moves.Count && pkmn.basePkmn.lvl2LearnableMoves.Count >= pkmn.CalculateLvl2MoveSlots(pkmn.lvl))
                {
                    lvl2UnlockableMoves.SetActive(true);
                }
                else if (pkmn.CalculateLvl3MoveSlots(pkmn.lvl) > pkmn.lvl3Moves.Count && pkmn.basePkmn.lvl3LearnableMoves.Count >= pkmn.CalculateLvl3MoveSlots(pkmn.lvl))
                {
                    lvl3UnlockableMoves.SetActive(true);
                }
                else
                {
                    SwitchScreen(0);
                }
            }
        }
    }

    public void SetStatChangesText()
    {
        statsChangesText.text = pkmn.lvl + " > " + (pkmn.lvl +1) + "\n\n" +
            pkmn.stats.mHp + " > " + pkmn.CalculateStatHP(pkmn.lvl + 1) + "\n" +
            pkmn.extraStats.mPp + " > " + pkmn.CalculatePP(pkmn.lvl + 1) + "\n" +
            pkmn.stats.atk + " > " + pkmn.CalculateStatAtk(pkmn.lvl + 1) + "\n" +
            pkmn.stats.def + " > " + pkmn.CalculateStatDef(pkmn.lvl + 1) + "\n" +
            pkmn.stats.sAtk + " > " + pkmn.CalculateStatSAtk(pkmn.lvl + 1) + "\n" +
            pkmn.stats.sDef + " > " + pkmn.CalculateStatSDef(pkmn.lvl + 1) + "\n" +
            pkmn.stats.spd + " > " + pkmn.CalculateStatSpd(pkmn.lvl + 1) + "\n\n" +

            GetStatModText(pkmn.extraStats.proficiencyBonus) + " > " + GetStatModText(pkmn.CalculateProfBonus(pkmn.lvl + 1)) + "\n" +
            pkmn.extraStats.baseDC + " > " + pkmn.CalculateDC(pkmn.lvl + 1) + "\n" +
            pkmn.extraStats.hitDice + " > " + pkmn.CalculateHitDice(pkmn.lvl + 1) + "\n\n" +

            pkmn.lvl1Moves.Count + " > " + pkmn.CalculateLvl1MoveSlots(pkmn.lvl + 1) + "\n" +
            pkmn.lvl2Moves.Count + " > " + pkmn.CalculateLvl2MoveSlots(pkmn.lvl + 1) + "\n" +
            pkmn.lvl3Moves.Count + " > " + pkmn.CalculateLvl3MoveSlots(pkmn.lvl + 1) + "\n";
    }

    public void CreateLVLUPMoves()
    {
        moveLearnerIndex = 0;
        foreach(Transform move in lvl1UnlockableMovesContainer.transform)
        {
            Destroy(move.gameObject);
        }
        foreach(Transform move in lvl2UnlockableMovesContainer.transform)
        {
            Destroy(move.gameObject);
        }
        foreach(Transform move in lvl3UnlockableMovesContainer.transform)
        {
            Destroy(move.gameObject);
        }

        foreach (MoveSO move in pkmn.basePkmn.lvl1LearnableMoves)
        {
            if (!pkmn.lvl1Moves.Contains(pkmn.basePkmn.lvl1LearnableMoves.IndexOf(move)))
            {
                Instantiate(moveShower, lvl1UnlockableMovesContainer.transform).GetComponent<UnlockableMoveShower>().SetMove(UnlockableMoveShower.Mode.leveling,move, pkmn, 1);
            }
            else
            {
                Instantiate(new GameObject(), lvl1UnlockableMovesContainer.transform);
            }
        }
        foreach (MoveSO move in pkmn.basePkmn.lvl2LearnableMoves)
        {
            if (!pkmn.lvl2Moves.Contains(pkmn.basePkmn.lvl2LearnableMoves.IndexOf(move)))
            {
                Instantiate(moveShower, lvl2UnlockableMovesContainer.transform).GetComponent<UnlockableMoveShower>().SetMove(UnlockableMoveShower.Mode.leveling, move, pkmn, 2);
            }
            else
            {
                Instantiate(new GameObject(), lvl2UnlockableMovesContainer.transform);
            }
        }
        foreach (MoveSO move in pkmn.basePkmn.lvl3LearnableMoves)
        {
            if (!pkmn.lvl3Moves.Contains(pkmn.basePkmn.lvl3LearnableMoves.IndexOf(move)))
            {
                Instantiate(moveShower, lvl3UnlockableMovesContainer.transform).GetComponent<UnlockableMoveShower>().SetMove(UnlockableMoveShower.Mode.leveling, move, pkmn, 3);
            }
            else
            {
                Instantiate(new GameObject(), lvl3UnlockableMovesContainer.transform);
            }
        }
    }

    public void ReturnHub()
    {
        SceneManager.LoadScene("Hub");
    }
}
