using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CountryRangeSelection : MonoBehaviour
{
    public void SetCountryRange(int range)
    {
        GameManager.Instance.CountryRange = (ECountryRange)range;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameManager.Instance.GameMode switch
        {
            EGameMode.FlagToTextField => "FlagToTextFieldInGame",
            EGameMode.FlagToMultipleChoice => "FlagToMultipleChoiceInGame",
            _ => throw new System.ArgumentOutOfRangeException()
        });
    }
}
