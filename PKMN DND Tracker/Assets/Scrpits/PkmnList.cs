using System.Collections.Generic;
using UnityEngine;

public class PkmnList : MonoBehaviour
{
    public static PkmnList Instance;

    public List<PkmnSO> pkmnList;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    [ContextMenu("Sort List")]
    public void SortList()
    {
        for (int x = 0; x + 1 < pkmnList.Count; x++)
        {
            for (int y = 0; y + 1  < pkmnList.Count; y++)
            {
                while (pkmnList[y].isMega || pkmnList[y].pkmnNumber == pkmnList[y + 1].pkmnNumber)
                {
                    pkmnList.RemoveAt(y);
                }

                if (pkmnList[y].pkmnNumber > pkmnList[y + 1].pkmnNumber)
                {
                    PkmnSO pkmn = pkmnList[y];
                    pkmnList[y] = pkmnList[y+1];
                    pkmnList[y+1] = pkmn;
                }
            }
        }
    }
}
