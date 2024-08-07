using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelection : MonoBehaviour
{
    public void SetGameMode(int mode)
    {
        GameManager.Instance.GameMode = (EGameMode)mode;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameManager.Instance.GameMode switch
        {
            EGameMode.FlagToTextField => "CountriesRange",
            EGameMode.FlagToMultipleChoice => "CountriesRange",
            _ => throw new System.ArgumentOutOfRangeException()
        });
    }
}
