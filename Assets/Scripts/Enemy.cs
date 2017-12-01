using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    protected Rigidbody2D rgb;
    public Transform groundCheck;
    public GameObject healthPack;
    protected GameObject player;
    public Transform wallCheck;
    public LayerMask playerMask;
    protected Animator animator;
    protected bool grounded = true;
    protected bool wallHit = false;
    protected bool hasTarget = false;
    protected bool enemyAtacking = false;
    protected bool isDead = false;
    public float speed;
    protected float checkWallRadious = 0.001f;
    protected float checkPlayerRadious = 0.1f;
    public int lives;
    public float attackTimer = 0f;
    protected float tempAttackTimer = 0f;
    protected bool invoulnarable = false;
    protected GameObject disposableParent;

    // Use this for initialization
    protected void Start () {
        rgb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>().gameObject;
        disposableParent = GameObject.Find("Disposable");
        if (disposableParent == null)
        {
            disposableParent = new GameObject("Disposable");
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {


        if (!isDead)
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position, checkWallRadious);
            //Always move foreward - if not fighting that is
            if (!hasTarget)
            {
                //check if there is ground to walk on
                
                if (!grounded)
                {
                    FlipDirectionOfMovement();
                }
                //check if there is an obstacle in front
                wallHit = Physics2D.OverlapCircle(wallCheck.position, checkWallRadious);
                if (wallHit && !hasTarget)
                {
                    FlipDirectionOfMovement();
                    
                }
            }
            if (!enemyAtacking)
            {
                if (grounded)
                {
                    rgb.velocity = new Vector2(speed * gameObject.transform.localScale.x, rgb.velocity.y);
                    animator.SetBool("Walk", true);
                }
                
            }
        }
        

        
        


	}

    private void Update()
    {
        if (!isDead)
        {
            CheckIfShouldAttack();
            if (hasTarget && !player.GetComponent<Player>().isDead)
            {
                enemyAtacking = Physics2D.OverlapCircle(wallCheck.position, checkWallRadious, playerMask);
                if (!enemyAtacking || !hasTarget)
                {
                    enemyAtacking = Physics2D.OverlapCircle(wallCheck.position, checkPlayerRadious, playerMask);
                }
                if (enemyAtacking)
                {
                    animator.SetBool("Walk", false);
                    tempAttackTimer += Time.deltaTime;
                    //Debug.Log(tempAttackTimer + " " + attackTimer);
                    if (tempAttackTimer >= attackTimer)
                    {
                        animator.SetBool("Attack", true);
                        //player.GetComponent<Player>().GetHit();
                        Invoke("PlayerGetsHit", 0.3f);
                        tempAttackTimer = 0f;
                    }
                }

            }
            else
            {
                if (enemyAtacking)
                {
                    enemyAtacking = false;
                }
                if (player.GetComponent<Player>().isDead)
                {
                    hasTarget = false;
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

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && hasTarget)
        {
            rgb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void StopAttacking()
    {
        animator.SetBool("Attack", false);
    }

    public void DisableCollider()
    {
        rgb.mass = 1;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public void Die()
    {
        isDead = true;
        rgb.velocity = new Vector2(0, 0);
        rgb.gravityScale = 1;
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

    

}
