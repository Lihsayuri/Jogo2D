using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMenuMusic : MonoBehaviour
{
    private void Start()
    {
        MenuMusic menuMusic = FindObjectOfType<MenuMusic>();
        if (menuMusic != null)
        {
            Destroy(menuMusic.gameObject);
        }
    }
}