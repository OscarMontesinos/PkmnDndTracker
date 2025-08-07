using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PkmnShower : MonoBehaviour
{
    public PkmnSO pkmn;
    public Image portrait;
    public TextMeshProUGUI nameText;

    public void Set(PkmnSO pkmn)
    {
        this.pkmn = pkmn;
        portrait.sprite = pkmn.pkmnSprite;
        nameText.text = pkmn.name;
    }

    public void Set(PkmnSO pkmn, string chName)
    {
        this.pkmn = pkmn;
        portrait.sprite = pkmn.pkmnSprite;
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
            CharacterManager.Instance.DeleteCharacter(nameText.text);
            Destroy(gameObject);
        }
    }



}
