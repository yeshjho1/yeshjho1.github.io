using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CountryRangeSelection : MonoBehaviour
{
    public void SetCountryRange(int range)
    {
        GameManager.Instance.CountryRange = (ECountryRange)range;
        UnityEngine.SceneManagement.SceneManager.LoadScene("FlagToTextField");
    }
}
