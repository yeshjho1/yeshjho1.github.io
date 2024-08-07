using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ECountryGameElement
{
    Name = 0,
    Flag = 1,
    Capital = 2,
}

public enum ECountryGameType
{
    TextField = 0,
    MultipleChoices = 1,
}


public enum ECountryRange
{
    All = 0,
    Random100 = 1,
    Random50 = 2,
    Random10 = 3,
    Random1 = 4,
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

    public ECountryGameElement CountryGameFrom;
    public ECountryGameElement CountryGameTo;
    public ECountryGameType CountryGameType;
    public ECountryRange CountryRange;

    public GameResult GameResult;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        Instance = this;
        CountryDataStorage = new CountryDataStorage(_countriesRawData.text);
    }
}
