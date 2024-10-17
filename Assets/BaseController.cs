using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : DefensiveStructure
{

    private float _burnTimer = 0f;

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
        return; // No Effect on Base
    }

    public override void Burn(float seconds)
    {
        if (_burning)
        {
            StartCoroutine(BurnCoroutine(seconds));
        }
        else
        {
            _burnTimer = Mathf.Max(_burnTimer, seconds);
        }
    }

    private IEnumerator BurnCoroutine(float seconds)
    {
        _burning = true;
        _burnTimer = seconds;

        while (_burnTimer > 0f)
        {
            _burnTimer -= Time.deltaTime;
            yield return null;
        }

        _burning = false;
        _burnTimer = 0f;
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
