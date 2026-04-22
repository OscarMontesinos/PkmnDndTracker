using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PkmnShower : MonoBehaviour
{
    public PkmnSO pkmn;
    public Image sprite;
    public TextMeshProUGUI nameText;

    public void Set(PkmnSO pkmn)
    {
        this.pkmn = pkmn;
        sprite.sprite = pkmn.pkmnPortraits[0];
        nameText.text = pkmn.name;
    }

    public void Set(PkmnSO pkmn, string chName, int portrait)
    {
        PlayerPrefs.SetInt(pkmn.name + "Portrait", 0);
        this.pkmn = pkmn;

        sprite.sprite = pkmn.pkmnPortraits[portrait];
        nameText.text = chName;
    }

    public void Choose()
    {
        CreationHandler.Instance.pkmn = pkmn;
        CreationHandler.Instance.SwipeScreen(1);
    }

    public void PlayWith()
    {
        if (!CharacterManager.Instance.deleteMode)
        {
            CharacterManager.Instance.PlayWithCharacter(nameText.text);
        }
        else
        {
            Hub.Instance.DeletePkmnMenu(pkmn, nameText.text, pkmn.pkmnPortraits.IndexOf(sprite.sprite));
            Hub.Instance.targetShower = gameObject;
        }
    }



}
