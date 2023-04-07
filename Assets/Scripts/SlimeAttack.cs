using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeAttack : MonoBehaviour
{
    public GameObject slime;

    [SerializeField]
    private Sprite [] _liveSprites;

    [SerializeField]
    private GameObject _liveImage;
    public void TakeDamageSlime(int damage)
    {
        _liveImage.GetComponent<SpriteRenderer>().sprite = _liveSprites[0];
        Destroy(slime);
        return;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
