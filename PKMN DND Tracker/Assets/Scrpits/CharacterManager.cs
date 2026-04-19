using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public List<CharacterData> data = new List<CharacterData>();

    public Image bg;

    public GameObject pkmnChooserContainer;
    public GameObject pkmnShower;
    public GameObject pkmnGameObject;

    public bool deleteMode;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        int loadIndex = 0;
        while (PlayerPrefs.HasKey("CharactersSaved" + loadIndex))
        {
            data.Add(new CharacterData());
            data[loadIndex].chName = PlayerPrefs.GetString("CharactersSaved" + loadIndex);
            loadIndex++;
        }

        if (PlayerPrefs.HasKey("CharactersSaved0"))
        {
            UploadInfo();
        }
        else
        {
            NewCharacter();
        }
    }


    private void Update()
    {
        if (bg && !deleteMode)
        {
            bg.color = MegaBGColorHandler.Instance.megaBGColor;
        }
        else if (bg)
        {
            bg.color = Color.red;
        }
    }

    public void CreateCharacterShowers()
    {
        if (pkmnChooserContainer)
        {
            foreach (CharacterData data in data)
            {
                foreach (PkmnSO pkmn in PkmnList.Instance.pkmnList)
                {
                    if (data.pkmnName == pkmn.name)
                    {
                        Instantiate(pkmnShower, pkmnChooserContainer.transform).GetComponent<PkmnShower>().Set(pkmn, data.chName);
                    }
                }

            }
        }
    }

    public void UploadInfo()
    {
        foreach (CharacterData data in data)
        {
            data.chName =  PlayerPrefs.GetString(data.chName + "ChName");
            data.pkmnName =  PlayerPrefs.GetString(data.chName + "PkmnName");
            data.level =  PlayerPrefs.GetInt(data.chName + "Level");

            data.currentHP = PlayerPrefs.GetInt(data.chName + "HP");
            data.currentXP = PlayerPrefs.GetInt(data.chName + "XP");
            data.currentPP = PlayerPrefs.GetInt(data.chName + "PP");


            data.stats.con = PlayerPrefs.GetInt(data.chName + "Con");
            data.stats.str = PlayerPrefs.GetInt(data.chName + "Str");
            data.stats.cha = PlayerPrefs.GetInt(data.chName + "Cha");
            data.stats.intel = PlayerPrefs.GetInt(data.chName + "Intel");
            data.stats.wis = PlayerPrefs.GetInt(data.chName + "Wis");
            data.stats.dex = PlayerPrefs.GetInt(data.chName + "Dex");

            data.lvl1MoveCount = PlayerPrefs.GetInt(data.chName + "lvl1MoveCount");
            data.lvl2MoveCount = PlayerPrefs.GetInt(data.chName + "lvl2MoveCount");
            data.lvl3MoveCount = PlayerPrefs.GetInt(data.chName + "lvl3MoveCount");

            int i = 0;
            while (i < data.lvl1MoveCount)
            {
                data.lvl1Moves.Add(PlayerPrefs.GetInt(data.chName + "lvl1Move" + i));
                i++;
            }

            i = 0;
            while (i < data.lvl2MoveCount)
            {
                data.lvl2Moves.Add(PlayerPrefs.GetInt(data.chName + "lvl2Move" + i));
                i++;
            }

            i = 0;
            while (i < data.lvl3MoveCount)
            {
                data.lvl3Moves.Add(PlayerPrefs.GetInt(data.chName + "lvl3Move" + i));
                i++;
            }

            if (PlayerPrefs.HasKey(data.chName + "item" + "Count"))
            {
                data.itemCount = PlayerPrefs.GetInt(data.chName + "item" + "Count");
                if (PlayerPrefs.HasKey(data.chName + "item" + (data.itemCount - 1) + "name"))
                    {
                    i = 0;
                    while (i < data.itemCount)
                    {
                        data.itemsName.Add(PlayerPrefs.GetString(data.chName + "item" + i + "name"));
                        data.itemsQuantity.Add(PlayerPrefs.GetInt(data.chName + "item" + i + "quantity"));
                        i++;
                    }
                }
            }
        }

        CreateCharacterShowers();
    }

    public void UploadInfo(string chName,out PkmnSO pkmn, out CharacterData data)
    {
        data = new CharacterData(); 
        pkmn = null;

        foreach (CharacterData savedData in this.data)
        {
            if (chName == savedData.chName)
            {
                foreach (PkmnSO listedPkmn in PkmnList.Instance.pkmnList)
                {
                    if (savedData.pkmnName == listedPkmn.name)
                    {
                        pkmn = listedPkmn;
                    }
                }

                data.chName = PlayerPrefs.GetString(chName + "ChName");
                data.pkmnName = PlayerPrefs.GetString(chName + "PkmnName");
                data.level = PlayerPrefs.GetInt(chName + "Level");

                data.currentHP = PlayerPrefs.GetInt(chName + "HP");
                data.currentXP = PlayerPrefs.GetInt(chName + "XP");
                data.currentPP = PlayerPrefs.GetInt(chName + "PP");


                data.stats.con = PlayerPrefs.GetInt(chName + "Con");
                data.stats.str = PlayerPrefs.GetInt(chName + "Str");
                data.stats.cha = PlayerPrefs.GetInt(chName + "Cha");
                data.stats.intel = PlayerPrefs.GetInt(chName + "Intel");
                data.stats.wis = PlayerPrefs.GetInt(chName + "Wis");
                data.stats.dex = PlayerPrefs.GetInt(chName + "Dex");

                data.lvl1MoveCount = PlayerPrefs.GetInt(chName + "lvl1MoveCount");
                data.lvl2MoveCount = PlayerPrefs.GetInt(chName + "lvl2MoveCount");
                data.lvl3MoveCount = PlayerPrefs.GetInt(chName + "lvl3MoveCount");

                int i = 0;
                while(i < data.lvl1MoveCount) 
                { 
                    data.lvl1Moves.Add(PlayerPrefs.GetInt(chName + "lvl1Move" + i));
                    i++;
                }

                i = 0;
                while (i < data.lvl2MoveCount)
                {
                    data.lvl2Moves.Add(PlayerPrefs.GetInt(chName + "lvl2Move" + i));
                    i++;
                }

                i = 0;
                while (i < data.lvl3MoveCount)
                {
                    data.lvl3Moves.Add(PlayerPrefs.GetInt(chName + "lvl3Move" + i));
                    i++;
                }

                if (PlayerPrefs.HasKey(data.chName + "item" + "Count"))
                {
                    data.itemCount = PlayerPrefs.GetInt(chName + "item" + "Count");
                    if (PlayerPrefs.HasKey(data.chName + "item" + (data.itemCount - 1) + "name"))
                    {
                        i = 0;
                        while (i < data.itemCount)
                        {
                            data.itemsName.Add(PlayerPrefs.GetString(data.chName + "item" + i + "name"));
                            data.itemsQuantity.Add(PlayerPrefs.GetInt(data.chName + "item" + i + "quantity"));
                            i++;
                        }
                    }
                }
            }
        }
    }
    public void SafeInfo(Pkmn pkmn)
    {
        if (!PlayerPrefs.HasKey(pkmn.pkmnName + "ChName"))
        {
            int loadIndex = 0;
            while (PlayerPrefs.HasKey("CharactersSaved" + loadIndex))
            {
                loadIndex++;
            }
            PlayerPrefs.SetString("CharactersSaved" + loadIndex, pkmn.pkmnName);
        }
        
        PlayerPrefs.SetString(pkmn.pkmnName + "ChName", pkmn.pkmnName);
        PlayerPrefs.SetString(pkmn.pkmnName + "PkmnName", pkmn.basePkmn.name);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Level", pkmn.lvl);

        PlayerPrefs.SetInt(pkmn.pkmnName + "HP", pkmn.stats.hp);
        PlayerPrefs.SetInt(pkmn.pkmnName + "XP", pkmn.xp);
        PlayerPrefs.SetInt(pkmn.pkmnName + "PP", pkmn.extraStats.pp);

        PlayerPrefs.SetInt(pkmn.pkmnName + "Con", pkmn.baseDndStats.con);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Str", pkmn.baseDndStats.str);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Cha", pkmn.baseDndStats.cha);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Intel", pkmn.baseDndStats.intel);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Wis", pkmn.baseDndStats.wis);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Dex", pkmn.baseDndStats.dex);

        PlayerPrefs.SetInt(pkmn.pkmnName + "lvl1MoveCount", pkmn.lvl1Moves.Count);
        PlayerPrefs.SetInt(pkmn.pkmnName + "lvl2MoveCount", pkmn.lvl2Moves.Count);
        PlayerPrefs.SetInt(pkmn.pkmnName + "lvl3MoveCount", pkmn.lvl3Moves.Count);

        foreach (int move in pkmn.lvl1Moves)
        {
            PlayerPrefs.SetInt(pkmn.pkmnName + "lvl1Move" + pkmn.lvl1Moves.IndexOf(move), move);
        }

        foreach (int move in pkmn.lvl2Moves)
        {
            PlayerPrefs.SetInt(pkmn.pkmnName + "lvl2Move" + pkmn.lvl2Moves.IndexOf(move), move);
        }

        foreach (int move in pkmn.lvl3Moves)
        {
            PlayerPrefs.SetInt(pkmn.pkmnName + "lvl3Move" + pkmn.lvl3Moves.IndexOf(move), move);
        }


        PlayerPrefs.SetInt(pkmn.pkmnName + "item" + "Count", pkmn.inventory.Count);
        int i = 0;
        while (i < pkmn.inventory.Count)
        {
            PlayerPrefs.SetString(pkmn.pkmnName + "item" + i + "name", pkmn.inventory[i].itemName);
            PlayerPrefs.SetInt(pkmn.pkmnName + "item" + i + "quantity", pkmn.inventory[i].quantity);
            i++;
        }

        UploadInfo();


    }

    public void NewCharacter()
    {
        SceneManager.LoadScene("SceneCreatePokmn");
    }

    public void DeleteCharacter(string chName)
    {
        int i = 0;
        while (PlayerPrefs.GetString("CharactersSaved" + i) != chName)
        {
            i++;
        }
        PlayerPrefs.DeleteKey("CharactersSaved" + i);

        PlayerPrefs.DeleteKey(chName + "ChName");
        PlayerPrefs.DeleteKey(chName + "PkmnName");
        PlayerPrefs.DeleteKey(chName + "Level");

        PlayerPrefs.DeleteKey(chName + "HP");
        PlayerPrefs.DeleteKey(chName + "PP");


        PlayerPrefs.DeleteKey(chName + "Con");
        PlayerPrefs.DeleteKey(chName + "Str");
        PlayerPrefs.DeleteKey(chName + "Cha");
        PlayerPrefs.DeleteKey(chName + "Intel");
        PlayerPrefs.DeleteKey(chName + "Wis");
        PlayerPrefs.DeleteKey(chName + "Dex");


        i = 0;
        while (i < PlayerPrefs.GetInt(chName + "lvl1MoveCount"))
        {
            PlayerPrefs.DeleteKey(chName + "lvl1Move" + i);
            i++;
        }

        i = 0;
        while (i < PlayerPrefs.GetInt(chName + "lvl2MoveCount"))
        {
            PlayerPrefs.DeleteKey(chName + "lvl2Move" + i);
            i++;
        }

        i = 0;
        while (i < PlayerPrefs.GetInt(chName + "lvl3MoveCount"))
        {
            PlayerPrefs.DeleteKey(chName + "lvl3Move" + i);
            i++;
        }

        PlayerPrefs.DeleteKey(chName + "lvl1MoveCount");
        PlayerPrefs.DeleteKey(chName + "lvl2MoveCount");
        PlayerPrefs.DeleteKey(chName + "lvl3MoveCount");


        DeleteInventory(chName);
        


    }

    public void DeleteItem(string chName, int index)
    {
        PlayerPrefs.SetInt(chName + "item" + "Count", PlayerPrefs.GetInt(chName + "item" + "Count")-1);
        PlayerPrefs.DeleteKey(chName + "item" + index + "name");
            PlayerPrefs.DeleteKey(chName + "item" + index + "quantity");
    }

    public void DeleteInventory(string chName)
    {
        int i = 0;
        while (i < PlayerPrefs.GetInt(chName + "item" + "Count"))
        {
            PlayerPrefs.DeleteKey(chName + "item" + i + "name");
            PlayerPrefs.DeleteKey(chName + "item" + i + "quantity");
            i++;
        }
    }
    public void PlayWithCharacter(string chName)
    {
        Pkmn pkmn = Instantiate(pkmnGameObject).GetComponent<Pkmn>();

        PkmnSO pkmnSO = null;
        CharacterData data = null;

        UploadInfo(chName, out pkmnSO, out data);
        pkmn.InitializePkmn(data,pkmnSO,data.chName);

        SceneManager.LoadScene("CharacterSheet");
    }

    public void EnterOrExitDeleteMode()
    {
        deleteMode = !deleteMode;
    }

    [ContextMenu("ResetPlayerPrefs")]
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("SceneCreatePokmn");
    }
}
