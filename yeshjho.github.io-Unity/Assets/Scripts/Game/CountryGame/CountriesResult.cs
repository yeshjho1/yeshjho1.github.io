using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountriesResult : MonoBehaviour
{
    [SerializeField] private GameObject _countryListElementPrefab;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameObject _scrollContent;
    [SerializeField] private GameObject _flagWindow;
    [SerializeField] private Image[] _flagImages;
    [SerializeField] private TMP_Text _fullNameText;
    [SerializeField] private TMP_Text _shortNameText;
    [SerializeField] private TMP_Text _capitalText;
    [SerializeField] private TMP_Text _capitalCommentText;


    private void Start()
    {
        _flagWindow.SetActive(false);

        var countryGameResult = (CountryGameResult)GameManager.Instance.GameResult;

        _scoreText.text = $"{countryGameResult.Score}/{countryGameResult.TotalCountryCount}";

        foreach (string countryCode in countryGameResult.WrongCountryCodes)
        {
            CountryData countryData = GameManager.Instance.CountryDataStorage.CountryData[countryCode];

            Sprite flag = Resources.Load<Sprite>($"flag/{countryData.ISO3166Code}");

            GameObject countryListElement = Instantiate(_countryListElementPrefab, _scrollContent.transform);
            foreach (Image image in countryListElement.GetComponentsInChildren<Image>())
            {
                if (image.GetComponent<Button>() == null)
                {
                    image.sprite = flag;
                    image.preserveAspect = true;
                    break;
                }
            }
            countryListElement.GetComponentInChildren<TMP_Text>().text = $"{countryData.KoreanNameShort} - {string.Join(", ", countryData.Capital)}";

            countryListElement.GetComponent<Button>().onClick.AddListener(() =>
            {
                _flagWindow.SetActive(true);

                foreach (Image image in _flagImages)
                {
                    image.sprite = flag;
                }

                _fullNameText.text = countryData.KoreanNameShort;
                _shortNameText.text = $"({countryData.KoreanNameFull})";
                _capitalText.text = string.Join(", ", countryData.Capital);
                _capitalCommentText.text = countryData.CapitalComment;
            });
        }
    }
}
