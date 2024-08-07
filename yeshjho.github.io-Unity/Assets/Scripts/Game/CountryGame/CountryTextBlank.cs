using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class CountryTextBlank : ICountryGameInput
{
    [SerializeField] private TMP_InputField _inputField;

    private CountryData _currentCountryData;


    public void OnSubmit()
    {
        if (_inputField.text == string.Empty)
        {
            return;
        }

        HashSet<string> toCheckFrom = GameManager.Instance.CountryGameTo == ECountryGameElement.Name
            ? _currentCountryData.KoreanNames
            : _currentCountryData.Capitals;
        bool isCorrect = toCheckFrom.Any(countryName =>
            string.Equals(
                Regex.Replace(_inputField.text, @"\s", ""), 
                Regex.Replace(countryName, @"\s", ""),
                System.StringComparison.InvariantCultureIgnoreCase)
            );
        _countryGame.OnAnswer(isCorrect);
    }

    public override void Initialize(CountryData countryData)
    {
        base.Initialize(countryData);

        _currentCountryData = countryData;
        _inputField.text = "";
    }
}
