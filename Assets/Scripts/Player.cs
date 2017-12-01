using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    Animator animator;
    Rigidbody2D rgb;


    public bool isDead = false;
    bool isFalling = false;
    private bool mobileMoveToggle = false;
    private bool keyboardMoveToggle = true;
    public int lives;

    //trigger/collider aspect ratio
    private BoxCollider2D blackBarCollider;

    //Move
    private bool facingRight = true;
    public float maxSpeed;
    private bool jumping = true, doubleJump = false;
    public float tempPlayerYPos = 0.0f;

    public float move;

    //Jump
    public float jumpMove;
    public bool jumpLock = false;
    public LayerMask groundLayer;
    public float jumpPower;
    private float jumpYMax = 0.7f;
    private float doubleJumpParameter = 1.4f;

    //Attack
    bool m_isAxisInUse = false;
    public bool hasStone = false;
    public GameObject throwStone;
    public bool attacking = false;
    public LayerMask enemyLayer;
    float throwSpeed = 15;
    bool startMayhame;
    GameObject disposableParent;

    // Use this for initialization
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        blackBarCollider = GameObject.FindGameObjectWithTag("BlackBarLeft").GetComponent<BoxCollider2D>();
        disposableParent = GameObject.Find("Disposable");
        if (disposableParent == null)
        {
            disposableParent = new GameObject("Disposable");
        }
    }


    private void FixedUpdate()
    {
        if (keyboardMoveToggle)
        {
            move = Input.GetAxis("Horizontal");
        }


        if (!isDead && !isFalling)
        {
            float attack = Input.GetAxis("Fire1");

            if (!jumping)
            {


                if (attack != 0)
                {
                    if (m_isAxisInUse == false)
                    {
                        SetAttacking();

                        m_isAxisInUse = true;
                    }
                }
                if (attack == 0)
                {
                    m_isAxisInUse = false;
                }
            }

            if (move != 0 && !attacking)
            {
                if (move > 0) //&& !jumping
                {
                    if (!facingRight)
                    {
                        Flip();
                    }
                    if (!startMayhame)
                    {
                        blackBarCollider.isTrigger = true;
                    }
                    
                    rgb.velocity = new Vector2(move * maxSpeed, rgb.velocity.y);
                }
                else if (move < 0)
                {
                    if (facingRight)
                    {
                        Flip();
                    }
                    if (!startMayhame)
                    {
                        blackBarCollider.isTrigger = false;
                    }
                        
                    rgb.velocity = new Vector2(move * maxSpeed, rgb.velocity.y);
                }
                //else if (!jumping && rgb.velocity.x == 0)
                //{
                //    if ((move > 0 && facingRight) || (move < 0 && !facingRight))
                //    {
                //        if (rgb.velocity.y == 0)
                //        {
                //            int direction = move > 0 ? 1 : -1;
                //            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + direction, transform.position.y), smoothing * Time.deltaTime);
                //        } 
                //        rgb.velocity = new Vector2(move * jumpMove, rgb.velocity.y);
                //        Debug.Log(rgb.velocity.x);
                //    }

                //}
                //animator.SetBool("IsRunning", true);


            }
            if (!attacking)
                animator.SetFloat("MovingSpeed", Mathf.Abs(move));


            float jumpAxisValue = Input.GetAxis("Jump");
            if (keyboardMoveToggle)
            {
                if (!attacking)
                {
                    if (jumpAxisValue > 0 && jumping && (transform.position.y >= (tempPlayerYPos + jumpYMax)) && !doubleJump)
                    {
                        rgb.AddForce(new Vector2(0f, jumpPower - (jumpPower / doubleJumpParameter)), ForceMode2D.Impulse);
                        doubleJump = true;
                    }
                    else if (jumpAxisValue > 0 && !jumping && !jumpLock)
                    {
                        float someValue = 0.6f;
                        RaycastHit2D groundRay;
                        groundRay = Physics2D.Raycast(transform.position, -Vector2.up, someValue, groundLayer);
                        //Debug.DrawRay(transform.position, -Vector2.up, Color.red);
                        //Debug.Log(groundRay.collider);
                        if (groundRay.collider != null)
                        {
                            jumping = true;
                            jumpLock = true;
                            tempPlayerYPos = transform.position.y;
                            animator.SetBool("IsGrounded", false);
                            rgb.velocity = new Vector2(rgb.velocity.x, 0f);
                            rgb.AddForce(new Vector2(0f, jumpPower / doubleJumpParameter), ForceMode2D.Impulse);
                            doubleJump = false;


                        }
                    }

                }
                if (jumpAxisValue == 0 && !jumping)
                {
                    ResetJumpLock();
                }
            }

            //animator.SetFloat("VerticalVelocity", rgb.velocity.y);
            animator.SetFloat("VerticalVelocity", rgb.velocity.y);
        }
        else if (isFalling)
        {
            animator.SetBool("IsFalling", true);
        }



    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    public void EndJump()
    {
        float someValue = 0.6f;
        RaycastHit2D groundRay;
        groundRay = Physics2D.Raycast(transform.position, -Vector2.up, someValue, groundLayer);
        //Debug.DrawRay(transform.position, -Vector2.up, Color.red);
        //Debug.Log(groundRay.collider);
        if (groundRay.collider != null)
        {
            jumping = false;
            animator.SetBool("IsGrounded", true);
        }
    }

    private void ResetJumpLock()
    {
        jumpLock = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && jumping)
        {
            rgb.velocity = new Vector2(0f, rgb.velocity.y);
        }
    }

    public void StopMovingForward()
    {
        rgb.velocity = new Vector2(0, rgb.velocity.y);

    }

    public void StopMoving()
    {
        rgb.velocity = new Vector2(0, 0);

    }

    public void ResetAnimation()
    {
        isFalling = false;
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }

    public void IsFallingTrue()
    {
        isFalling = true;
    }

    public void MobileMove(int value)
    {
        if (mobileMoveToggle)
        {
            move = value;
            if (value == 0 && gameObject)
            {
                //StopMovingForeward();
            }
        }

    }

    public void MobileJump()
    {
        if (mobileMoveToggle && !jumping)
        {
            float someValue = 0.6f;
            RaycastHit2D groundRay;
            groundRay = Physics2D.Raycast(transform.position, -Vector2.up, someValue, groundLayer);
            //Debug.DrawRay(transform.position, -Vector2.up, Color.red);
            //Debug.Log(groundRay.collider);
            if (groundRay.collider != null)
            {
                jumping = true;
                animator.SetBool("IsGrounded", false);
                rgb.velocity = new Vector2(rgb.velocity.x, 0f);
                rgb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
                Debug.Log("Jumping");
            }


        }
    }

    public void ToggleControlls()
    {
        keyboardMoveToggle = !keyboardMoveToggle;
        mobileMoveToggle = !mobileMoveToggle;
    }

    public void PlayerDie()
    {
        //TODO
        Debug.Log("Todo");
    }

    public void SetAttacking()
    {
        attacking = true;
        StopMovingForward();
        animator.SetFloat("MovingSpeed", 0);
        animator.SetTrigger("Attack");
        if (hasStone)
        {
            Debug.Log("Throwing stone");
            GameObject rock = Instantiate(throwStone, transform.position + new Vector3(transform.localScale.x*0.2f, 0.6f, 0), Quaternion.identity);
            rock.transform.parent = disposableParent.transform;
            rock.GetComponent<Rigidbody2D>().isKinematic = false;
            rock.GetComponent<BoxCollider2D>().isTrigger = false;
            rock.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 0) * throwSpeed;
            hasStone = false;
        }

        //Debug.Log("Set attacking");
    }

    public void ResetAttacking()
    {
        attacking = false;
        //Debug.Log("Reset attacking");
    }

    public void AttackRaycast()
    {
        if (!hasStone)
        {
            RaycastHit2D hitRay;
            float distance = 1.2f;
            hitRay = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0), distance, enemyLayer);
            //Debug.DrawRay(transform.position, new Vector2(transform.localScale.x, 0) * distance, Color.red,1.0f);
            if (hitRay.collider != null)
            {
                //Debug.Log(hitRay.collider.gameObject.name);
                hitRay.collider.GetComponent<Enemy>().LoseLife();
                GetComponent<AudioSource>().Play();
            }
            else
            {
                Debug.Log("GOt nothing");
            }
        }
        else
        {
            //GameObject rock = Instantiate(throwStone, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            //rock.GetComponent<Rigidbody2D>().isKinematic = false;
            //rock.GetComponent<BoxCollider2D>().isTrigger = false;
            //rock.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 0) * throwSpeed;
        }

    }

    public void GetHit()
    {

        lives--;
        GameManager.instance.LooseLife(lives);

        if (lives <= 0)
        {
            isDead = true;
            ResetAnimation();
            animator.SetBool("Dead", true);
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GameObject.FindObjectOfType<GameManager>().AddToDeathCount();
            Invoke("ResetGame", 2);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("FlashRed", 0.1f);
        }

    }

    public void GetHit(int num)
    {

        lives-= num;
        GameManager.instance.LooseLife(lives);

        if (lives <= 0)
        {
            isDead = true;
            ResetAnimation();
            GameObject.FindObjectOfType<GameManager>().AddToDeathCount();
            animator.SetBool("Dead", true);
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Invoke("ResetGame", 2);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("FlashRed", 0.1f);
        }

    }

    private void FlashRed()
    {

        GetComponent<SpriteRenderer>().color = Color.white;

    }

    public void ResetGame()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        hasStone = false;
        isDead = false;
        GameManager.instance.ResetGame();
        
    }

    public void SetHasStone()
    {
        hasStone = true;
    }

    public void ResetHasStone()
    {
        hasStone = false;
    }

    public bool CheckHasStone()
    {
        return hasStone;
    }

    public void SetMayhame()
    {
        startMayhame = true;
        blackBarCollider.isTrigger = false;
    }

    public void ResetMayhame()
    {
        startMayhame = false;
        
    }

    public bool GetMayhame()
    {
        return startMayhame;
    }
}
