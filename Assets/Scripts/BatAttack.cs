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

    public void moveUp(GameObject morcego)
    {
        // Pega a posição atual do morcego
        Vector3 currentPosition = morcego.transform.position;

        // Calcula a próxima posição do morcego para frente
        Vector3 nextPosition = currentPosition + new Vector3(0, 1, 0);

        morcego.transform.position = nextPosition;

        movedUp = true;
    }

    // Move o morcego uma unidade para trás
    public void moveDown(GameObject morcego)
    {
        // Pega a posição atual do morcego
    Vector3 currentPosition = morcego.transform.position;

        // Calcula a próxima posição do morcego para trás
    Vector3 nextPosition = currentPosition + new Vector3(0, -1, 0);

    morcego.transform.position = nextPosition;

    movedUp = false;


    }
    // Start is called before the first frame update
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
            if (movedUp)
                moveDown(morcego);
            else
                moveUp(morcego);
        }
    }
}
