using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;

    public int level = 1;

    public bool trocaCena = false;

    public List<string> weapons = new List<string> ();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if ((SceneManager.GetActiveScene().name == "FirstLevel"))
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
