using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryGameModeSelection : MonoBehaviour
{
    [SerializeField] private ECountryGameElement _from;
    [SerializeField] private ECountryGameElement _to;
    [SerializeField] private ECountryGameType _type;
    

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.CountryGameFrom = _from;
            GameManager.Instance.CountryGameTo = _to;
            GameManager.Instance.CountryGameType = _type;
            UnityEngine.SceneManagement.SceneManager.LoadScene("CountriesRange");
        });
    }
}
