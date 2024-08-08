using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CountryResultFlag : ICountryGameResult
{
    [SerializeField] private Image[] _images;

    public override void Initialize(CountryData countryData, bool isCorrect)
    {
        base.Initialize(countryData, isCorrect);

        foreach (Image image in _images)
        {
            image.sprite = Resources.Load<Sprite>($"flag/{countryData.ISO3166Code}");
        }
    }
}
