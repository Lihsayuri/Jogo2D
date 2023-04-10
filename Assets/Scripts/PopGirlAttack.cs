  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PopGirlAttack : MonoBehaviour
{

    public GameObject popGirl;

    public GameObject player;

    [SerializeField]
    private Conductor conductor;

    private Vector3 playerPosition;

    private int lastPositionInBeats;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap wallTilemap;

    [SerializeField]
    private int dmg = 3;

    [SerializeField]
    private int beatsPerMove = 2;

    public int vida_popGirl = 4;

    [SerializeField]
    private Sprite [] _liveSprites;

    [SerializeField]
    private GameObject _liveImage;


    public void TakeDamagePopGirl(int damage)
    {
        Debug.Log("ENTREI NO TAKE DAMAGE popGirl");
        vida_popGirl -= damage;
        Debug.Log("Vida popGirl: " + vida_popGirl);
        if (vida_popGirl <= 0) {
            Debug.Log("Morreu");
            _liveImage.GetComponent<SpriteRenderer>().sprite = _liveSprites[0];
            Destroy(popGirl);
            return;
        }

        _liveImage.GetComponent<SpriteRenderer>().sprite = _liveSprites[vida_popGirl];
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3 newPosition = popGirl.transform.position + (Vector3)direction;
        Vector2 boxSize = popGirl.GetComponent<BoxCollider2D>().size;

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

        Vector3Int gridPosition = groundTilemap.WorldToCell(newPosition);
        if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
            return false;
        return true;
    }

    private void move(GameObject esqueleto, Vector3 direction)
    {
        // Pega a posição atual do esqueleto
        Vector3 currentPosition = esqueleto.transform.position;

        // Calcula a próxima posição do esqueleto para a direita
        Vector3 nextPosition = currentPosition + direction;

        // Verifica se pode se mover para a direita sem bater em uma parede
        if (CanMove(nextPosition - currentPosition))
        {
            esqueleto.transform.position = nextPosition;
        }
    }


    void MoveTowardsPlayer()
    {
        Vector3 skeletonPosition = popGirl.transform.position;
        skeletonPosition.y -= 0.25f;
        Vector3 playerDirection = (playerPosition - skeletonPosition).normalized;
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

        move(popGirl, closestVector);

        

    }


    private void UpdatePlayerPosition()
    {
        playerPosition = player.transform.position;
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
        // Debug.Log(popGirl.transform.position);
    }

    void Update()
    {
        //Debug.Log(playerPosition);
        // InvokeRepeating("UpdatePlayerPosition", 0f, 1f); // atualiza a posição do player a cada 1 segundo
        // if (conductor.BeatChanged())
        if (BeatChanged() && (conductor.songPositionInBeats % beatsPerMove == 0))
        {
            MoveTowardsPlayer();
        }
    }
}


