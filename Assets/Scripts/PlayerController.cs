using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerMovement controls;
    [SerializeField]
    private Conductor conductor;

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
    }

    private void Move(Vector2 direction)
    {
        if (conductor.seconds_off_beat() < beat_detection_range) {
            transform.position += (Vector3)direction;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
