using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
    public void Default()
    {
        SceneManager.LoadScene("Arena1v1");
    }

    public void Boxys()
    {
        SceneManager.LoadScene("Arena1v1_Boxys");
    }

    public void Cave()
    {
        SceneManager.LoadScene("Arena1v1_Cave");
    }
}
