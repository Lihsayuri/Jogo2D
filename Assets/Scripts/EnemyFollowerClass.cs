using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowerClass : EnemyMoveClass
{

    public GameObject player;

    private Vector3 playerPosition;

    void MoveTowardsPlayer()
    {
        Vector3 enemyPosition = enemy.transform.position;
        enemyPosition.y -= 0.25f;
        Vector3 playerDirection = (playerPosition - enemyPosition).normalized;
        Vector3 closestVector = Vector3.right;
        float smallestAngle = Mathf.Abs(Vector3.Angle(playerDirection, closestVector));



        if (smallestAngle > Mathf.Abs(Vector3.Angle(playerDirection, Vector3.up)))
        {
            smallestAngle = Mathf.Abs(Vector3.Angle(playerDirection, Vector3.up));
            closestVector = Vector3.up;
        }

        if (smallestAngle > Mathf.Abs(Vector3.Angle(playerDirection, Vector3.left)))
        {
            smallestAngle = Mathf.Abs(Vector3.Angle(playerDirection, Vector3.left));
            closestVector = Vector3.left;
        }

        if (smallestAngle > Mathf.Abs(Vector3.Angle(playerDirection, Vector3.down)))
        {
            closestVector = Vector3.down;
        }

        move(enemy, closestVector);



    }

    private void UpdatePlayerPosition()
    {
        playerPosition = player.transform.position;
    }

        void Start()
    {
        lastPositionInBeats = conductor.songPositionInBeats;
        InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
    }

    void Update()
    {
        if (BeatChanged() && (conductor.songPositionInBeats % beatsPerMove == 0))
        {
            MoveTowardsPlayer();
        }
    }
}
