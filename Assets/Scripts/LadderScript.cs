using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LadderScript : MonoBehaviour
{
    [SerializeField]
    private int nextLevel = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager.Instance.level = nextLevel;
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
