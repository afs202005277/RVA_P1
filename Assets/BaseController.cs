using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : DefensiveStructure
{
    public override void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyObject();
        }
    }

    protected override void DestroyObject()
    {
        Debug.Log("GAME FINISHED");
        base.DestroyObject();
    }

    protected override void Attack()
    {
        return;
    }

}
