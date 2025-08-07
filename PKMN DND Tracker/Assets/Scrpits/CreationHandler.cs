using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreationHandler : MonoBehaviour
{
    public static CreationHandler Instance;

    public PkmnOnCreation pkmnPlaceholder;

    public Image bg;
    public GameObject buttonForwards;
    public GameObject buttonBackwards;

    int currentScreen;
    public List<Screen> screens;

    [Serializable]
    public struct Screen
    {
        public GameObject screen;
        public bool canGoForward;
        public bool canGoBackward;
    }

    public GameObject pkmnChooserContainer;
    public GameObject pkmnShower;

    public TMP_InputField lvlField;
    public TMP_InputField conField;
    public TMP_InputField strField;
    public TMP_InputField chaField;
    public TMP_InputField intField;
    public TMP_InputField wisField;
    public TMP_InputField dexField;

    int moveIndex;
    public List<GameObject> moveMenus;
    public TextMeshProUGUI moveMenuText;

    public GameObject moveChooserContainer1;
    public GameObject moveChooserContainer2;
    public GameObject moveChooserContainer3;
    public GameObject moveShower;

    public Image finalPortrait;
    public TMP_InputField nameField;

    [HideInInspector]
    public PkmnSO pkmn;
    public int lvl;
    public Pkmn.DndBaseStats dndStats;
    public List<int> lvl1Moves;
    public List<int> lvl2Moves;
    public List<int> lvl3Moves;

    public GameObject pkmnGameObject;



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
        SwipeScreen(0);
        ShowPkmns();
    }
    private void Update()
    {
        bg.color = MegaBGColorHandler.Instance.megaBGColor;
    }

    public void SwipeScreen(int val)
    {
        currentScreen += val;
        if (currentScreen > screens.Count - 1)
        {
            CreatePkmn();
        }

        foreach (Screen screen in screens)
        {
            screen.screen.SetActive(false);
        }

        screens[currentScreen].screen.SetActive(true);
        buttonForwards.SetActive(screens[currentScreen].canGoForward);
        buttonBackwards.SetActive(screens[currentScreen].canGoBackward);
    }

    public void ShowPkmns()
    {
        foreach(PkmnSO pkmn in PkmnList.Instance.list)
        {
            Instantiate(pkmnShower,pkmnChooserContainer.transform).GetComponent<PkmnShower>().Set(pkmn);
        }
    }

    public void SetDNDStats()
    {
        if(lvlField.text != "" && conField.text != "" && strField.text != "" && chaField.text != "" && intField.text != "" && wisField.text != "" && dexField.text != "")
        {
            lvl = int.Parse(lvlField.text);
            dndStats.con = int.Parse(conField.text);
            dndStats.str = int.Parse(strField.text);
            dndStats.cha = int.Parse(chaField.text);
            dndStats.intel = int.Parse(intField.text);
            dndStats.wis = int.Parse(wisField.text);
            dndStats.dex = int.Parse(dexField.text);

            pkmnPlaceholder.basePkmn = pkmn;
            pkmnPlaceholder.lvl = lvl;
            pkmnPlaceholder.SetPkmn();

            ShowMoves();

            SwipeScreen(1);
        }
    }

    public void ShowMoves()
    {
        foreach(MoveSO move in pkmn.lvl1LearnableMoves)
        {
            Instantiate(moveShower, moveChooserContainer1.transform).GetComponent<UnlockableMoveShower>().SetMove(move,pkmnPlaceholder,1);
        }
        foreach(MoveSO move in pkmn.lvl2LearnableMoves)
        {
            Instantiate(moveShower, moveChooserContainer2.transform).GetComponent<UnlockableMoveShower>().SetMove(move,pkmnPlaceholder,2);
        }
        foreach(MoveSO move in pkmn.lvl3LearnableMoves)
        {
            Instantiate(moveShower, moveChooserContainer3.transform).GetComponent<UnlockableMoveShower>().SetMove(move,pkmnPlaceholder,3);
        }
        SwipeMenuMoves(0);
    }

    public void SwipeMenuMoves(int val)
    {
        moveIndex += val;


        if (moveIndex > moveMenus.Count - 1)
        {
            moveIndex = 0;
        }
        else if (moveIndex < 0)
        {
            moveIndex = moveMenus.Count - 1;
        }

        if(moveIndex == 1 && pkmnPlaceholder.lvl < 3)
        {
            moveIndex -= val;
        }

        if(moveIndex == 2 && pkmnPlaceholder.lvl < 20)
        {
            moveIndex -= val;
        }

        foreach (GameObject menu in moveMenus)
        {
            menu.SetActive(false);
        }
        moveMenus[moveIndex].SetActive(true);

        string remainingSlots = "";

        switch (moveIndex)
        {
            case 0:
                remainingSlots = "Espacios restantes: " + (pkmnPlaceholder.extraStats.lvl1MoveSlots - lvl1Moves.Count) ;
                break;
            case 1:
                remainingSlots = "Espacios restantes: " + (pkmnPlaceholder.extraStats.lvl2MoveSlots - lvl2Moves.Count) ;
                break;
            case 2:
                remainingSlots = "Espacios restantes: " + (pkmnPlaceholder.extraStats.lvl3MoveSlots - lvl3Moves.Count) ;
                break;
        }

        moveMenuText.text = "Movimientos de nivel " + (moveIndex + 1) + "\n" + remainingSlots;
    }

    public void SetMoves()
    {
        if(((pkmnPlaceholder.extraStats.lvl1MoveSlots - lvl1Moves.Count) == 0 || pkmnPlaceholder.extraStats.lvl1MoveSlots > pkmn.lvl1LearnableMoves.Count) &&
            ((pkmnPlaceholder.extraStats.lvl2MoveSlots - lvl2Moves.Count) == 0 || pkmnPlaceholder.extraStats.lvl2MoveSlots > pkmn.lvl2LearnableMoves.Count) &&
            ((pkmnPlaceholder.extraStats.lvl3MoveSlots - lvl3Moves.Count) == 0 || pkmnPlaceholder.extraStats.lvl3MoveSlots > pkmn.lvl3LearnableMoves.Count))
        {
            finalPortrait.sprite = pkmn.pkmnSprite;
            SwipeScreen(1);
        }
    }

    public void CreatePkmn()
    {
        if(nameField.text != "" && PlayerPrefs.HasKey(nameField.text + "ChName"))
        {
            Pkmn pkmn = Instantiate(pkmnGameObject).GetComponent<Pkmn>();
            pkmn.InitializePkmn(this, nameField.text);

            CharacterManager.Instance.SafeInfo(pkmn);

            SceneManager.LoadScene("CharacterSheet");
        }
    }
}
