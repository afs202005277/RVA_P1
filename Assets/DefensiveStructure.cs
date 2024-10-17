using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefensiveStructure : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Defensive Stats")]
    public float health;

    [Header("Attack Stats")]
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;

    protected float nextAttackTime = 0f;

    public LayerMask monstersLayer;  // Layer to detect monsters
    protected GameObject currentTarget;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    protected Vector3 highestPoint;

    void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {
        monstersLayer = LayerMask.GetMask("Monsters");

        Collider collider = gameObject.GetComponent<Collider>();

        if (collider != null)
        {
            Bounds bounds = collider.bounds;
            highestPoint = bounds.max;
        }
        else
        {
            //Debug.LogError("Collider not found on the tower.");
        }
    }

    public abstract void TakeDamage(float damage);

    public abstract void Stun(float seconds);
    protected abstract void Attack();

    void Update()
    {
        DetectAndAttack();
    }

    protected void DetectAndAttack()
    {
        DetectMonsters();
        if (currentTarget != null && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    protected void DetectMonsters()
    {
        Collider[] monstersInRange = Physics.OverlapSphere(transform.position, attackRange, monstersLayer);
        if (monstersInRange.Length > 0)
        {
            currentTarget = monstersInRange[0].gameObject;
        }
        else
        {
            currentTarget = null;  // No monsters in range
        }
    }

    protected virtual void DestroyObject()
    {
        gameManager.RemoveDefense(gameObject);
        Debug.Log("Defensive object destroyed!");
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
