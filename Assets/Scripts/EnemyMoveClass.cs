using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMoveClass : EnemyBaseClass
{
    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap wallTilemap;

    [SerializeField]
    protected int beatsPerMove = 2;



    protected bool CanMove(Vector2 direction)
    {
        Vector3 newPosition = enemy.transform.position + (Vector3)direction;
        Vector2 boxSize = enemy.GetComponent<BoxCollider2D>().size;

        // Verifica se h� algum objeto com BoxCollider2D na pr�xima posi��o
        Collider2D hit = Physics2D.OverlapBox(newPosition, boxSize, 0f);

        if (hit != null && (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Boss") || hit.gameObject.CompareTag("Player") || hit.gameObject.CompareTag("Door")))
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                PlayerController playerScript = (PlayerController)hit.gameObject.GetComponent(typeof(PlayerController));

                playerScript.TakeDamage(dmg);
                animator.SetBool("isAttacking", true);
                return false;
            }

            return false;
        } else
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDying", false);
            animator.SetBool("isWalking", true);
        }


        Vector3Int gridPosition = groundTilemap.WorldToCell(newPosition);
        if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
            return false;



        return true;
    }

    protected void move(GameObject enemy, Vector3 direction)
    {
        // Pega a posi��o atual do enemy
        Vector3 currentPosition = enemy.transform.position;

        // Calcula a pr�xima posi��o do enemy para a direita
        Vector3 nextPosition = currentPosition + direction;

        // Verifica se pode se mover para a direita sem bater em uma parede
        if (CanMove(nextPosition - currentPosition))
        {
            enemy.transform.position = nextPosition;
        }
    }
}
