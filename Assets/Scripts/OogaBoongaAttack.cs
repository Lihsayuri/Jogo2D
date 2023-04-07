using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OogaBoongaAttack : MonoBehaviour
{
    public GameObject oogaboonga;

    [SerializeField]
    private Conductor conductor;

    private int lastPositionInBeats;

    private bool movedRight = false;


    public void moveRight(GameObject esqueleto)
    {
        // Pega a posição atual do esqueleto
        Vector3 currentPosition = esqueleto.transform.position;

        // Calcula a próxima posição do esqueleto para a direita
        Vector3 nextPosition = currentPosition + new Vector3(2, 0, 0);


        esqueleto.transform.position = nextPosition;

        movedRight= true;
        
    }

    // Move o esqueleto uma unidade para a esquerda
    public void moveLeft(GameObject esqueleto)
    {
        // Pega a posição atual do esqueleto
        Vector3 currentPosition = esqueleto.transform.position;

        // Calcula a próxima posição do esqueleto para a esquerda
        Vector3 nextPosition = currentPosition + new Vector3(-2, 0, 0);

   
        esqueleto.transform.position = nextPosition;

        movedRight = false;
        
    }
 
    public bool BeatChanged(){
        if (lastPositionInBeats == conductor.songPositionInBeats)
            return false;
        lastPositionInBeats = conductor.songPositionInBeats;
        return true;
    }        


    void Start()
    {
        lastPositionInBeats = conductor.songPositionInBeats;
    }

    void Update()
    {
        //Debug.Log(playerPosition);
        // InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
        // if (conductor.BeatChanged())
        if (BeatChanged())
        {
            if (movedRight)
                moveLeft(oogaboonga);
            else
                moveRight(oogaboonga);
        }
    }
}
