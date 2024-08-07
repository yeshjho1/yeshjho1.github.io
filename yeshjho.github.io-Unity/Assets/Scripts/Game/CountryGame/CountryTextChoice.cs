using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CountryTextChoice : ICountryGameInput
{
    [SerializeField] private GameObject[] _choiceButtons;

    private int _correctIndex;


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
            _choiceButtons[i].GetComponentInChildren<TMP_Text>().text =
                GameManager.Instance.CountryGameTo == ECountryGameElement.Name ?
                    //$"{data.KoreanNameShort} ({data.KoreanNameFull})" :
                    data.KoreanNameShort :
                    // Capital
                    string.IsNullOrEmpty(data.CapitalComment) ? 
                        string.Join(", ", data.Capital) : 
                        data.CapitalComment;
        }
    }

    private void OnSelect(int index)
    {
        _countryGame.OnAnswer(index == _correctIndex);
    }


    protected override void Awake()
    {
        base.Awake();

        int choiceCount = _choiceButtons.Length;
        for (int i = 0; i < choiceCount; i++)
        {
            int index = i;
            _choiceButtons[i].GetComponent<Button>().onClick.AddListener(() => OnSelect(index));
        }
    }
}
