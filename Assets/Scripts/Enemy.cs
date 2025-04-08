using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 3;
    [SerializeField] float knockback = 1;
    BoxCollider2D boxCol;
    Animator anim;
    SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool turn = false;
    private Vector2 boxSize3 = new (0.2f, 0.2f);
    private float offsetx3 = -0.5f;
    private float offsety3 = -0.2f;
    private Vector2 boxSize1 = new (0.9f, 0.2f);
    private float offsetx1 = 0.0f;
    private float offsety1 = -1.0f;
    private float horizontalInput;
    private bool reeling = false;
    

    private Vector2 boxSize2 = new (0.2f, 0.2f);
    private float offsetx2 = 0.5f;
    private float offsety2 = -0.2f;
    [SerializeField] LayerMask jumpableGround;
    public float distanceToGround;
    public bool distanceToRight;
    public bool distanceToLeft;

    private Vector2 leftvector = new Vector2(-1f,0);
    private Vector2 rightvector = new Vector2(1f,0);
    // Start is called before the first frame update
    public GameObject boxy;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
    }
    private bool IsGrounded(){
        return Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);}
    void OnDrawGizmos()    
    {
        Gizmos.color = Color.red;
        Vector3 offsetvector2 = new Vector3(offsetx2 + 0.1f, offsety2, 0f); 
        Gizmos.DrawWireCube(transform.position + offsetvector2, boxSize2);
        
        Vector3 offsetvector3 = new Vector3(offsetx3 - 0.1f, offsety3, 0f); 
        Gizmos.DrawWireCube(transform.position + offsetvector3, boxSize3);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Sword")){
            rb.velocity = new Vector2(-10f*knockback,5f*knockback);
            reeling = true;
            rb.AddTorque(5f*knockback);
            health -= 1;}
        if(collision.gameObject.CompareTag("Sword2")){
            rb.velocity = new Vector2(10f*knockback,5f*knockback);
            reeling = true;
            rb.AddTorque(-5f*knockback);
            health -= 1;}
        if(collision.gameObject.CompareTag("SwordDOwn")){
            health -= 3;}
        if(collision.gameObject.CompareTag("Boom")){
            health -= 3;
        }}
    private void Move(){
        if(!reeling){
        if(turn != true){
            rb.velocity = new Vector2(2,rb.velocity.y);}
        else{
            rb.velocity = new Vector2(-2f,rb.velocity.y);}}}

    private void turnaround(){
        if(distanceToRight){
            turn = true;
            sprite.flipX = true;}
        else if(distanceToLeft){
            turn = false;
            sprite.flipX = false;}}
    public void DisableCollider( GameObject obj)
    {
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("No BoxCollider2D found on the target object.");
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
        distanceToRight = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.right, 0.1f, jumpableGround);
        distanceToLeft = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.left, 0.1f, jumpableGround);

        
        turnaround();
        Move();
        if(IsGrounded() && rb.velocity.y == 0){
            reeling = false;
        }
        if(health <= 0){
            Destroy(this);
            GetComponent<SpriteRenderer>().enabled = false;
            DisableCollider(boxy);
            GetComponent<BoxCollider2D>().enabled = false;
            
        }
    }
}
