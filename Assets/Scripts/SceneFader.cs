using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Animator transition;
    private bool isTransitioning = false;

    public void LoadScreen(int levelIndex)
    {
        if (!isTransitioning)
        {
            StartCoroutine(Transition(levelIndex));
        }
    }

    IEnumerator Transition(int levelIndex)
    {
        isTransitioning = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelIndex);
        isTransitioning = false;
        this.gameObject.SetActive(false);
    }
}
