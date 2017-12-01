using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy {

    protected Vector3 rightBlock, leftBlock;
    protected bool goingLeft = true, goingRight = false;
    public float distance;
    public bool isAtacking;
    protected float timePassedFromAttack = 0;
	// Use this for initialization
    protected void Start () {
        base.Start();
        rightBlock = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        leftBlock =  new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
        if (gameObject.transform.localScale.x < 0)
        {
            goingLeft = true;
            goingRight = false;
        }else
        {
            goingLeft = false;
            goingRight = true;
        }
        isAtacking = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {


        if (!isDead)
        {
            float direction = gameObject.transform.localScale.x;
            rgb.velocity = new Vector2(speed * gameObject.transform.localScale.x, rgb.velocity.y);
            if((transform.position.x <= leftBlock.x && direction < 0) && goingLeft)
            {
                FlipDirectionOfMovement();
                goingLeft = false;
                goingRight = true;

            }else if (transform.position.x >= rightBlock.x && direction > 0 && goingRight)
            {
                FlipDirectionOfMovement();
                goingLeft = true;
                goingRight = false;
            }
        }
        

        
        


	}

    private void Update()
    {
        if (!isDead)
        {
            if (!isAtacking)
            {
                timePassedFromAttack += Time.deltaTime;
                if (timePassedFromAttack > 4)
                {
                    isAtacking = true;
                }
            }
        }

    }

    protected void PlayerGetsHit()
    {
        player.GetComponent<Player>().GetHit();
    }

    protected void FlipDirectionOfMovement()
    {
        gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    public void LoseLife()
    {
        if(!invoulnarable)
        {
            invoulnarable = true;
            lives--;
            
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
        

        
    }

    public void LoseLife(int number)
    {
        if (!invoulnarable)
        {
            invoulnarable = true;
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
        


    }

    protected void FlashRed()
    {

        GetComponent<SpriteRenderer>().color = Color.white;
        invoulnarable = false;
    }

    protected void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    protected void CheckIfShouldAttack()
    {

        RaycastHit2D attackRay;
        float attackRange = 3f;
        Vector2 direction;
        if (player.transform.position.x < transform.position.x)
        {
            direction = new Vector2(-1, 0);
        }
        else
        {
            direction = new Vector2(1, 0);
        }
        attackRay = Physics2D.Raycast(transform.position, direction, attackRange, playerMask);
        Debug.DrawRay(transform.position, direction * attackRange);
        if (attackRay.collider != null && attackRay.collider.gameObject.CompareTag("Player"))
        {
            int dir = gameObject.transform.position.x > player.transform.position.x ? -1 : 1;
            if (gameObject.transform.localScale.x != dir)
            {
                FlipDirectionOfMovement();
            }
            if (!hasTarget)
            {
                tempAttackTimer = attackTimer;
                hasTarget = true;
            }


            //animator.SetBool("Attack",true);
        }
        else
        {
            hasTarget = false;
            rgb.constraints = RigidbodyConstraints2D.FreezeRotation;
            animator.SetBool("Attack", false);
            tempAttackTimer = attackTimer;
        }
    }

    public void StopAttacking()
    {
        animator.SetBool("Attack", false);
    }

    public void DisableCollider()
    {
        rgb.mass = 1;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void Die()
    {
        Debug.Log("one down");
        isDead = true;
        rgb.velocity = new Vector2(0, 0);
        rgb.isKinematic = false;
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
        GetComponent<AudioSource>().Play();
        animator.SetBool("Die", true);
        GameObject hpack = Instantiate(healthPack, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
        hpack.transform.parent = disposableParent.transform;
        Invoke("DisableCollider", 1f);
        Invoke("DestroyEnemy", 2f);
    }


    protected void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Platyer");
            if (isAtacking)
            {
                Debug.Log("Platyer attack");
                isAtacking = false;
                player.GetComponent<Player>().GetHit();
            }
        }
    }
}
