// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class SpecialGauge : MonoBehaviour
// {
//     public GameObject player; // Referência ao objeto que você deseja monitorar para detectar quando ele está no ritmo.
//     public float specialAmount; // Quantidade atual de especial do personagem.
//     public float specialIncrease; // Quantidade de especial adicionada a cada batida.
//     public float maxSpecial; // Quantidade máxima de especial que o personagem pode ter.
//     public Image specialBar; // Referência à imagem da gauge de especial.

//     private void Start()
//     {
//         specialBar.fillAmount = 0; // Inicia a barra de especial com zero.
//     }

//     private void Update()
//     {
//         // Atualiza a barra de especial com base na quantidade de especial atual do personagem.
//         specialBar.fillAmount = specialAmount / maxSpecial;
//     }

//     // Função chamada a cada batida da música.
//     public void Beat()
//     {
//         // Verifica se o personagem está no ritmo.
//         if (player.GetComponent<PlayerController>().onTime)
//         {
//             Debug.Log("Special increased!");
//             // Adiciona a quantidade de especial apropriada à variável de especial do personagem.
//             specialAmount += specialIncrease;

//             // Verifica se a quantidade de especial do personagem é maior que a quantidade máxima permitida.
//             if (specialAmount > maxSpecial)
//             {
//                 // Define a quantidade de especial do personagem como a quantidade máxima permitida.
//                 specialAmount = maxSpecial;
//             }
//         }
//     }
// }