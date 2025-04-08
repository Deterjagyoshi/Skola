using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Playermovement_2 : MonoBehaviour
{
    BoxCollider2D boxCol;
    [Header("Jumpable ground")]
    [SerializeField] LayerMask jumpableGround;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float doubleJumpHeight = 5f;
    private MovementStates state = MovementStates.idle;
    private Rigidbody2D rb;
    private bool hangon = false;
    private bool reel = false;
    Animator anim;
    [SerializeField] Sword sword;
    public float distanceToGround;
    [SerializeField] private TextMeshProUGUI HP;

    public bool Bleed = false;
    

    public int HitPoints = 100; 

    
    public GameObject HITRIGHT;
    public GameObject HITLEFT; 
    public GameObject HITDOWN;

    private float Timer = 0f;
    private float Timer2 = 0f;

     


    
    SpriteRenderer sprite;
    enum MovementStates {idle,running,jumping,falling,attack1,attack2};
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        DisableCollider(HITLEFT);
        DisableCollider(HITRIGHT);
        DisableCollider(HITDOWN);

    }

    public void EnableCollider(GameObject obj)
    {
        BoxCollider2D collider = obj.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
        else
        {
            Debug.LogWarning("No BoxCollider2D found on the target object.");
        }
    }

    // Disable the BoxCollider2D
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

    private bool IsGrounded(){
        return Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);}
    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if( horizontalInput > 0.001)
        {
            sprite.flipX = false;
        }
        else if( horizontalInput < -0.001)
        {
            sprite.flipX = true;
        }
        
         
        if(!reel){
        if(hangon){
            rb.velocity =  new Vector2(0,0);
            
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed,rb.velocity.y);
        }}
        
        
    }
    void Fling()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) && (distanceToGround < 4))
        {
            transform.Translate(Vector3.up * (4 - distanceToGround));
            rb.velocity = new Vector2(rb.velocity.x,10f);
        }

        //if((sword.isSword1 && sword.swrodable1)){
        //    rb.AddForce(new Vector2(0,0.2f),ForceMode2D.Impulse);
        //}
        
    }
    void Jump()
    {
        rb.AddForce(new Vector2(0,jumpHeight),ForceMode2D.Impulse);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        Attack();
        Fling();
        Move();
        UpdateAnimationState();
        
        HP.text = "" + HitPoints;
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        
        
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
            Jump();}
        
        
        anim.SetInteger("state",(int)state);
        
        if(IsGrounded()&&(rb.velocity.y == 0)){
            reel = false;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, jumpableGround);
        if (hit.collider != null)
        {
            // Measure the distance
            distanceToGround = hit.distance;
            
        }
         if(HitPoints <= 0){
            Die();
         }
        
        
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Spike")){
            rb.velocity = new Vector2(rb.velocity.x,10f);
            Bleed = true;
            HitPoints -= 20;
        }
        else {
            Bleed = false;
        }
        if(collision.gameObject.CompareTag("Boom")){
            rb.velocity = new Vector2(rb.velocity.x,10f);
            reel = true;
            
        }
    }

    bool IsAnimationPlaying(string animName)
    {
        // Get the current animation state information
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        
        // Check if the animation is playing and matches the given name
        return stateInfo.IsName(animName) && anim.GetCurrentAnimatorClipInfo(0).Length > 0;
    }

    void Attack()
    {
        if(sprite.flipX)
        {
            if(IsAnimationPlaying("Attack2")||IsAnimationPlaying("Attack1"))
            {
                EnableCollider(HITLEFT);
            }
            else
            {
                DisableCollider(HITLEFT);
            }
        }
        else
        {
            if(IsAnimationPlaying("Attack2")||IsAnimationPlaying("Attack1"))
            {
                EnableCollider(HITRIGHT);
            }
            else
            {
                DisableCollider(HITRIGHT);
            }
        }
        Timer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            EnableCollider(HITDOWN);
            Timer = 1f;
        }
        if(Timer < 0){
            DisableCollider(HITDOWN);
        }    
    }
    void Swing_on()
    {
        if(sprite.flipX){
            EnableCollider(HITLEFT);
        }
        else{
            EnableCollider(HITRIGHT);
        }
        Debug.Log("Swingon");
    }
    void Swing_off()
    {
        DisableCollider(HITLEFT);
        DisableCollider(HITRIGHT);
        Debug.Log("Swingoff");
    }
    
     void UpdateAnimationState()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if(rb.velocity.x > 0)
        {
            state = MovementStates.running;
            
        }
        else if(rb.velocity.x < 0)
        {
            state = MovementStates.running;
            
        }
        else
        {
            state = MovementStates.idle;
            
        }
        if((rb.velocity.y < -0.1))
        {
            state = MovementStates.falling;
        }
        
        if(rb.velocity.y > 0.1)
        {
            state = MovementStates.jumping;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(IsGrounded())
            {
                state = MovementStates.attack1;
                hangon = true;
                
                
                
            }
            else{
                state = MovementStates.attack2;
                
            }
            
        }
        else{
            hangon = false;
        }
            
        
    }
    public void unhang()
    {
        hangon = false;
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HitPoints = 100;
    }
    
    
    
        
        
}
