using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Hub : MonoBehaviour
{
    public static Hub Instance;

    public GameObject selectorPanel;
    public GameObject resetPanel;
    public GameObject removePanel;

    public PkmnShower pkmnToRemove;
    [HideInInspector]
    public GameObject targetShower;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GoToHub();
    }

    public void GoToHub()
    {
        DeactivateAll();
        selectorPanel.SetActive(true);
    }

    void DeactivateAll()
    {
        selectorPanel.SetActive(false);
        resetPanel.SetActive(false);
        removePanel.SetActive(false);
    }
    public void ResetPkmnMenu()
    {
        DeactivateAll();
        resetPanel.SetActive(true);
    }

    public void DeletePkmnMenu(PkmnSO pkmn, string nameText, int portrait)
    {
        DeactivateAll();
        removePanel.SetActive(true);
        pkmnToRemove.Set(pkmn, nameText, portrait);
    }

    public void DeletePkmn()
    {
        CharacterManager.Instance.DeleteCharacter(pkmnToRemove.nameText.text);
        Destroy(targetShower);
        GoToHub();
    }
}
