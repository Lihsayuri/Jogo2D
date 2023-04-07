using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameObject player;

    private PlayerMovement controls;

    [SerializeField]
    private Sprite [] _liveSprites;

    [SerializeField]
    private Image _liveImage;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap wallTilemap;
    
    [SerializeField]
    private Conductor conductor;

    private int raycast_mask;

    public float beat_detection_range = 0.15f;

    public int vida = 5;


    private void Awake()
    {
        controls = new PlayerMovement();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        controls.Main.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
        if (conductor.seconds_off_beat() < beat_detection_range) {

            if (CanMove(direction))
                transform.position += (Vector3)direction;

        }

    }

    public void TakeDamage(int damage)
    {
        vida -= damage;
        if(vida > 0)
            _liveImage.sprite = _liveSprites[vida];
        else
        {
            gameObject.SetActive(false);
        }
    }


    private bool CanMove(Vector2 direction)
    {
        Vector3 newPosition = player.transform.position + (Vector3)direction;
        Vector2 boxSize = player.GetComponent<BoxCollider2D>().size;

        // Verifica se há algum objeto com BoxCollider2D na próxima posição
        Collider2D hit = Physics2D.OverlapBox(newPosition, boxSize, 0f);

        if (hit != null && (hit.gameObject.CompareTag("Enemy")))
        {
            Debug.Log("ENTREI NO HIT DO CAN MOVE");
            BatAttack batScriptConfere = hit.gameObject.GetComponent<BatAttack>() as BatAttack;
            if (batScriptConfere != null)
            {
                batScriptConfere.TakeDamageBat(1);
                return false;
            }
            
            SlimeAttack slimeScript = hit.gameObject.GetComponent<SlimeAttack>() as SlimeAttack;
            if (slimeScript != null){
                slimeScript.TakeDamageSlime(1);
                return false;
            }
            OogaBoongaAttack oogaScript = hit.gameObject.GetComponent<OogaBoongaAttack>() as OogaBoongaAttack;
            if (oogaScript != null){
                oogaScript.TakeDamageOoga(1);
                return false;
            }

            return false;
        }

        Vector3Int gridPosition = groundTilemap.WorldToCell(newPosition);
        if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
            return false;
        return true;

    }
}
