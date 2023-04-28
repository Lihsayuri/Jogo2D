using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBox : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject infoBox;


    void Start()
    {
        infoBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Entered 1");
        if (other.gameObject.CompareTag("Player"))
            Debug.Log("Entered");
            infoBox.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Exited");
        infoBox.SetActive(false);
    }

}
