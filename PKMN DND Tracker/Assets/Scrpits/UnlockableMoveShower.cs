using UnityEngine;

public class UnlockableMoveShower : MovShower
{
    public GameObject lockedImage;
    public GameObject unlockedImage;

    public int lvl;

    bool locked;

    private void Start()
    {
        LockOrUnlock();
    }
    public void LockOrUnlock()
    {
        switch (lvl)
        {
            case 1:
                if (!locked)
                {
                    locked = true;
                    lockedImage.SetActive(true);
                    unlockedImage.SetActive(false);

                    if (CreationHandler.Instance.lvl1Moves.Contains(transform.GetSiblingIndex()))
                    {
                        CreationHandler.Instance.lvl1Moves.Remove(transform.GetSiblingIndex());
                    }
                }
                else if (locked && CreationHandler.Instance.lvl1Moves.Count < CreationHandler.Instance.pkmnPlaceholder.extraStats.lvl1MoveSlots)
                {
                    locked = false;
                    lockedImage.SetActive(false);
                    unlockedImage.SetActive(true);

                    CreationHandler.Instance.lvl1Moves.Add(transform.GetSiblingIndex());
                }

                CreationHandler.Instance.moveMenuText.text = "Movimientos de nivel 1"  +
                    "\nEspacios restantes: " + (CreationHandler.Instance.pkmnPlaceholder.extraStats.lvl1MoveSlots - CreationHandler.Instance.lvl1Moves.Count);

                break;
            case 2:
                if (!locked)
                {
                    locked = true;
                    lockedImage.SetActive(true);
                    unlockedImage.SetActive(false);

                    if (CreationHandler.Instance.lvl2Moves.Contains(transform.GetSiblingIndex()))
                    {
                        CreationHandler.Instance.lvl2Moves.Remove(transform.GetSiblingIndex());
                    }
                }
                else if (locked && CreationHandler.Instance.lvl2Moves.Count < CreationHandler.Instance.pkmnPlaceholder.extraStats.lvl2MoveSlots)
                {
                    locked = false;
                    lockedImage.SetActive(false);
                    unlockedImage.SetActive(true);

                    CreationHandler.Instance.lvl2Moves.Add(transform.GetSiblingIndex());
                }
                CreationHandler.Instance.moveMenuText.text = "Movimientos de nivel 2" +
                    "\nEspacios restantes: " + (CreationHandler.Instance.pkmnPlaceholder.extraStats.lvl2MoveSlots - CreationHandler.Instance.lvl2Moves.Count);

                break;
            case 3:
                if (!locked)
                {
                    locked = true;
                    lockedImage.SetActive(true);
                    unlockedImage.SetActive(false);

                    if (CreationHandler.Instance.lvl3Moves.Contains(transform.GetSiblingIndex()))
                    {
                        CreationHandler.Instance.lvl3Moves.Remove(transform.GetSiblingIndex());
                    }
                }
                else if (locked && CreationHandler.Instance.lvl3Moves.Count < CreationHandler.Instance.pkmnPlaceholder.extraStats.lvl3MoveSlots)
                {
                    locked = false;
                    lockedImage.SetActive(false);
                    unlockedImage.SetActive(true);

                    CreationHandler.Instance.lvl3Moves.Add(transform.GetSiblingIndex());
                }

                CreationHandler.Instance.moveMenuText.text = "Movimientos de nivel 3" +
                    "\nEspacios restantes: " + (CreationHandler.Instance.pkmnPlaceholder.extraStats.lvl3MoveSlots - CreationHandler.Instance.lvl3Moves.Count);

                break;
        }

    }

    public void SetMove(MoveSO move, Pkmn pkmn, int lvl)
    {
        this.lvl = lvl;
        base.SetMove(move, pkmn);
    }
}
