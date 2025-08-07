using System.Collections.Generic;
using UnityEngine;

public class PkmnList : MonoBehaviour
{
    public static PkmnList Instance;

    public List<PkmnSO> list;

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
}
