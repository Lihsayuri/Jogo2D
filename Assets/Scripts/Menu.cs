using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


public class Menu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadStory()
    {
        SceneManager.LoadScene(5);
    }


}
