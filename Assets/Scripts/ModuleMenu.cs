using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModuleMenu : MonoBehaviour
{
    public void SinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayer");
    }
}
