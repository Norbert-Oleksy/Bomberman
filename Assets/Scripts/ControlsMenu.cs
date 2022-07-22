using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    public GameObject HighestscoreText;
    public GameObject HigheststageText;

    public void SetStats()
    {
        if (PlayerPrefs.HasKey("SinglePlayerHighestScore"))
        {
            HighestscoreText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("SinglePlayerHighestScore").ToString();
        }

        if (PlayerPrefs.HasKey("SinglePlayerHighestStage"))
        {
            HigheststageText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("SinglePlayerHighestStage").ToString();
        }
    }
}
