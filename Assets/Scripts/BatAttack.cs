using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BatAttack : EnemyMoveClass
{

    private bool movedUp = false;

    void Update()
    {

        if (BeatChanged() && (conductor.songPositionInBeats % beatsPerMove == 0))
        {
            if (movedUp)
            {
                movedUp = false;
                move(enemy, Vector3.down);
            }
            else
            {
                movedUp = true;
                move(enemy, Vector3.up);
            }
        }


    }
}
