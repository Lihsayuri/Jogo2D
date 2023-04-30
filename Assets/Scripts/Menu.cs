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
        if (PlayerManager.Instance != null){
            PlayerManager.Instance.weapons.Clear();

        }

        SceneManager.LoadScene(0);
    }
    public void LoadGame()
    {
        if (GameOverPanel.instance != null)
        {
            GameOverPanel.instance.GetComponent<AudioSource>().Stop();
        }
        if (MenuMusic.instance != null)
        {
            MenuMusic.instance.GetComponent<AudioSource>().Stop();
        }
        if (WinPanel.instance != null)
        {
            WinPanel.instance.GetComponent<AudioSource>().Stop();
        }
        if (PlayerManager.Instance != null){
            PlayerManager.Instance.weapons.Clear();

        }
        SceneManager.LoadScene(1);
    }

    public void LoadTutorial()
    {        
        if (GameOverPanel.instance != null)
        {
            GameOverPanel.instance.GetComponent<AudioSource>().Stop();
        }
        if (MenuMusic.instance != null)
        {
            MenuMusic.instance.GetComponent<AudioSource>().Stop();
        }
        SceneManager.LoadScene(6);
    }

    public void LoadInstructions()
    {
        // StartCoroutine(Transition(4));
        if (GameOverPanel.instance != null)
        {
            GameOverPanel.instance.GetComponent<AudioSource>().Stop();
        }
        if (WinPanel.instance != null)
        {
            WinPanel.instance.GetComponent<AudioSource>().Stop();
        }
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.weapons.Clear();

        }

        SceneManager.LoadScene(4);
    }

    public void LoadStory()
    {
        if (GameOverPanel.instance != null)
        {
            GameOverPanel.instance.GetComponent<AudioSource>().Stop();
        }
        if (WinPanel.instance != null)
        {
            WinPanel.instance.GetComponent<AudioSource>().Stop();
        }
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.weapons.Clear();

        }
        SceneManager.LoadScene(5);
    }


}
