using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SkeletonNamespace;

public class PlayerController : MonoBehaviour
{

    private PlayerMovement controls;

    [SerializeField]
    private Tilemap groundTilemap;

    [SerializeField]
    private Tilemap wallTilemap;
    
    [SerializeField]
    private Conductor conductor;

    private int raycast_mask;

    public float beat_detection_range = 0.15f;


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
        raycast_mask = LayerMask.GetMask("Enemies");
    }

    private void Move(Vector2 direction)
    {
        if (conductor.seconds_off_beat() < beat_detection_range) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, raycast_mask);
            Debug.Log(hit.collider);
            Debug.Log(raycast_mask);
            if (hit.collider == null)
            {
                if (CanMove(direction))
                    transform.position += (Vector3)direction;
            }
            else
            {
                Debug.Log(hit.collider.gameObject);
                hit.collider.gameObject.SetActive(false);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("hey");
        // ..and if the GameObject you intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
            return false;
        return true;
    }
}
