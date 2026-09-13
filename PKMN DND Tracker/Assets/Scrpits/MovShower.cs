using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovShower : MonoBehaviour
{
    Pkmn pkmn;
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
        else if (pkmn.CheckAbilityName("Piel Eléctrica"))
        {
            normalTypeConversion = GameManager.Type.Electric;
        }
    }
    
    public virtual void SetMove(MoveSO move, Pkmn pkmn)
    {
        this.pkmn= pkmn;
        this.move = move;

        if (UIManager.Instance)
        {
            UIManager.Instance.movShowerList.Add(this);
        }

        movName.text = move.moveName;

        if (move.precision)
        {
            precision.text = "1d20";
            int mod = 0;
            if (move.moveClass == GameManager.MoveCategory.Special)
            {
                mod += pkmn.statsMod.sAtk;
            }
            else if (move.moveClass == GameManager.MoveCategory.Physical)
            {
                mod += pkmn.statsMod.atk;
            }

            mod += pkmn.eqManager.GetAttackMod(move);

            precision.text += " + " + mod;
        }
        else
        {
            precision.text = "-";
        }

            SetDmgDices();

        range.text = move.range.ToString();
        if (move.area >= 0)
        {
            area.text = move.area.ToString();
        }
        else
        {
            area.text = "Cono";
        }

        if (move.savingThrowType != "-")
        {
            salvationName.text = move.savingThrowType;
            salvationValue.text = pkmn.GetSpellDC(move.scallingSavingThrowType,move.secondaryScallingSavingThrowType,move.combatScallingStat, move.savingThrowMultiplier).ToString("F0");
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

        if(move.extraEffect != "-")
        {
            description.text += "\nEfecto bonus: " + move.extraEffect.ToString();
        }

        foreach(GameManager.TypeVisuals type in GameManager.Instance.typesVisuals)
        {
            if((type.type == move.type && move.type != GameManager.Type.Normal) || (move.type == GameManager.Type.Normal && type.type == normalTypeConversion))
            {
                mainPanel.color = new Color(type.color.r, type.color.g, type.color.b, mainPanel.color.a);
                titlePanel.color = new Color(type.color.r, type.color.g, type.color.b, mainPanel.color.a);

                moveType.sprite = type.sprite;

                switch (move.moveClass)
                {
                    case GameManager.MoveCategory.Physical:
                        moveClass.sprite = GameManager.Instance.fisicalMovSpr;
                        break;
                    case GameManager.MoveCategory.Special:
                        moveClass.sprite = GameManager.Instance.specialMovSpr;
                        break;
                    case GameManager.MoveCategory.Status:
                        moveClass.sprite = GameManager.Instance.statusMovSpr;
                        break;

                }

                return;
            }
        }

    }

    public void SetDmgDices()
    {
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
            float diceMult = 1;
            if (move.type == pkmn.type1 || move.type == pkmn.type2 ||
               (move.type == GameManager.Type.Normal && normalTypeConversion != GameManager.Type.Normal) ||

               ((pkmn.type1 == GameManager.Type.Dragon && pkmn.type2 == GameManager.Type.none && pkmn.lvl >= 10
               && (move.type == GameManager.Type.Water || move.type == GameManager.Type.Electric || move.type == GameManager.Type.Fire
               || move.type == GameManager.Type.Grass))) ||
               
               (pkmn.CheckAbilityName("Cólera") && move.moveClass == GameManager.MoveCategory.Special &&  (pkmn.stats.hp / pkmn.stats.mHp) * 100 <= 50))
            {
                diceMult += 0.5f;

                if (pkmn.CheckAbilityName("Adaptabilidad"))
                {
                    diceMult += 1;
                }
            }
            else if (pkmn.type1 == GameManager.Type.Ground && pkmn.type2 == GameManager.Type.none && pkmn.HasAbility(2))
            {
                if (move.type == GameManager.Type.Rock || move.type == GameManager.Type.Steel)
                {
                    diceMult += 0.5f;
                }
            }
            else if(pkmn.CheckAbilityName("Crin de Fuego") && move.type == GameManager.Type.Fire)
            {
                diceMult += 0.5f;
            }
            if (UIManager.Instance)
            {
                diceMult += (UIManager.Instance.stabDices*0.5f);
            }
            dices *= diceMult;
            if (dices < 1)
            {
                dices = 1;
            }
            dmg.text = Mathf.Floor(dices).ToString("F0") + "d" + move.dmgDiceType;
        }

        if(move.dmgBonus != 0)
        {
            dmg.text += " ";
            if (move.dmgBonus > 0)
            {
                dmg.text += "+";
            }
            dmg.text += move.dmgBonus.ToString();
        }

        if(move.dmgStatBonus != GameManager.DndStatsType.none)
        {
            int statBonus = pkmn.GetDndStatMod(move.dmgStatBonus);
            dmg.text += " ";
            if (statBonus > 0)
            {
                dmg.text += "+";
            }
            dmg.text += statBonus.ToString();
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
