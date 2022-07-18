using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject ExitButton;
    public GameObject InfoText;


    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
