using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CountriesStudy : MonoBehaviour
{
    private enum EThumbnailType
    {
        Name,
        Capital,
        Flag
    }
    private readonly Dictionary<EThumbnailType, string> _thumbnailTypeTextMap = new()
    {
        { EThumbnailType.Name, "나라 이름" },
        { EThumbnailType.Capital, "수도" },
        { EThumbnailType.Flag, "국기" }
    };

    private readonly List<ECountryRange> _countryRanges = new()
    {
        ECountryRange.All,
        ECountryRange.Asia,
        ECountryRange.Europe,
        ECountryRange.Africa,
        ECountryRange.NorthAmerica,
        ECountryRange.SouthAmerica,
        ECountryRange.America,
        ECountryRange.Oceania
    };
    private readonly Dictionary<ECountryRange, string> _countryRangeTextMap = new()
    {
        { ECountryRange.All, "전체" },
        { ECountryRange.Asia, "아시아" },
        { ECountryRange.Europe, "유럽" },
        { ECountryRange.Africa, "아프리카" },
        { ECountryRange.NorthAmerica, "북아메리카" },
        { ECountryRange.SouthAmerica, "남아메리카" },
        { ECountryRange.America, "아메리카" },
        { ECountryRange.Oceania, "오세아니아" }
    };


    [SerializeField] private GameObject _gridButtonPrefab;

    [SerializeField] private GameObject _scrollContent;
    [SerializeField] private TMP_Text _thumbnailTypeText;
    [SerializeField] private TMP_Text _countryRangeText;
    [SerializeField] private TMP_InputField _searchInputField;
    [SerializeField] private CountryInformation _countryInformation;
    [SerializeField] private GameObject _searchIcon;


    private List<CountryData> _countries;
    private List<Button> _gridButtons = new();

    private EThumbnailType _thumbnailType = EThumbnailType.Name;
    private ECountryRange _countryRange = ECountryRange.All;


    public void CycleThumbnailType()
    {
        _thumbnailType = (EThumbnailType)(((int)_thumbnailType + 1) % 3);
        _thumbnailTypeText.text = $"표시: {_thumbnailTypeTextMap[_thumbnailType]}";

        _countries.Sort((x, y) => _thumbnailType switch
        {
            EThumbnailType.Name => x.KoreanNameShort.CompareTo(y.KoreanNameShort),
            EThumbnailType.Capital => string.Join(", ", x.Capitals).CompareTo(string.Join(", ", y.Capitals)),
            EThumbnailType.Flag => x.KoreanNameShort.CompareTo(y.KoreanNameShort),
            _ => 0
        });

        for (int i = 0; i < _countries.Count; i++)
        {
            InitializeButton(_gridButtons[i], _countries[i]);
        }

        FilterCountries();
    }

    public void CycleCountryRange()
    {
        _countryRange = _countryRanges[(_countryRanges.IndexOf(_countryRange) + 1) % _countryRanges.Count];
        _countryRangeText.text = $"범위: {_countryRangeTextMap[_countryRange]}";

        FilterCountries();
    }

    public void ClearSearch()
    {
        _searchInputField.text = "";
    }


    private void FilterCountries()
    {
        for (int i = 0; i < _countries.Count; i++)
        {
            CountryData data = _countries[i];
            _gridButtons[i].gameObject.SetActive(
                _countryRange switch
                {
                    ECountryRange.Asia => data.Continents.Contains("아시아"),
                    ECountryRange.Europe => data.Continents.Contains("유럽"),
                    ECountryRange.Africa => data.Continents.Contains("아프리카"),
                    ECountryRange.NorthAmerica => data.Continents.Contains("북아메리카"),
                    ECountryRange.SouthAmerica => data.Continents.Contains("남아메리카"),
                    ECountryRange.America => data.Continents.Contains("북아메리카") ||
                                             data.Continents.Contains("남아메리카"),
                    ECountryRange.Oceania => data.Continents.Contains("오세아니아"),
                    _ => true
                } &&
                (
                    string.IsNullOrEmpty(_searchInputField.text) ||
                    string.IsNullOrWhiteSpace(_searchInputField.text) ||
                    data.KoreanNames.Any(x => x.Contains(_searchInputField.text)) ||
                    data.Capitals.Any(x => x.Contains(_searchInputField.text))
                )
            );
        }
    }

    private void InitializeButton(Button button, CountryData country)
    {
        var text = button.GetComponentInChildren<TMP_Text>();
        var image = button.GetComponentsInChildren<Image>()[1];

        text.enabled = _thumbnailType != EThumbnailType.Flag;
        image.enabled = _thumbnailType == EThumbnailType.Flag;

        switch (_thumbnailType)
        {
            case EThumbnailType.Name:
                text.text = country.KoreanNameShort;
                break;
            case EThumbnailType.Capital:
                text.text = string.Join(", ", country.Capital);
                break;
            case EThumbnailType.Flag:
                image.sprite = Resources.Load<Sprite>($"flag/{country.ISO3166Code}");
                break;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            _countryInformation.Initialize(country);
            _countryInformation.gameObject.SetActive(true);
        });
    }

    private void Awake()
    {
        _countries = GameManager.Instance.CountryDataStorage.CountryData
            .Where(x => !GameManager.Instance.CountryDataStorage.Option.CountryCodesToExclude.Contains(x.Key))
            .Select(x => x.Value)
            .ToList();

        _countries.Sort((x, y) => x.KoreanNameShort.CompareTo(y.KoreanNameShort));

        foreach (CountryData country in _countries)
        {
            Button button = Instantiate(_gridButtonPrefab, _scrollContent.transform).GetComponent<Button>();
            _gridButtons.Add(button);
            InitializeButton(button, country);
        }

        _searchInputField.onValueChanged.AddListener(_ =>
        {
            FilterCountries();
            _searchIcon.SetActive(string.IsNullOrEmpty(_searchInputField.text));
        });
    }
}
