using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CountryTextQuestion : ICountryGameQuestion
{
    [SerializeField] private TMP_Text _mainText;
    [SerializeField] private TMP_Text _subText;


    public override void Initialize(CountryData countryData)
    {
        base.Initialize(countryData);

        if (GameManager.Instance.CountryGameFrom == ECountryGameElement.Name)
        {
            _mainText.text = countryData.KoreanNameShort;
            _subText.text = $"({countryData.KoreanNameFull})";
        }
        else  // Capital
        {
            _mainText.text = string.Join(", ", countryData.Capital);
            _subText.text = countryData.CapitalComment;
        }
    }
}
