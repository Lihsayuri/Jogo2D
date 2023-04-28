using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInFadeOut : MonoBehaviour
{
    public static FadeInFadeOut instance;

    [SerializeField]
    private GameObject fadeCanvas;
    [SerializeField]
    private Animator fadeAnim;

    private void Awake()
    {
        MakeSingleton();
    }

    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void FadeIn(string levelName)
    {
        StartCoroutine(FadeInAnimation(levelName));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutAnimation());
    }

    private IEnumerator FadeInAnimation(string levelName)
    {
        fadeCanvas.SetActive(true);
        fadeAnim.Play("FadeIn");
        yield return new WaitForSeconds(fadeAnim.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(levelName);
    }

    private IEnumerator FadeOutAnimation()
    {
        fadeCanvas.SetActive(true);
        fadeAnim.Play("FadeOut");
        yield return new WaitForSeconds(fadeAnim.GetCurrentAnimatorStateInfo(0).length);
        fadeCanvas.SetActive(false);
    }
}
