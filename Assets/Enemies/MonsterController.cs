using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Animator animator;
    public GameManager gameManager;
    public float health = 10f;
    public float speed = 2f;
    public float damage = 1f;
    private GameObject currentDefense = null;

    void Start()
    {
        animator.SetBool("isWalking", true);
    }   

    void Update()
    {
        List<GameObject> defenses = gameManager.defenses;


        // Get closest defense to the monster
        GameObject closestDef = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject defense in defenses)
        {
            float distance = Vector3.Distance(defense.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestDef = defense;
            }
        }

        Vector3 direction = (closestDef.transform.position - transform.position).normalized;

        Vector3 increment = speed * Time.deltaTime * direction;

        if (animator.GetBool("isWalking"))
        {
            transform.position += increment;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Defenses") && !currentDefense)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
            currentDefense = collision.gameObject;
        } 

    }


    public void DealDamage()
    {
        if (!currentDefense) {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            currentDefense = null;
            return;
        }
        currentDefense.GetComponent<DefensiveStructure>().TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            animator.SetBool("died", true);
        }
    }

    public void KillEnemy()
    {
        Destroy(gameObject);
    }
}
