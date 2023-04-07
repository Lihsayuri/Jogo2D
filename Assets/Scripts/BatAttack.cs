using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BatAttack : MonoBehaviour
{
    public GameObject morcego;

    [SerializeField]
    private Conductor conductor;

    private int lastPositionInBeats;

    private bool movedUp = false;

    [SerializeField]
    private int dmg = 1;

    [SerializeField]
    private int beatsPerMove = 2;


    private bool CanMove(Vector2 direction)
    {
        Vector3 newPosition = morcego.transform.position + (Vector3)direction;
        Vector2 boxSize = morcego.GetComponent<BoxCollider2D>().size;

        // Verifica se há algum objeto com BoxCollider2D na próxima posição
        Collider2D hit = Physics2D.OverlapBox(newPosition, boxSize, 0f);

        if (hit != null && (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Player")))
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                PlayerController playerScript = (PlayerController)hit.gameObject.GetComponent(typeof(PlayerController));
                playerScript.TakeDamage(dmg);
                return false;
            }
            return false;
        }

        return true;
    }

    public void moveUp(GameObject morcego)
    {
        // Pega a posição atual do morcego
        Vector3 currentPosition = morcego.transform.position;

        // Calcula a próxima posição do morcego para frente
        Vector3 nextPosition = currentPosition + new Vector3(0, 1, 0);

        if (CanMove(nextPosition - currentPosition)){
            morcego.transform.position = nextPosition;
            movedUp = true;
        }
    }

    public void moveDown(GameObject morcego)
    {
        Vector3 currentPosition = morcego.transform.position;

        Vector3 nextPosition = currentPosition + new Vector3(0, -1, 0);


        if (CanMove(nextPosition - currentPosition)){
            morcego.transform.position = nextPosition;
            movedUp = false;
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

        if (BeatChanged() && (conductor.songPositionInBeats % beatsPerMove == 0))
        {
            if (movedUp)
                moveDown(morcego);
            else
                moveUp(morcego);
        }
    }
}
