using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("WOW playing the game!");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("lolded xD");
    }
}
