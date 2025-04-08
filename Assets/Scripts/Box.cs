using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    [SerializeField] float HitPoints = 3;
    [SerializeField] float knockback = 1;

    BoxCollider2D boxCol;
    Animator anim;
    SpriteRenderer sprite;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Sword")){
            rb.velocity = new Vector2(-10f*knockback,5f*knockback);
            rb.AddTorque(5f*knockback);
            HitPoints -= 1;
        }
        if(collision.gameObject.CompareTag("Sword2")){
            rb.velocity = new Vector2(10f*knockback,5f*knockback);
            rb.AddTorque(-5f*knockback);
            HitPoints -= 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
