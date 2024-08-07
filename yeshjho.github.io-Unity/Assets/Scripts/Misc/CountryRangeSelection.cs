using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CountryRangeSelection : MonoBehaviour
{
    public void SetCountryRange(int range)
    {
        GameManager.Instance.CountryRange = (ECountryRange)range;
        string from = GameManager.Instance.CountryGameFrom switch
        {
            ECountryGameElement.Name => "Text",
            ECountryGameElement.Flag => "Flag",
            ECountryGameElement.Capital => "Text",
            _ => throw new System.ArgumentOutOfRangeException()
        };
        string to = GameManager.Instance.CountryGameTo switch
        {
            ECountryGameElement.Name => "Text",
            ECountryGameElement.Flag => "Flag",
            ECountryGameElement.Capital => "Text",
            _ => throw new System.ArgumentOutOfRangeException()
        };
        string gameMode = GameManager.Instance.CountryGameType switch
        {
            ECountryGameType.TextField => "Blank",
            ECountryGameType.MultipleChoices => "Choice",
            _ => throw new System.ArgumentOutOfRangeException()
        };
        UnityEngine.SceneManagement.SceneManager.LoadScene($"{from}To{to}{gameMode}");
    }
}
