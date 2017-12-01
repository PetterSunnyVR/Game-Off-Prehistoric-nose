using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy {

    public GameObject bullet;
    public GameObject barrel;

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Update()
    {
        if (!isDead)
        {
            CheckIfShouldAttack();
            if (hasTarget && !player.GetComponent<Player>().isDead)
            {
                tempAttackTimer += Time.deltaTime;
                //Debug.Log(tempAttackTimer + " " + attackTimer);
                if (tempAttackTimer >= attackTimer)
                {
                    animator.SetBool("Attack", true);
                    tempAttackTimer = 0f;
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

    protected void ShootBullet()
    {
        int rotation = transform.localScale.x < 1 ? 90 : -90; 
        GameObject b = Instantiate(bullet, barrel.transform.position, Quaternion.Euler(0, 0, rotation));
        b.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.transform.localScale.x * speed, 0);
        b.GetComponent<Bullet>().SetShooter(gameObject.transform.position);
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
        if (!invoulnarable)
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
        float attackRange = 8f;
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
            animator.SetBool("Attack", false);
            tempAttackTimer = attackTimer;
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && hasTarget)
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
        GetComponent<CircleCollider2D>().enabled = false;
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
