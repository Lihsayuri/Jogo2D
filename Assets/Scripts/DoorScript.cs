using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject door;
    public bool locked = false;
    public void OpenDoor()
    {
        if (!locked)
        {
            door.SetActive(false);
        }
    }
}