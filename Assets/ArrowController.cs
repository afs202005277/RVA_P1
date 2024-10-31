using System;
using Unity.VisualScripting;
using UnityEngine;
using Vuforia;

public class ArrowController : MonoBehaviour
{
    private GameObject target;
    private Vector3 headPosition;
    private Rigidbody rb;
    private bool isStuck = false;
    private MonsterController targetScript;
    private Vector3 center;
    public float damage;

    private float _destroyDelay = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        LaunchArrow();
        DestroyAfterDelay();
    }

    private void Update()
    {
        Transform arCameraTransform = VuforiaBehaviour.Instance.transform;
        if (!isStuck)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(-90, 0, 0);
        }
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
        this.targetScript = target.GetComponent<MonsterController>();
        Collider monsterCollider = target.GetComponent<Collider>();
        center = monsterCollider.bounds.center;
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
        Vector3 copy = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //drawSphere(copy, Color.green);
        //drawLine(transform.position, velocity.normalized, Color.red);
        if (ContainsNaN(velocity))
        {
            Debug.Log("Velocity with nan. Arrow destroyed.");
            Destroy(gameObject);
        }
        else if (velocity != Vector3.zero)
        {
            // Apply the calculated velocity to the arrow's Rigidbody
            rb.velocity = velocity;
        }
        else
        {
            Debug.LogWarning("ArrowController: No valid trajectory found!");
        }
    }

    Vector3 CalculateLaunchVelocity()
    {
        float g = Physics.gravity.y;

        Vector3 archerPosition = transform.position;
        Vector3 monsterHeadPosition = center;

        float heightDifference = (monsterHeadPosition.y - archerPosition.y);

        float time = Mathf.Sqrt(2 * heightDifference / g);

        float vy = heightDifference / time;

        Vector3 horizontalDisplacement = new Vector3(monsterHeadPosition.x - archerPosition.x, 0, monsterHeadPosition.z - archerPosition.z);

        Vector3 horizontalVelocity = horizontalDisplacement / time;

        Vector3 velocity = horizontalVelocity + new Vector3(0, vy, 0);

        float horizontalDistance = horizontalDisplacement.magnitude;

        time = horizontalDistance / horizontalVelocity.magnitude;

        vy = (heightDifference - 0.5f * g * Mathf.Pow(time, 2)) / time;

        velocity = horizontalVelocity + new Vector3(0, vy, 0);

        velocity.y = velocity.y / 6;
        return velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Arrow collided!");
        Stick();
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Monsters")
        {
            StickToMonster(target);
        }
        if (collision.gameObject.CompareTag("Terrain"))
        {
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
        if (monster != null)
        {
            transform.SetParent(monster.transform);
            monster.GetComponent<MonsterController>().TakeDamage(damage);
        }
    }

    void drawSphere(Vector3 position, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name += color.ToString();
        sphere.transform.position = position;
        sphere.transform.localScale = new Vector3(1, 1, 1);
        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.color = color;
    }

    void drawLine(Vector3 direction, Vector3 start, Color color)
    {
        GameObject lineObject = new GameObject("InfiniteLine");

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material for lines
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Set the position count (2 points for a simple line)
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start + direction.normalized*100);
    }

    bool ContainsNaN(Vector3 vector)
    {
        return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
    }

    void DestroyAfterDelay()
    {
        Invoke(nameof(DestroyArrow), _destroyDelay);
    }
    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
