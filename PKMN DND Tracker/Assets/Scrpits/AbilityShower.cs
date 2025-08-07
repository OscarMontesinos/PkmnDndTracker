using TMPro;
using UnityEngine;

public class AbilityShower : MonoBehaviour
{
    public int lvlRequired;
    public GameObject lockedImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void LockAbility()
    {
        lockedImage.SetActive(true);
    }

    public void ActivateAbility(AbilitySO ability)
    {
        nameText.text = ability.abName;
        descriptionText.text = ability.description;
    }
}
