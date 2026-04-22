using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovShower : MonoBehaviour
{
    MoveSO move;
    public TextMeshProUGUI movName;
    public TextMeshProUGUI precision;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI range;
    public TextMeshProUGUI area;
    public TextMeshProUGUI salvationName;
    public TextMeshProUGUI salvationValue;
    public TextMeshProUGUI duration;
    public TextMeshProUGUI actionType;
    public TextMeshProUGUI ppCost;
    public TextMeshProUGUI description;

    public Image mainPanel;
    public Image titlePanel;
    public Image moveType;
    public Image moveClass;

    public GameManager.Type normalTypeConversion = GameManager.Type.Normal;

    public void CheckAbilities(Pkmn pkmn)
    {
        if (pkmn.CheckAbilityName("Piel Feérica"))
        {
            normalTypeConversion = GameManager.Type.Fairy;
        }
    }
    
    public virtual void SetMove(MoveSO move, Pkmn pkmn)
    {
        this.move = move;

        movName.text = move.moveName;
        precision.text = move.precision;

        CheckAbilities(pkmn);

        if (move.dmgDices == 0 && move.dmgDiceType == 0)
        {
            dmg.text = "";
        }
        else if (move.useHitDice)
        {
            dmg.text = move.dmgDices + "d" + pkmn.extraStats.hitDice;
        }
        else
        {
            float dices = move.dmgDices;
            if (move.type == pkmn.type1 || move.type == pkmn.type2 || (move.type == GameManager.Type.Normal && normalTypeConversion != GameManager.Type.Normal))
            {
                float diceMult = 1.5f;

                if (pkmn.CheckAbilityName("Adaptabilidad"))
                {
                    diceMult = 2;
                }
                    
                dices *= diceMult;
            }
            dmg.text = Mathf.Floor(dices).ToString("F0") + "d" + move.dmgDiceType;
        }

        range.text = move.range.ToString();
        area.text = move.area.ToString();

        if (move.savingThrowType != "-")
        {
            salvationName.text = move.savingThrowType;
            salvationValue.text = (pkmn.extraStats.baseDC * move.savingThrowMultiplier).ToString("F0");
        }
        else
        {
            salvationName.text = "-";
            salvationValue.text = "-";

        }

            duration.text = move.duration;

        string actionType = "";
        if (move.isAction)
        {
            actionType += "A\n";
        }
        if (move.isBonusAction)
        {
            actionType += "B\n";
        }
        if (move.isReaction)
        {
            actionType += "R\n";
        }
        if (move.hasPriority)
        {
            actionType += "P\n";
        }
        this.actionType.text = actionType;

        ppCost.text = "PP: " + move.pps.ToString();
        description.text = move.description;

        foreach(GameManager.TypeVisuals type in GameManager.Instance.typesVisuals)
        {
            if((type.type == move.type && move.type != GameManager.Type.Normal) || (move.type == GameManager.Type.Normal && type.type == normalTypeConversion))
            {
                mainPanel.color = new Color(type.color.r, type.color.g, type.color.b, mainPanel.color.a);
                titlePanel.color = new Color(type.color.r, type.color.g, type.color.b, mainPanel.color.a);

                moveType.sprite = type.sprite;

                switch (move.moveClass)
                {
                    case GameManager.MoveClass.Physical:
                        moveClass.sprite = GameManager.Instance.fisicalMovSpr;
                        break;
                    case GameManager.MoveClass.Special:
                        moveClass.sprite = GameManager.Instance.specialMovSpr;
                        break;
                    case GameManager.MoveClass.Status:
                        moveClass.sprite = GameManager.Instance.statusMovSpr;
                        break;

                }

                return;
            }
        }

    }



    public void Activate()
    {
        if (move.pps <= Pkmn.Instance.extraStats.pp)
        {
            Pkmn.Instance.ChangePP(-move.pps);
            UIManager.Instance.SwitchScreen(0);
        }
    }
}
