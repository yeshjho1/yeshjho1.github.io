using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CountryFlagChoice : ICountryGameInput
{
    [SerializeField] private GameObject[] _choiceButtons;
    private Image[] _buttonImages;
    [SerializeField] private GameObject _flagWindow;
    [SerializeField] private Image[] _flagWindowImages;

    private int _correctIndex;
    private int _selectedIndex = -1;


    public void OnConfirm()
    {
        _countryGame.OnAnswer(_selectedIndex == _correctIndex);
        _flagWindow.SetActive(false);
    }

    public override void Initialize(CountryData countryData)
    {
        base.Initialize(countryData);

        int choiceCount = _choiceButtons.Length;
        var rng = new System.Random();
        List<string> choiceCodes = GameManager.Instance.CountryDataStorage.CountryData.Keys
            .Where(x => !GameManager.Instance.CountryDataStorage.Option.CountryCodesToExclude.Contains(x) && x != countryData.ISO3166Code)
            .OrderBy(_ => rng.Next()).Take(choiceCount - 1).ToList();
        _correctIndex = rng.Next(choiceCount);
        choiceCodes.Insert(_correctIndex, countryData.ISO3166Code);

        for (int i = 0; i < choiceCount; i++)
        {
            CountryData data = GameManager.Instance.CountryDataStorage.CountryData[choiceCodes[i]];
            _buttonImages[i].sprite = Resources.Load<Sprite>($"flag/{data.ISO3166Code}");
        }
    }


    private void OnSelect(int index)
    {
        _selectedIndex = index;

        _flagWindow.SetActive(true);

        foreach (Image image in _flagWindowImages)
        {
            image.sprite = _buttonImages[index].sprite;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        int choiceCount = _choiceButtons.Length;
        _buttonImages = new Image[choiceCount];
        for (int i = 0; i < choiceCount; i++)
        {
            int index = i;
            _choiceButtons[i].GetComponent<Button>().onClick.AddListener(() => OnSelect(index));
            foreach (Image image in _choiceButtons[i].GetComponentsInChildren<Image>())
            {
                if (image.GetComponent<Button>() == null)
                {
                    _buttonImages[i] = image;
                    break;
                }
            }
        }

        _flagWindow.SetActive(false);
    }
}
