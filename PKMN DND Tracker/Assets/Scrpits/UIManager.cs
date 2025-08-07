using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TextMeshProUGUI hpText;
    public Slider hpBar;
    public Image hpBarFill;
    public TextMeshProUGUI ppText;
    public Slider ppBar;

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
    public TextMeshProUGUI dndOtherText;

    public TextMeshProUGUI dndProf1Text;
    public TextMeshProUGUI dndProf2Text;

    public GameObject abilitiesDestination;
    public GameObject abilitiesGO;

    public GameObject movesDestination;
    public GameObject moveGO;

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
        SwitchScreen(0);
        SetPkmn();

    }

    public void SetPkmn()
    {
        nameText.text = pkmn.pkmnName;
        lvlText.text = pkmn.lvl.ToString();
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
            Destroy (type2Obj);
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

        foreach (Pkmn.LearnableAbilities ability in pkmn.basePkmn.abilities)
        {
            if(pkmn.lvl >= ability.lvlRequired)
            {
                Instantiate(abilitiesGO, abilitiesDestination.transform).GetComponent<AbilityShower>().ActivateAbility(ability.ability);
            }
            else
            {
                Instantiate(abilitiesGO, abilitiesDestination.transform).GetComponent<AbilityShower>().LockAbility();
            }
        }

        foreach (int move in pkmn.lvl1Moves)
        {
            Instantiate(moveGO, movesDestination.transform).GetComponent<MovShower>().SetMove(pkmn.basePkmn.lvl1LearnableMoves[move],pkmn);
        }

        foreach (int move in pkmn.lvl2Moves)
        {
            Instantiate(moveGO, movesDestination.transform).GetComponent<MovShower>().SetMove(pkmn.basePkmn.lvl2LearnableMoves[move],pkmn);
        }

        foreach (int move in pkmn.lvl3Moves)
        {
            Instantiate(moveGO, movesDestination.transform).GetComponent<MovShower>().SetMove(pkmn.basePkmn.lvl3LearnableMoves[move],pkmn);
        }

        UpdateHp();
        UpdatePp();
        UpdateStats();
        UpdateDND();
        UpdateInfo(0);
        UpdateDNDProf();
    }
    private void Update()
    {
        UpdateActionTokens();

        if (pkmn.extraStats.isMega)
        {
            bg.color = MegaBGColorHandler.Instance.megaBGColor;
        }
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

    public void UpdateStats()
    {
        statsText.text = pkmn.stats.hp + "\n" + pkmn.stats.atk + "\n" + pkmn.stats.def + "\n" + pkmn.stats.sAtk + "\n" + pkmn.stats.sDef + "\n" +pkmn.stats.spd;
        statsModText.text = "+" + pkmn.statsMod.atk + "\n" + "+" + pkmn.statsMod.sAtk + "\n" + "+" + pkmn.statsMod.spd;
    }

    public void UpdateDND()
    {
        dndText.text = pkmn.dndStats.con + "\n" + pkmn.dndStats.str + "\n" + pkmn.dndStats.cha + "\n" + pkmn.dndStats.intel + "\n" + pkmn.dndStats.wis + "\n" +
            pkmn.dndStats.dex;
        dndOtherText.text = pkmn.extraStats.proficiencyBonus + "\n" + pkmn.extraStats.baseDC + "\n" + pkmn.extraStats.hitDiceNumber + "d" + pkmn.extraStats.hitDice;
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
}
