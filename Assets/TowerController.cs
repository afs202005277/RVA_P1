using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : DefensiveStructure
{
    public override void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyObject();
        }
    }

    protected override void Attack()
    {
        if (currentTarget != null)
        {
            if (projectilePrefab == null)
            {
                projectilePrefab = Resources.Load<GameObject>("SpikyBall");
            }

            GameObject cannonball = Instantiate(projectilePrefab, highestPoint, transform.rotation);
            BallController cannonballScript = cannonball.GetComponent<BallController>();
            if (cannonballScript != null)
            {
                cannonballScript.SetTarget(currentTarget);
            }
        }
    }

    protected override void DestroyObject()
    {
        Debug.Log("Tower Destroyed!");
        base.DestroyObject();
    }
}
