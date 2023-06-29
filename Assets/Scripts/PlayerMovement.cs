using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float dashSpeed = 100f;
    [SerializeField] float dashCooldownSeconds = 0.5f;
    [SerializeField] float attackSeconds = 0.5f;
    [SerializeField] float dashSecond = 0.5f;
    [SerializeField] Collider2D trampolineCollider;
    Rigidbody2D myRb;
    Vector2 moveInput;
    BoxCollider2D myBoxCollider;
    CompositeCollider2D GroundCollider;
    Animator PlayerAnimator;
    int directionForDash;
    float normalGravity;
    bool dashCooldown;
    bool dashCooldown2;
    bool isAttacking;
    int airDashCounter;
    bool isAttackPressed;
    int currentDirectionForDash;
    public int sceneNumberCameFromFile;
    bool isDirectionAvailable;


    bool isRunning;
    bool isJumping;
    public bool isAttackingAn;
    bool isDashingAn;
    public bool isDead;


    string currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        myRb= GetComponent<Rigidbody2D>();
        myBoxCollider= GetComponent<BoxCollider2D>();
        GroundCollider = FindAnyObjectByType<CompositeCollider2D>();
        PlayerAnimator = GetComponent<Animator>();
        directionForDash = 1;
        normalGravity = myRb.gravityScale;
        isDirectionAvailable = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!PlayerAnimator.GetBool("isDead"))
        {
            if (!dashCooldown)
            {
                if (!isAttacking)
                {
                    Run();

                    SpriteChangesInAction();
                }
            }
            
        }
        

    }

    private void LateUpdate()
    {
        AnimationHandling();
    }

    void SpriteChangesInAction()
    {
        // turning sprite according to direction
        if(myRb.velocity.x > Mathf.Epsilon && isDirectionAvailable)
        {
            transform.localScale = new Vector2(1,1);
            directionForDash = 1;

        }
        
        else if(myRb.velocity.x < -Mathf.Epsilon && isDirectionAvailable)
        {
            transform.localScale = new Vector2(-1, 1);
            directionForDash = -1;
        }

        if (myBoxCollider.IsTouching(GroundCollider))
        {

           
            isJumping= false;

            airDashCounter = 0;
        }

        if (!myBoxCollider.IsTouching(GroundCollider))
        {
            isJumping = true;

        }
        if (myRb.velocity.y > 45)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, 45);
        }

        if (myBoxCollider.IsTouching(trampolineCollider))
        {
            airDashCounter= 0;
        }


    }

    void ChangeAnimationState(string newAnimation)
    {
        if (newAnimation == currentAnimation) return;

        PlayerAnimator.Play(newAnimation);

        currentAnimation= newAnimation;
    }


    void OnMove(InputValue input)
    {
        moveInput= input.Get<Vector2>();
    }

    void OnDash(InputValue input)
    {

        if (!PlayerAnimator.GetBool("isDead"))
        {
            if (!dashCooldown2 && airDashCounter == 0 && !isAttacking)
            {
                currentDirectionForDash = directionForDash;
                airDashCounter++;
                dashCooldown2 = true;
                dashCooldown = true;
                if (isJumping)
                {
                    isJumping = false;
                }
                StartCoroutine(Dash());

            }
        }
       
    }

    IEnumerator Dash()
    {
        FindObjectOfType<AudioManagerScript>().Play("DashSound", false);
        isDirectionAvailable = false;
        myRb.gravityScale = 0;
        myRb.velocity = new Vector2(dashSpeed * currentDirectionForDash, 0);
        isDashingAn = true;
        yield return new WaitForSecondsRealtime(dashSecond);
        isDashingAn= false;
        isDirectionAvailable = true;
        myRb.gravityScale = normalGravity;
        dashCooldown = false;
        
        if (isAttackPressed)
        {
            myRb.velocity = new Vector2(0, 0);
            isAttacking = true;         
            StartCoroutine(Attack());
            
            
        }
        

        yield return new WaitForSecondsRealtime(dashCooldownSeconds);
        dashCooldown2 = false;
       



    }

    void AnimationHandling()
    {

        if (isDashingAn)
        {

            ChangeAnimationState("Dash");
        }

        else if (isAttackingAn)
        {
            ChangeAnimationState("Attacking");
        }

        else if (isRunning)
        {
            ChangeAnimationState("Running");
        }

        else if (isJumping)
        {
            ChangeAnimationState("Jumping");
        }

        else ChangeAnimationState("Idling");
        


    }

    void Run()
    {
        
        myRb.velocity = new Vector2(moveInput.x * moveSpeed, myRb.velocity.y);

        if (Mathf.Abs(myRb.velocity.x) > 0 && myBoxCollider.IsTouching(GroundCollider))
        {
           
            isRunning = true;

        }
        else
        {
            isRunning= false;
        }

    }

    void OnJump(InputValue input)
    {
        if (!PlayerAnimator.GetBool("isDead"))
        {
            if (!dashCooldown)
            {
                if (!isAttacking)
                {
                    Jump();
                }

            }

        }
    }

    void Jump()
    {
        if (myBoxCollider.IsTouching(GroundCollider))
        {
            FindObjectOfType<AudioManagerScript>().Play("JumpingSound", false);
            myRb.velocity = new Vector2(myRb.velocity.x, jumpSpeed);
        }
            
    }

    void OnFire(InputValue input)
    {

        
        if (!PlayerAnimator.GetBool("isDead") && myBoxCollider.IsTouching(GroundCollider) && !isAttacking)
        {
            if (!dashCooldown)
            {
                myRb.velocity = new Vector2(0, 0);
                
                StartCoroutine(Attack());
            }
            else
            {
                isAttackPressed= true;
            }
        }
    } 



    IEnumerator Attack()
    {
        FindObjectOfType<AudioManagerScript>().Play("PlayerSlash", false);
        isAttacking = true;
        isAttackingAn = true;
       
        yield return new WaitForSeconds(attackSeconds);

        isAttackingAn= false;
        
        isAttacking= false;
        isAttackPressed= false;
        
       
              
    }
    public void LoadData(GameData data)
    {

        transform.position = data.playerPosition;
        sceneNumberCameFromFile = data.sceneNumber;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = transform.position;
        data.sceneNumber = SceneManager.GetActiveScene().buildIndex;
    }

  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trampoline"))
        {
            FindObjectOfType<AudioManagerScript>().Play("TrampolineSound", false);
        }
    }

}
