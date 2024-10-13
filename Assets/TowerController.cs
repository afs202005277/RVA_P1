using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : DefensiveStructure
{
    private bool _canAttack = true;
    private float _stunTimer = 0f;
    public override void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyObject();
        }
    }

    public override void Stun(float seconds)
    {
        if (_canAttack)
        {
            StartCoroutine(StunCoroutine(seconds));
        }
        else
        {
            _stunTimer = Mathf.Max(_stunTimer, seconds);
        }
    }

    private IEnumerator StunCoroutine(float seconds)
    {
        _canAttack = false;
        _stunTimer = seconds;

        while (_stunTimer > 0f)
        {
            _stunTimer -= Time.deltaTime;
            yield return null;
        }

        _canAttack = true;
        _stunTimer = 0f;
    }

    protected override void Attack()
    {
        if (!_canAttack)
        {
            return;
        }
        if (currentTarget != null)
        {
            if (projectilePrefab == null)
            {
                projectilePrefab = Resources.Load<GameObject>("SpikyBall");
            }

            GameObject cannonball = Instantiate(projectilePrefab, highestPoint, transform.rotation);
            BallController cannonballScript = cannonball.GetComponent<BallController>();
            cannonballScript.damage = attackDamage;
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
