using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy {

    Vector2 playerTempPos;
    Vector3 playerTempPosition;
    public bool isWalking = false;
    public bool startMayhame = false;
    public bool isThinking = false;
    int walkCount = 0;
    
    float force = 3;
    public float attackAgainTime;
    float attackTimeCounter = 0;
    // Update is called once per frame

    void FixedUpdate()
    {
        if (!isDead)
        {
            if (startMayhame && !hasTarget)
            {
                if (!isWalking && !hasTarget)
                {
                    playerTempPosition = GameObject.FindObjectOfType<Player>().transform.position;
                    isThinking = false;
                    isWalking = true;
                    Debug.Log("looking");
                    
                }
                else if(!isThinking)
                {
                    attackTimeCounter = attackTimer;
                    float difference = playerTempPosition.x - transform.position.x;
                    if (difference<0)
                    {
                        if(transform.localScale.x > 0)
                        {
                            FlipDirectionOfMovement();
                        }
                    }else
                    {
                        if (transform.localScale.x < 0)
                        {
                            FlipDirectionOfMovement();
                        }
                    }
                    rgb.velocity = new Vector2(speed * gameObject.transform.localScale.x, rgb.velocity.y);
                    animator.SetBool("Walk", true);
                    if(Mathf.Abs(playerTempPosition.x - transform.position.x)<0.5f)
                    {
                        
                        isThinking = true;
                        walkCount++;
                        animator.SetBool("Walk", false);
                        if (walkCount > 2)
                        {
                            attackAgainTime = 1.4f;
                        }
                        Invoke("ResetWalk", attackAgainTime);
                    }
                }
                
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void ResetWalk()
    {
        if (walkCount > 2)
        {
            attackAgainTime = 0.5f;
            walkCount = 0;
        }
        rgb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isThinking = false;
        isWalking = false;
        hasTarget = false;

    }

    private void Update()
    {
        if (!isDead)
        {
            
            enemyAtacking = Physics2D.OverlapCircle(wallCheck.position, checkWallRadious,playerMask);
            if (enemyAtacking && !isThinking)
            {
                //rgb.constraints = RigidbodyConstraints2D.FreezeAll;
                attackTimeCounter += Time.deltaTime;
                //hasTarget = true;
                hasTarget = true;
                isWalking = false;
                animator.SetBool("Walk", false);
                //tempAttackTimer += Time.deltaTime;
                
                
                if (attackTimeCounter > attackTimer)
                {
                    animator.SetBool("Attack", true);
                    Invoke("PlayerGetsHit", 0.3f);
                    isThinking = true;
                    attackTimeCounter = 0;
                }

                //isThinking = true;
                //tempAttackTimer = 0f;
            }else
            {
                hasTarget = false;
            }
        }

    }

    protected void PlayerGetsHit()
    {
        player.GetComponent<Player>().GetHit(2);
        ResetWalk();
    }

    protected void FlipDirectionOfMovement()
    {
        gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    public void LoseLife()
    {
        lives--;
        if (lives > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            GetComponent<AudioSource>().Play();
            Invoke("FlashRed", 0.1f);

        }
        else
        {
            Die();
        }


    }

    public void LoseLife(int number)
    {
        lives -= number;
        if (lives > 0)
        {
            animator.SetTrigger("Hit");
            GetComponent<SpriteRenderer>().color = Color.red;
            GetComponent<AudioSource>().Play();
            Invoke("FlashRed", 0.1f);

        }
        else
        {
            Die();
        }


    }

    protected void FlashRed()
    {

        GetComponent<SpriteRenderer>().color = Color.white;

    }

    protected void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void StopAttacking()
    {
        animator.SetBool("Attack", false);
        Invoke("ResetWalk", attackAgainTime);
    }

    public void SetMayhame(bool val)
    {
        startMayhame = val;

        GameObject.FindObjectOfType<FinishLevelTrigger>().GetComponent<BoxCollider2D>().isTrigger = false;

        
    }

    public void Die()
    {
        isDead = true;
        player.GetComponent<Player>().ResetMayhame();
        GameObject.FindObjectOfType<FinishLevelTrigger>().GetComponent<BoxCollider2D>().isTrigger = true;
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
        GetComponent<AudioSource>().Play();
        animator.SetBool("Die", true);
        Instantiate(healthPack, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, 0.8f) ;
        Invoke("DisableCollider", 1f);
        Invoke("DestroyEnemy", 2f);
    }
}
