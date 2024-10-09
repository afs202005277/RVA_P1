using System;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private GameObject target;
    private Vector3 headPosition;
    private Rigidbody rb;
    private bool isStuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchArrow();
    }

    private void Update()
    {
        if (!isStuck)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(-90, 0, 0);
        }
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
        Collider monsterCollider = target.GetComponent<Collider>();

        if (monsterCollider != null)
        {
            float targetHeadOffset = monsterCollider.bounds.size.y;

            Vector3 targetPosition = target.transform.position;

            headPosition = new Vector3(targetPosition.x, targetPosition.y + targetHeadOffset, targetPosition.z);
        }
        else
        {
            Debug.LogWarning("Monster has no collider!");
        }
    }

    void LaunchArrow()
    {
        // Calculate the initial velocity needed to hit the target
        Vector3 velocity = CalculateLaunchVelocity();
        if (velocity != Vector3.zero)
        {
            // Apply the calculated velocity to the arrow's Rigidbody
            rb.velocity = velocity;
        }
        else
        {
            Debug.LogWarning("No valid trajectory found!");
        }
    }

    Vector3 CalculateLaunchVelocity()
    {
        Vector3 finalCoords = headPosition;

        Vector3 requiredMovement = headPosition - transform.position;

        requiredMovement.y = 0;

        Vector3 startSpeed = requiredMovement / (float)(Math.Sqrt(-2 * transform.position.y / Physics.gravity.y));

        return startSpeed;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Im stuck!");
        Debug.Log(collision.gameObject.name);
        Stick();
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Monsters")
        {
            // Logic when the arrow hits the monster
            Debug.Log("Arrow hit the monster!");
            StickToMonster(target);
        }
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Debug.Log("destroying in 5 seconds");
            Destroy(gameObject, 5);
        }
    }

    void Stick()
    {
        rb.isKinematic = true;
        isStuck = true;
        GetComponent<Collider>().enabled = false;
    }

    void StickToMonster(GameObject monster)
    {
        rb.isKinematic = true;

        GetComponent<Collider>().enabled = false;

        transform.SetParent(monster.transform);
        Debug.Log("Set parent!");
    }
}
