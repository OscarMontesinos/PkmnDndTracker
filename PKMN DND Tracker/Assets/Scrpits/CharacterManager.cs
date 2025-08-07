using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public List<CharacterData> data = new List<CharacterData>();
    public GameObject pkmnChooserContainer;
    public GameObject pkmnShower;
    public GameObject pkmnGameObject;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.DeleteAll();

        if (Instance)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

       
        if (!PlayerPrefs.HasKey("CharactersSaved"))
        {
            PlayerPrefs.SetInt("CharactersSaved", 0);
        }

        int loadIndex = 0;
        while (PlayerPrefs.HasKey("CharactersSaved" + loadIndex))
        {
            data.Add(new CharacterData());
            loadIndex++;
        }

        PlayerPrefs.SetInt("CharactersSaved",1);

        if (PlayerPrefs.GetInt("CharactersSaved") >= 1)
        {
            UploadInfo();
        }
        else
        {
            NewCharacter();
        }
    }

    public void CreateCharacterShowers()
    {
        if (pkmnChooserContainer)
        {
            foreach (CharacterData data in data)
            {
                foreach (PkmnSO pkmn in PkmnList.Instance.list)
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
            data.currentPP = PlayerPrefs.GetInt(data.chName + "PP");


            data.stats.con = PlayerPrefs.GetInt(data.chName + "Con");
            data.stats.str = PlayerPrefs.GetInt(data.chName + "Str");
            data.stats.cha = PlayerPrefs.GetInt(data.chName + "Cha");
            data.stats.intel = PlayerPrefs.GetInt(data.chName + "Intel");
            data.stats.wis = PlayerPrefs.GetInt(data.chName + "Wis");
            data.stats.dex = PlayerPrefs.GetInt(data.chName + "Dex");

            foreach (int move in data.lvl1Moves)
            {
                data.lvl1Moves[data.lvl1Moves.IndexOf(move)] = PlayerPrefs.GetInt(data.chName + "lvl1Move" + data.lvl1Moves.IndexOf(move));
            }

            foreach(int move in data.lvl2Moves)
            {
                data.lvl2Moves[data.lvl2Moves.IndexOf(move)] = PlayerPrefs.GetInt(data.chName + "lvl2Move" + data.lvl2Moves.IndexOf(move));
            }

            foreach(int move in data.lvl3Moves)
            {
                data.lvl3Moves[data.lvl3Moves.IndexOf(move)] = PlayerPrefs.GetInt(data.chName + "lvl3Move" + data.lvl3Moves.IndexOf(move));
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
                foreach (PkmnSO listedPkmn in PkmnList.Instance.list)
                {
                    if (data.pkmnName == listedPkmn.name)
                    {
                        pkmn = listedPkmn;
                    }
                }

                data.chName = PlayerPrefs.GetString(data.chName + "ChName");
                data.pkmnName = PlayerPrefs.GetString(data.chName + "PkmnName");
                data.level = PlayerPrefs.GetInt(data.chName + "Level");

                data.currentHP = PlayerPrefs.GetInt(data.chName + "HP");
                data.currentPP = PlayerPrefs.GetInt(data.chName + "PP");


                data.stats.con = PlayerPrefs.GetInt(data.chName + "Con");
                data.stats.str = PlayerPrefs.GetInt(data.chName + "Str");
                data.stats.cha = PlayerPrefs.GetInt(data.chName + "Cha");
                data.stats.intel = PlayerPrefs.GetInt(data.chName + "Intel");
                data.stats.wis = PlayerPrefs.GetInt(data.chName + "Wis");
                data.stats.dex = PlayerPrefs.GetInt(data.chName + "Dex");

                foreach (int move in data.lvl1Moves)
                {
                    data.lvl1Moves[data.lvl1Moves.IndexOf(move)] = PlayerPrefs.GetInt(data.chName + "lvl1Move" + data.lvl1Moves.IndexOf(move));
                }

                foreach (int move in data.lvl2Moves)
                {
                    data.lvl2Moves[data.lvl2Moves.IndexOf(move)] = PlayerPrefs.GetInt(data.chName + "lvl2Move" + data.lvl2Moves.IndexOf(move));
                }

                foreach (int move in data.lvl3Moves)
                {
                    data.lvl3Moves[data.lvl3Moves.IndexOf(move)] = PlayerPrefs.GetInt(data.chName + "lvl3Move" + data.lvl3Moves.IndexOf(move));
                }
            }
        }
        

        CreateCharacterShowers();
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
            PlayerPrefs.SetInt("CharactersSaved" + loadIndex, 0);
        }

        PlayerPrefs.SetString(pkmn.pkmnName + "ChName", pkmn.pkmnName);
        PlayerPrefs.SetString(pkmn.pkmnName + "PkmnName", pkmn.basePkmn.name);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Level", pkmn.lvl);

        PlayerPrefs.SetInt(pkmn.pkmnName + "HP", pkmn.stats.hp);
        PlayerPrefs.SetInt(pkmn.pkmnName + "PP", pkmn.extraStats.pp);

        PlayerPrefs.SetInt(pkmn.pkmnName + "Con", pkmn.dndStats.con);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Str", pkmn.dndStats.str);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Cha", pkmn.dndStats.cha);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Intel", pkmn.dndStats.intel);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Wis", pkmn.dndStats.wis);
        PlayerPrefs.SetInt(pkmn.pkmnName + "Dex", pkmn.dndStats.dex);

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

        UploadInfo();


    }

    public void NewCharacter()
    {
        SceneManager.LoadScene("SceneCreatePokmn");
    }

    public void PlayWithCharacter(string chName)
    {
        Pkmn pkmn = Instantiate(pkmnGameObject).GetComponent<Pkmn>();

        PkmnSO pkmnSO = null;
        CharacterData data=null;

        UploadInfo(chName, out pkmnSO, out data);
        pkmn.InitializePkmn(data,pkmnSO,data.chName);

        SceneManager.LoadScene("CharacterSheet");
    }
}
