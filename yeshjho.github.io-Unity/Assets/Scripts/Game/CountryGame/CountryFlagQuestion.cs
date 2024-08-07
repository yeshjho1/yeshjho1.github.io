using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CountryFlagQuestion : ICountryGameQuestion
{
    [SerializeField] private Image[] _flagImages;


    public override void Initialize(CountryData countryData)
    {
        base.Initialize(countryData);

        Sprite flag = Resources.Load<Sprite>($"flag/{countryData.ISO3166Code}");
        foreach (Image image in _flagImages)
        {
            image.sprite = flag;
        }
    }
}