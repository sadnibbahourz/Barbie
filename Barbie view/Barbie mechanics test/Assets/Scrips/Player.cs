using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
//Notes
/*
 * Use ray to jump instead of collider ✔
 * Make changable telekinesis radius (Do last)
 */

public class Player : MonoBehaviour
{
    #region Refrences
    [Header("Check Points")]
    [Header("Refrences")]
    public Transform startPos;
    public Transform checkPoint1;
    public Transform checkPoint2;
    [Space(20)]
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject throwable;
    public Transform throwpos;
    [Header("Masks")]
    public LayerMask groundMask = 1<<10;
    public LayerMask objecctMask = 1 << 9;
    #endregion
    #region Variables
    //Visible Variables
    [Header("Variables")]
    public string xInput = "Horizontal";
    [Tooltip("Scale of the player")] public float scale = 0.04f;
    [Tooltip("Should the player move with Rigidbody or translate")] public bool useRbMovement = false;
    [Range(0, 4000)] public float RbMovementSpeed = 1500;
    [Range(0, 1)] public float transMovementSpeed = 0.09f;
    [Tooltip("The heigt the player will die")] public float MinHeight = -6;
    [Tooltip("How heigh the player can jump")] public float jumpForce;
    [Header("raycast settings")]
    public Vector2 rayDirection;
    public float rayDistance;
    public float rayOffset = -0.64f;

    //Hidden Variables
    [HideInInspector]public bool isDead = false;
    private float xMovement;
    private bool faceingRight = true;
    private Rigidbody targetrb;
    private Vector2 addForce;
    private int checkpoint = 0;
    #endregion

    private void Start()
    {
        transform.position = startPos.position;
    }
    private void Update()
    {
        xMovement =Input.GetAxisRaw(xInput)*transMovementSpeed;
    }
    void FixedUpdate()
    {
        if (useRbMovement)
        {
            RbMovement();
        }
        else
        {
            TransMovement();
        }
        FaceingDirection();
        Flip();
        Animation();
        MinHeigt();
        //throw
        if (Input.GetButtonDown("Fire1"))
        {
            //Throw();

        }
        //Jump
        if (Input.GetButtonDown("Jump") && CanJump())
        {
            Jump();
        }
        Debug.Log(Checkpoint().name);
        Debug.Log(checkpoint);
    }
    #region Functions
    [System.Obsolete("I think TransMovement() is better to use")]
    void RbMovement()
    {
        //moves the player
        addForce = new Vector2(Input.GetAxisRaw(xInput) * RbMovementSpeed * Time.deltaTime, 0);
        rb.AddForce(addForce);

        // max speed
        if(rb.velocity.x > 5)
        {
            rb.velocity = new Vector3(5,rb.velocity.y,0);
        }else if (rb.velocity.x < -5)
        {
            rb.velocity = new Vector3(-5, rb.velocity.y, 0);
        }

        //jump

    }
    void TransMovement()
    {
        transform.Translate(new Vector2(xMovement,0));
    }
    void FaceingDirection()
    {
        //Is the player faceing right?
        if(Input.GetAxisRaw("Horizontal") == 1){
            faceingRight = true;
        }else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            faceingRight = false;
        }
    }
    void Flip()
    {
        //flips the player

        if (faceingRight)
        {
            transform.localScale = new Vector3(scale,scale,0);
        } else if (!faceingRight)
        {
            transform.localScale = new Vector3(-scale, scale, 0);
        }
    }
    void Animation()
    {
        if(xMovement == 0)
        {
            animator.SetBool("isWalking", false); 
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }
    void Throw()
    {
            Instantiate(throwable, throwpos.position, throwpos.rotation);
    }
    void Jump()
    {
        rb.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
    }
    bool CanJump()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 0.7f, groundMask | objecctMask);
    }
    void Die()
    {
        isDead = true;

        transform.position = Checkpoint().position;
        rb.velocity = new Vector2(0,0);
    }
    Transform Checkpoint()
    {
        
        if(transform.position.x > checkPoint2.position.x || checkpoint == 2)
        {
            checkpoint = 2;
            return checkPoint2;
        }else if (transform.position.x > checkPoint1.position.x && transform.position.x < checkPoint2.position.x || checkpoint == 1)
        {
            checkpoint = 1;
            return checkPoint1;
        }
        else if(checkpoint == 0)
        {
            return startPos;
        }else
        {
            return null;
        }
    }
    void MinHeigt()
    {
        if(transform.position.y <= MinHeight)
        {
            Die();
        }
    }
    #endregion
}
