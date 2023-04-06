using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnemies : MonoBehaviour
{
    public Animator animator; // Referência ao componente Animator
    public GameObject conductor; // Referência ao objeto condutor

    [SerializeField]
    private float multiplier = 10f; // Multiplicador de velocidade

    void Update()
    {
        float animationTime = conductor.GetComponent<Conductor>().secPerBeat*multiplier; // Obtenha o valor atual da variável do objeto condutor
        animator.speed = 1f / animationTime; // Defina a velocidade da animação como o inverso do tempo de animação
    }
}


