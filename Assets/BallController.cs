using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;          // Speed of the cannonball
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
        Debug.Log("Cannon ball created");
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
        }
        Debug.Log($"Cannon ball Update Position: {transform.position.x}, {transform.position.y}, {transform.position.z}");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layerMask)
        {
            Debug.Log("Cannon ball destroyed");
            // Destroy the cannonball
            collision.gameObject.GetComponent<MonsterController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
