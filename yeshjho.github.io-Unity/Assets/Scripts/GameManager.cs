using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EGameMode
{
    FlagToTextField = 0,
}


public enum ECountryRange
{
    All = 0,
}


public class GameResult {}

public class CountryGameResult : GameResult
{
    public int Score;
    public int TotalCountryCount;
    public List<string> WrongCountryCodes;
}


public class GameManager : MonoBehaviour
{
    [SerializeField] private TextAsset _countriesRawData;

    public static GameManager Instance { get; private set; }
    public CountryDataStorage CountryDataStorage { get; private set; }

    public EGameMode GameMode { get; private set; }
    public GameResult GameResult;

    private string NextSceneNameByGameMode => GameMode switch
    {
        EGameMode.FlagToTextField => "CountriesRange",
        _ => throw new ArgumentOutOfRangeException()
    };


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        CountryDataStorage = new CountryDataStorage(_countriesRawData.text);
    }


    public void OnSelectGameMode(int gameMode)
    {
        GameMode = (EGameMode)gameMode;
        UnityEngine.SceneManagement.SceneManager.LoadScene(NextSceneNameByGameMode);
    }
}
