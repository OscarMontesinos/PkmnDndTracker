using System.Collections.Generic;
using UnityEngine;

public class EquipmentList : MonoBehaviour
{
    public static EquipmentList Instance;

    public List<EquipmentSO> eqList;

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
        eqList.Sort();
    }
}
