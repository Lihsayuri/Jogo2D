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


    private bool CanMove(Vector2 direction)
    {
        Vector3 newPosition = oogaboonga.transform.position + (Vector3)direction;
        Vector2 boxSize = oogaboonga.GetComponent<BoxCollider2D>().size;

        // Verifica se há algum objeto com BoxCollider2D na próxima posição
        Collider2D hit = Physics2D.OverlapBox(newPosition, boxSize, 0f);

        if (hit != null && (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Player")))
        {
            return false;
        }

        return true;
    }

    public void moveRight(GameObject oogaboonga)
    {
        // Pega a posição atual do oogaboonga
        Vector3 currentPosition = oogaboonga.transform.position;

        // Calcula a próxima posição do oogaboonga para a direita
        Vector3 nextPosition = currentPosition + new Vector3(2, 0, 0);

        if (CanMove(nextPosition - currentPosition)){
            oogaboonga.transform.position = nextPosition;
            movedRight= true;
        }
    }

    // Move o oogaboonga uma unidade para a esquerda
    public void moveLeft(GameObject oogaboonga)
    {
        // Pega a posição atual do oogaboonga
        Vector3 currentPosition = oogaboonga.transform.position;

        // Calcula a próxima posição do oogaboonga para a esquerda
        Vector3 nextPosition = currentPosition + new Vector3(-2, 0, 0);

        if (CanMove(nextPosition - currentPosition)){
            oogaboonga.transform.position = nextPosition;
            movedRight = false;
        } 
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