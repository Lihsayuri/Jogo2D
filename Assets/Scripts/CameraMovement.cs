using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null) {
            transform.position = player.transform.position + offset;
        }
    }
}
