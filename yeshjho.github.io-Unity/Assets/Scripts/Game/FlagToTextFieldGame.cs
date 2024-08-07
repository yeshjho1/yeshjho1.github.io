using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagToTextFieldGame : MonoBehaviour
{
    [SerializeField] private Image[] _flagImages;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _inputPart;
    [SerializeField] private GameObject _resultPart;
    [SerializeField] private GameObject _correctPart;
    [SerializeField] private GameObject _wrongPart;
    [SerializeField] private TMP_Text _fullAnswer;
    [SerializeField] private TMP_Text _shortAnswer;

    private int _score;
    private CountryData _currentCountryData;
    private List<string> _wrongCountryCodes = new();

    private int _totalCountryCount;
    private List<string> _countryCodePool;


    public void OnSubmit()
    {
        if (_inputField.text == string.Empty)
        {
            return;
        }

        _inputPart.SetActive(false);
        _resultPart.SetActive(true);
        
        bool isCorrect = _currentCountryData.KoreanNames.Any(countryName =>
            string.Equals(Regex.Replace(_inputField.text, @"\s", ""), Regex.Replace(countryName, @"\s", "")));

        _correctPart.SetActive(isCorrect);
        _wrongPart.SetActive(!isCorrect);

        _fullAnswer.text = _currentCountryData.KoreanNameFull;
        _shortAnswer.text = $"({_currentCountryData.KoreanNameShort})";

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

    public void OnNext()
    {
        NextQuiz();
    }


    private void NextQuiz()
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

        _inputPart.SetActive(true);
        _resultPart.SetActive(false);
        _inputField.text = "";

        _currentCountryData = GameManager.Instance.CountryDataStorage.CountryData[_countryCodePool[0]];
        _countryCodePool.RemoveAt(0);
        
        _progressText.text = $"{_totalCountryCount - _countryCodePool.Count}/{_totalCountryCount}";

        foreach (Image image in _flagImages)
        {
            image.sprite = Resources.Load<Sprite>($"flag/{_currentCountryData.ISO3166Code}");
        }
    }

    private void Start()
    {
        var rng = new System.Random();
        _countryCodePool = GameManager.Instance.CountryDataStorage.CountryData.Keys
            .Where(x => !GameManager.Instance.CountryDataStorage.Option.CountryCodesToExclude.Contains(x))
            .OrderBy(_ => rng.Next()).Take(GameManager.Instance.CountryRange switch
            {
                ECountryRange.All => GameManager.Instance.CountryDataStorage.CountryData.Count,
                ECountryRange.Random100 => 100,
                ECountryRange.Random50 => 50,
                ECountryRange.Random10 => 10,
                ECountryRange.Random1 => 1,
                _ => throw new ArgumentOutOfRangeException()
            }).ToList();
        _totalCountryCount = _countryCodePool.Count;

        _scoreText.text = "0";

        NextQuiz();
    }
}
