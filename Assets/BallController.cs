using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 5f;          // Speed of the cannonball
    public float damage = 20f;         // Damage the cannonball applies
    private GameObject target;         // The target (zombie) for the cannonball
    private float margin = 0.2f;
    private int layerMask;
    private Vector3 lastPosition;

    public void SetTarget(GameObject monster)
    {
        target = monster;
        lastPosition = target.transform.position;
    }

    void Start()
    {
        layerMask = LayerMask.NameToLayer("Monsters");
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;
        if (target != null)
        {
            lastPosition = target.transform.position;
            direction = (target.transform.position - transform.position).normalized;
        }
        else if (lastPosition != null)
        {
            direction = (lastPosition - transform.position).normalized;
            if ((lastPosition - transform.position).magnitude < margin)
            {
                // para quando o monstro for morto e desaparecer e ainda existirem bolas a caminho
                Destroy(gameObject);
            }
        }

        if (direction != Vector3.zero)
        {
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(-90, 0, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layerMask)
        {
            collision.gameObject.GetComponent<MonsterController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
