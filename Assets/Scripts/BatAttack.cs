using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BatAttack : MonoBehaviour
{
    public GameObject morcego;

    [SerializeField]
    private Conductor conductor;

    [SerializeField]
    private Sprite [] _liveSprites;

    [SerializeField]
    private GameObject _liveImage;

    private int lastPositionInBeats;

    private bool movedUp = false;


    public void TakeDamageBat(int damage)
    {
        Debug.Log("Bat took damage");
       _liveImage.GetComponent<SpriteRenderer>().sprite = _liveSprites[0];
        Destroy(morcego);
        return;

    }


    private bool CanMove(Vector2 direction)
    {
        Vector3 newPosition = morcego.transform.position + (Vector3)direction;
        Vector2 boxSize = morcego.GetComponent<BoxCollider2D>().size;

        // Verifica se há algum objeto com BoxCollider2D na próxima posição
        Collider2D hit = Physics2D.OverlapBox(newPosition, boxSize, 0f);

        if (hit != null && (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Player")))
        {
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

        if (BeatChanged())
        {
            if (movedUp)
                moveDown(morcego);
            else
                moveUp(morcego);
        }


    }
}
