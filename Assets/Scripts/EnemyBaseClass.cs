using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBaseClass : MonoBehaviour
{

    public GameObject enemy;


    [SerializeField]
    protected Conductor conductor;

    protected int lastPositionInBeats;

    [SerializeField]
    protected int dmg = 1;

    public int health = 1;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private GameObject _liveImage;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _liveImage.GetComponent<SpriteRenderer>().sprite = _liveSprites[0];
            Destroy(enemy);
            return;
        }
        _liveImage.GetComponent<SpriteRenderer>().sprite = _liveSprites[health];

    }

    public bool BeatChanged()
    {
        if (lastPositionInBeats == conductor.songPositionInBeats)
            return false;
        lastPositionInBeats = conductor.songPositionInBeats;
        return true;
    }


    void Start()
    {
        lastPositionInBeats = conductor.songPositionInBeats;
    }

}
