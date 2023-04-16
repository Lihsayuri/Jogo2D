using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OogaBoongaAttack : EnemyMoveClass
{
    private bool movedRight = false;

    void Update()
    {
        if (BeatChanged() && (conductor.songPositionInBeats % beatsPerMove == 0))
        {
            if (movedRight)
            {
                movedRight = false;
                Vector3 nextPosition = new Vector3(-2, 0, 0);
                move(enemy, nextPosition);
            }
            else
            {
                movedRight = true;
                Vector3 nextPosition = new Vector3(2, 0, 0);
                move(enemy, nextPosition);
            }
        }
    }
}
