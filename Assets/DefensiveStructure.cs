using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefensiveStructure : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Burning")]
    public GameObject burningPrefab;

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public GameObject buildingPrefab;
    public GameObject leftOversPrefab;

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

    protected bool _burning = false;
    protected bool _burnCool = false;

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

    public abstract void Burn(float seconds);
    protected abstract void Attack();

    void Update()
    {
        DetectAndAttack();
    }

    protected void DetectAndAttack()
    {
        DetectMonsters();
        BurnDamage();
        if (currentTarget != null && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    protected void BurnDamage()
    {
        if (!burningPrefab)
            return;
        if (_burning && !_burnCool)
        {
            TakeDamage(2);
            StartCoroutine(WaitBeforeCheck(0.5f));
        }
    }

    protected IEnumerator WaitBeforeCheck(float seconds)
    {
        _burnCool = true;
        yield return new WaitForSeconds(seconds);
        _burnCool = false;
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
        gameObject.layer = LayerMask.NameToLayer("Default");
        explosionPrefab.SetActive(true);
        buildingPrefab.SetActive(false);
        leftOversPrefab.SetActive(true);
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
