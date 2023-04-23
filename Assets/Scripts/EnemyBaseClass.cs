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

    public bool ganhou = false;

    public Animator animator;

    private bool isDying = false;

    public float animationTimeDead;

    IEnumerator WaitForAnimation()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDying", true);
        yield return new WaitForSeconds(animationTimeDead);
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        // Verifica se o dano recebido é maior que zero e se a animação não está sendo executada
 
        if (health <= 0)
        {
            _liveImage.GetComponent<SpriteRenderer>().sprite = _liveSprites[0];
            StartCoroutine(WaitForAnimation());
            if (gameObject.tag == "Boss")
            {
                ganhou = true;
            }
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
        animator.SetBool("isWalking", true);
        lastPositionInBeats = conductor.songPositionInBeats;
    }

}
