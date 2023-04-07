using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KnightAttack : MonoBehaviour
{

    public GameObject knight;

    [SerializeField]
    private Conductor conductor;

    private Vector3 playerPosition;

    private int lastPositionInBeats;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap wallTilemap;

    public void moveRight(GameObject knight)
    {
        // Pega a posição atual do knight
        Vector3 currentPosition = knight.transform.position;

        // Calcula a próxima posição do knight para a direita
        Vector3 nextPosition = currentPosition + new Vector3(1, 0, 0);

        // Verifica se pode se mover para a direita sem bater em uma parede
        if (CanMove(nextPosition - currentPosition))
        {
            knight.transform.position = nextPosition;
        }
    }

    // Move o knight uma unidade para a esquerda
    public void moveLeft(GameObject knight)
    {
        // Pega a posição atual do knight
        Vector3 currentPosition = knight.transform.position;

        // Calcula a próxima posição do knight para a esquerda
        Vector3 nextPosition = currentPosition + new Vector3(-1, 0, 0);

        // Verifica se pode se mover para a esquerda sem bater em uma parede
        if (CanMove(nextPosition - currentPosition))
        {
            knight.transform.position = nextPosition;
        }
    }

    // Move o knight uma unidade para frente
    public void moveUp(GameObject knight)
    {
        // Pega a posição atual do knight
        Vector3 currentPosition = knight.transform.position;

        // Calcula a próxima posição do knight para frente
        Vector3 nextPosition = currentPosition + new Vector3(0, 1, 0);

        // Verifica se pode se mover para frente sem bater em uma parede
        if (CanMove(nextPosition - currentPosition))
        {
            knight.transform.position = nextPosition;
        }
    }

    // Move o knight uma unidade para trás
    public void moveDown(GameObject knight)
    {
        // Pega a posição atual do knight
        Vector3 currentPosition = knight.transform.position;

        // Calcula a próxima posição do knight para trás
        Vector3 nextPosition = currentPosition + new Vector3(0, -1, 0);

        // Verifica se pode se mover para trás sem bater em uma parede
        if (CanMove(nextPosition - currentPosition))
        {
            knight.transform.position = nextPosition;
        }
    }


    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(knight.transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
            return false;
        return true;
    }

    void MoveTowardsPlayer()
    {
        Vector3 skeletonPosition = knight.transform.position;
        skeletonPosition.y -= 0.25f;
        Vector3 playerDirection = (playerPosition - skeletonPosition).normalized;
        Vector3 closestVector = Vector3.right;
        float smallestAngle = Mathf.Abs(Vector3.Angle(playerDirection, closestVector));

        if (playerDirection.x == 0 && playerDirection.y == 0)
        {
            return;
        }

        else{
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

            if (closestVector == Vector3.right)
            {
                moveRight(knight);
            }
            else if (closestVector == Vector3.up)
            {
                moveUp(knight);
            }
            else if (closestVector == Vector3.left)
            {
                moveLeft(knight);
            }
            else if (closestVector == Vector3.down)
            {
                moveDown(knight);
            }

        }

    }


    private void UpdatePlayerPosition()
    {
        playerPosition = GameObject.Find("Player").transform.position;
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
        InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
        // Debug.Log(knight.transform.position);
    }

    void Update()
    {
        //Debug.Log(playerPosition);
        // InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
        // if (conductor.BeatChanged())
        if (BeatChanged())
        {
            MoveTowardsPlayer();
        }
    }
}


