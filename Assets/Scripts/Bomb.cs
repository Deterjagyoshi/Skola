using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float expansionSpeed = 5f;  // How fast the collider expands
    public float maxRadius = 5f;
    public bool boom = false;
    BoxCollider2D boxCol;
    Animator anim;
    private Rigidbody2D rb;

    public float flingForce = 10f;
    private MovementStates state = MovementStates.idle;
    enum MovementStates {idle,boom};
    private void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("Ja");
        if(collision.gameObject.CompareTag("Sword")||collision.gameObject.CompareTag("Sword2")||collision.gameObject.CompareTag("Boom")||collision.gameObject.CompareTag("Spike")||collision.gameObject.CompareTag("SwordDOwn")){
            

           Transform parentTransform = collision.transform.parent;

        if (parentTransform != null)
        {
            Rigidbody2D parentRb = parentTransform.GetComponent<Rigidbody2D>();

            if (parentRb != null)
                {
                // Direction from this object to the parent
                Vector2 direction = (parentRb.position - (Vector2)transform.position).normalized;

                // Apply force to the parent object
                parentRb.AddForce(direction * flingForce, ForceMode2D.Impulse);
                }
            
        }
        
            boom = true;
            state = MovementStates.boom;
            Debug.Log("Boom");}
            rb.simulated = false;
    }
    // Start is called before the first frame update
    private CircleCollider2D circleCollider;

    // hej

    void Start()
    {
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        boxCol = GetComponent<BoxCollider2D>();
        circleCollider.radius = 0.1f;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (circleCollider.radius < maxRadius && boom)
        {
            
            circleCollider.radius += expansionSpeed * Time.deltaTime;
            anim.SetBool("boom", boom);
        }
    }
    public void KYS()
    {
        Destroy(this.gameObject);
    }
}
