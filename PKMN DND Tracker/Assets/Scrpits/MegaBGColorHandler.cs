using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBGColorHandler : MonoBehaviour
{
    public static MegaBGColorHandler Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public Color megaBGColor;
}
