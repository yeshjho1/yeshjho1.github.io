using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EGameMode
{
    FlagToTextField = 0,
}


public class GameManager : MonoBehaviour
{
    public EGameMode GameMode { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void OnSelectGameMode(int gameMode)
    {
        GameMode = (EGameMode)gameMode;
    }
}
