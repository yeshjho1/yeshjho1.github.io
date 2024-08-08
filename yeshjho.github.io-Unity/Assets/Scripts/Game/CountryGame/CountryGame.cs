using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class CountryGamePart : MonoBehaviour
{
    protected CountryGame _countryGame;

    protected virtual void Awake()
    {
        _countryGame = FindAnyObjectByType<CountryGame>();
    }
}


public class ICountryGameQuestion : CountryGamePart
{
    public virtual void Initialize(CountryData countryData) { }

    public virtual void OnResultShown() {}
}


public class ICountryGameInput : CountryGamePart
{
    public virtual void Initialize(CountryData countryData) { }
}


public class ICountryGameResult : CountryGamePart
{
    [SerializeField] private GameObject _correctPart;
    [SerializeField] private GameObject _wrongPart;
    [SerializeField] private CountryInformation _countryInformation;

    public virtual void Initialize(CountryData countryData, bool isCorrect)
    {
        _correctPart.SetActive(isCorrect);
        _wrongPart.SetActive(!isCorrect);
        _countryInformation.Initialize(countryData);
    }

    public void OnNext()
    {
        _countryGame.NextQuiz();
    }
}


public class CountryGame : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _progressText;

    private ICountryGameQuestion _questionPart;
    private ICountryGameInput _inputPart;
    private ICountryGameResult _resultPart;

    private int _score;
    private CountryData _currentCountryData;
    private List<string> _wrongCountryCodes = new();

    private int _totalCountryCount;
    private List<string> _countryCodePool;


    public void NextQuiz()
    {
        if (_countryCodePool.Count == 0)
        {
            GameManager.Instance.GameResult = new CountryGameResult
            {
                Score = _score,
                TotalCountryCount = _totalCountryCount,
                WrongCountryCodes = _wrongCountryCodes
            };
            UnityEngine.SceneManagement.SceneManager.LoadScene("CountriesResult");
            return;
        }

        _inputPart.gameObject.SetActive(true);
        _resultPart.gameObject.SetActive(false);

        string selectedCode = _countryCodePool[0];
        _currentCountryData = GameManager.Instance.CountryDataStorage.CountryData[selectedCode];
        _countryCodePool.RemoveAt(0);

        _questionPart.Initialize(_currentCountryData);
        _inputPart.Initialize(_currentCountryData);

        _progressText.text = $"{_totalCountryCount - _countryCodePool.Count}/{_totalCountryCount}";
    }

    public void OnAnswer(bool isCorrect)
    {
        _inputPart.gameObject.SetActive(false);
        _resultPart.gameObject.SetActive(true);

        _questionPart.OnResultShown();

        _resultPart.Initialize(_currentCountryData, isCorrect);

        if (isCorrect)
        {
            _score++;
            _scoreText.text = _score.ToString();
        }
        else
        {
            _wrongCountryCodes.Add(_currentCountryData.ISO3166Code);
        }
    }

    private void Awake()
    {
        var rng = new System.Random();
        _countryCodePool = GameManager.Instance.CountryDataStorage.CountryData.Keys
            .Where(x => !GameManager.Instance.CountryDataStorage.Option.CountryCodesToExclude.Contains(x))
            .Where(x => GameManager.Instance.CountryRange switch
            {
                ECountryRange.Asia => GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("아시아"),
                ECountryRange.Europe => GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("유럽"),
                ECountryRange.Africa => GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("아프리카"),
                ECountryRange.NorthAmerica => GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("북아메리카"),
                ECountryRange.SouthAmerica => GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("남아메리카"),
                ECountryRange.America => GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("북아메리카") ||
                                         GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("남아메리카"),
                ECountryRange.Oceania => GameManager.Instance.CountryDataStorage.CountryData[x].Continents.Contains("오세아니아"),
                _ => true
            })
            .OrderBy(_ => rng.Next()).Take(GameManager.Instance.CountryRange switch
            {
                ECountryRange.Random100 => 100,
                ECountryRange.Random50 => 50,
                ECountryRange.Random10 => 10,
                ECountryRange.Random1 => 1,
                _ => GameManager.Instance.CountryDataStorage.CountryData.Count
            }).ToList();
        _totalCountryCount = _countryCodePool.Count;

        _questionPart = FindAnyObjectByType<ICountryGameQuestion>();
        _inputPart = FindAnyObjectByType<ICountryGameInput>();
        _resultPart = FindAnyObjectByType<ICountryGameResult>();

        _scoreText.text = "0";
    }

    private void Start()
    {
        NextQuiz();
    }
}
