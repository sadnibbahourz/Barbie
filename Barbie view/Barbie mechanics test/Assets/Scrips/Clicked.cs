using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Notes
/*
 * Use rigidbody.addforce with quantinion instead of transform.position. So that the player can throw objects and so the objects don't clip into walls
 * Make so that the player release objects when die
 * Fix object can lift player
 */
public class Clicked : MonoBehaviour
{
    #region Refrences
    [Header("Refrences")]
    public Rigidbody2D rb;
    public Animator animator;
    public Transform respawnPos;
    public GameObject player;
    public LineRenderer lr;
    #endregion
    #region Variables
    [Header("Variables")]
    [Tooltip("The heigt the object will respawn")] public float MinHeight = -6;
    [Tooltip("How fast the object moves to the mouse")][Range(0,1)] public float smoothSpeed = 0.1f;
    Vector3 Smoothedpos;
    Vector3 reletiveMousePos;
    bool clicked = false;
    #endregion

    private void Start()
    {
        Respawn();
        lr.enabled = false;
    }
    private void OnMouseDown()
    {
        clicked = !clicked;
       
    }

    private void FixedUpdate()
    {
        SmoothMovement();
        Click();
        Sprites();
        MinHeigt();
    }

    void SmoothMovement()
    {
        reletiveMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 1));
        if (clicked)
        {
            Smoothedpos = Vector3.Lerp(transform.position, reletiveMousePos, smoothSpeed);
            transform.position = Smoothedpos;
        }
    }
    void Click()
    {
        if (clicked)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            //RenderLine();
        }
        else
        {
            clicked = false;
            rb.gravityScale = 1;
            lr.enabled = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            clicked = false;
            rb.gravityScale = 1;
        }
    }
    void Sprites()
    {
        if (clicked)
        {
            animator.SetBool("glow", true);
        } else
        {
            animator.SetBool("glow", false);
        }
    }
    void Respawn()
    {
        transform.position = respawnPos.position;
        transform.rotation = respawnPos.rotation;
        rb.velocity = new Vector2(0, 0);
    }
    void MinHeigt()
    {
        if (transform.position.y <= MinHeight)
        {
            Respawn();
        }
    }
    void RenderLine()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, player.transform.position);
        lr.enabled = true;
    }
}
