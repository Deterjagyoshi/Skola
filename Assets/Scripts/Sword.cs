using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool swrodable1 = false;
    BoxCollider2D boxCol;
    public bool isSword1 = false;
    public bool isSword2 = false;
    private Rigidbody2D rb;
    private float timer = 0f;
    SpriteRenderer sprite;
    [SerializeField] LayerMask jumpableGround;

    public float distanceToGround;

    
    private bool IsGrounded(){
        return Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, 1f, jumpableGround);}
    // Start is called before the first frame update
     private Vector2 boxSize1 = new (0.5f, 3.5f);

     private Vector2 boxSize2 = new (0.5f, 3.5f);

     private Vector2 boxSize3 = new (3f,2.5f);

     
    void OnDrawGizmos()    
    {
        Gizmos.color = Color.red;

        Vector3 offsetvector1 = new Vector3(0,2.2f, 0); 
        Gizmos.DrawWireCube(transform.position + offsetvector1, boxSize1);

        Vector3 offsetvector2 = new Vector3(0,-2.2f, 0); 
        Gizmos.DrawWireCube(transform.position + offsetvector2, boxSize1);

        Vector3 offsetvector3 = new Vector3(0,-2.2f, 0); 
        Gizmos.DrawWireCube(transform.position + offsetvector3, boxSize2); 

        Vector3 offsetvector4 = new Vector3(1.5f,0.2f,0);
        Gizmos.DrawWireCube(transform.position + offsetvector4, boxSize3);

        Vector3 offsetvector5 = new Vector3(-1.5f,0.2f,0);
        Gizmos.DrawWireCube(transform.position + offsetvector5, boxSize3);

        
        
    }
    private bool IsTouchGround(){
        Vector3 offsetvector2 = new Vector3(0,-2.2f, 0); 
        
        return Physics2D.BoxCast(boxCol.bounds.center, boxSize1, 0f, offsetvector2, 2f, jumpableGround);}
    void Start()
    {
        
        Renderer renderer = GetComponent<Renderer>();   
        sprite = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }
    
    void swordupdown()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            isSword2 = true;
            sprite.flipY = false;
            isSword1 = false;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<SpriteRenderer>().enabled = true;
            isSword1 = true;
            sprite.flipY = true;
            isSword2 = false;
            
        }
        else 
        {
            isSword1 = false;
            isSword2 = false;
            
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            timer = 1f;
        }
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if(distanceToGround < 4)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        swrodable1 = IsTouchGround();
        swordupdown();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, jumpableGround);
        if (hit.collider != null)
        {
            // Measure the distance
            distanceToGround = hit.distance;
            
        }

    }
}
