using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Image fadeOutImage; // Imagem usada para fazer o fadeout
    public float fadeOutTime = 1.5f; // Tempo de duração do fadeout
    public GameObject player; // Referência ao jogador

    [SerializeField]
    private GameObject gameOverPanel;
    public GameObject conductor;

    public GameObject winPanel;
    public IEnumerator FadeOutCoroutine()
    {
        // Aguarda um pequeno intervalo de tempo para iniciar o fadeout
        yield return new WaitForSeconds(0.5f);

        // Define o valor inicial da opacidade da imagem de fadeout
        Color color = fadeOutImage.color;
        color.a = 0f;
        fadeOutImage.color = color;

        // Inicia o fadeout
        float time = 0f;
        while (time < fadeOutTime)
        {
            // Atualiza o valor da opacidade da imagem de fadeout
            color.a = Mathf.Lerp(0f, 1f, time / fadeOutTime);
            fadeOutImage.color = color;

            // Faz o jogador ficar visível durante o fadeout
            player.SetActive(true);

            time += Time.deltaTime;
            yield return null;
        }

        gameOverPanel.SetActive(true);
                    // confere se tem musica de fundo, se tiver, para de tocar
        Conductor conductorScript = conductor.GetComponent<Conductor>() as Conductor;
        conductorScript.musicSource.Stop();
        // Faz o jogador ficar invisível após o fadeout
        player.SetActive(false);
    }

    public IEnumerator FadeOutCoroutineWin()
    {
        // Aguarda um pequeno intervalo de tempo para iniciar o fadeout
        yield return new WaitForSeconds(0.5f);

        // Define o valor inicial da opacidade da imagem de fadeout
        Color color = fadeOutImage.color;
        color.a = 0f;
        fadeOutImage.color = color;

        // Inicia o fadeout
        float time = 0f;
        while (time < fadeOutTime)
        {
            // Atualiza o valor da opacidade da imagem de fadeout
            color.a = Mathf.Lerp(0f, 1f, time / fadeOutTime);
            fadeOutImage.color = color;

            // Faz o jogador ficar visível durante o fadeout
            player.SetActive(true);

            time += Time.deltaTime;
            yield return null;
        }

        winPanel.SetActive(true);
                    // confere se tem musica de fundo, se tiver, para de tocar
        Conductor conductorScript = conductor.GetComponent<Conductor>() as Conductor;
        conductorScript.musicSource.Stop();
        // Faz o jogador ficar invisível após o fadeout
        player.SetActive(false);
    }
}