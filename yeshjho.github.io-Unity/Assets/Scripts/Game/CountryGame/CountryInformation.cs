using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;


public class CountryInformation : MonoBehaviour
{
    [SerializeField] private Image[] _flagImages;
    [SerializeField] private TMP_Text _fullNameText;
    [SerializeField] private TMP_Text _shortNameText;
    [SerializeField] private TMP_Text _capitalText;
    [SerializeField] private TMP_Text _capitalCommentText;


    public void Initialize(CountryData countryData)
    {
        Sprite flag = Resources.Load<Sprite>($"flag/{countryData.ISO3166Code}");

        foreach (Image image in _flagImages)
        {
            image.sprite = flag;
        }

        if (_fullNameText != null)
        {
            _fullNameText.text = countryData.KoreanNameShort;
        }
        if (_shortNameText != null)
        {
            _shortNameText.text = $"({countryData.KoreanNameFull})";
        }
        if (_capitalText != null)
        {
            _capitalText.text = string.Join(", ", countryData.Capital);
        }
        if (_capitalCommentText != null)
        {
            _capitalCommentText.text = countryData.CapitalComment;
        }
    }

    public void Initialize(string iso3166Code)
    {
        CountryData countryData = GameManager.Instance.CountryDataStorage.CountryData[iso3166Code];
        Initialize(countryData);
    }
}
