using TMPro;
using UnityEngine;

public class ReminderShower : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void SetMove(MoveSO move)
    {
        string actionType = "";
        if (move.isReaction) actionType = "Reacción";
        else if (move.hasPriority) actionType = "Prioridad";
        else
        {
            Destroy(gameObject);
        }
            nameText.text = actionType + ": " + move.moveName;

        descriptionText.text = move.description;
    }
    public void SetAbility(AbilitySO ability)
    {
        nameText.text = "Habilidad: " + ability.abName;
        descriptionText.text = ability.description;
    }
}
