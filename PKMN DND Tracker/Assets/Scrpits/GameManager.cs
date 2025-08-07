using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum Type
    {
        none, Normal, Fighting, Flying, Poison, Ground, Rock, Bug, Ghost, Steel, Fire, Water, Grass, Electric, Psychic, Ice, Dragon, Dark, Fairy
    }
    public enum MoveClass
    {
        Physical, Special, Status
    }
    [Serializable]
   public struct TypeVisuals
    {
        public Type type;
        public Color color;
        public GameObject image;
        public Sprite sprite;
    }

    public List<TypeVisuals> typesVisuals;
    public List<Color> hpColors = new List<Color>();

    public Sprite fisicalMovSpr;
    public Sprite specialMovSpr;
    public Sprite statusMovSpr;



    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
