using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _nonUNCountryListElement;

    [SerializeField] private Toggle _afghanistanPrevToggle;
    [SerializeField] private Toggle _afghanistanTalebanToggle;
    [SerializeField] private GameObject _nonUNCountryList;


    private void Start()
    {
        CountryDataStorage countriesData = GameManager.Instance.CountryDataStorage;
        CountryDataOption countriesOption = countriesData.Option;
        _afghanistanPrevToggle.isOn = !countriesOption.CountryCodesToExclude.Contains("af_1");
        _afghanistanTalebanToggle.isOn = !countriesOption.CountryCodesToExclude.Contains("af_0");

        _afghanistanPrevToggle.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                countriesOption.CountryCodesToExclude.Add("af_0");
                countriesOption.CountryCodesToExclude.Remove("af_1");
            }
            else
            {
                countriesOption.CountryCodesToExclude.Remove("af_0");
                countriesOption.CountryCodesToExclude.Add("af_1");
            }
            _afghanistanTalebanToggle.SetIsOnWithoutNotify(!isOn);
        });
        _afghanistanTalebanToggle.onValueChanged.AddListener(isOn =>
        {
            if (isOn)
            {
                countriesOption.CountryCodesToExclude.Remove("af_0");
                countriesOption.CountryCodesToExclude.Add("af_1");
            }
            else
            {
                countriesOption.CountryCodesToExclude.Add("af_0");
                countriesOption.CountryCodesToExclude.Remove("af_1");
            }
            _afghanistanPrevToggle.SetIsOnWithoutNotify(!isOn);
        });

        int nonUNCountryCount = 0;
        IEnumerable<CountryData> nonUNCountries = countriesData.CountryData.Values
            .Where(country => !country.HasUNMembership && country.ISO3166Code != "af_0");
        foreach (CountryData country in nonUNCountries)
        {
            nonUNCountryCount++;
            GameObject countryListElement = Instantiate(_nonUNCountryListElement, _nonUNCountryList.transform);

            Toggle toggle = countryListElement.GetComponentInChildren<Toggle>();
            toggle.isOn = !countriesOption.CountryCodesToExclude.Contains(country.ISO3166Code);
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    countriesOption.CountryCodesToExclude.Remove(country.ISO3166Code);
                }
                else
                {
                    countriesOption.CountryCodesToExclude.Add(country.ISO3166Code);
                }
            });

            countryListElement.transform.Find("Flag").GetComponent<Image>().sprite = 
                Resources.Load<Sprite>($"flag/{country.ISO3166Code}");

            countryListElement.transform.Find("FullName").GetComponent<TMP_Text>().text = country.KoreanNameShort;
            countryListElement.transform.Find("ShortName").GetComponent<TMP_Text>().text = $"({country.KoreanNameFull})";
        }

        float elementHeight = _nonUNCountryListElement.GetComponent<RectTransform>().rect.height;
        float interval = _nonUNCountryList.GetComponent<VerticalLayoutGroup>().spacing;
        _nonUNCountryList.GetComponent<RectTransform>().sizeDelta = new Vector2(0, elementHeight * nonUNCountryCount + interval * (nonUNCountryCount - 1));
    }
}
