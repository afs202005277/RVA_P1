using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Animator animator;
    public GameManager gameManager;
    public float health;
    public float speed;
    public float damage;
    public float stunTime = 1f;
    private GameObject currentDefense = null;

    public GameObject coinPopupPrefab;
    public Transform cameraTransform;
    public float maxHeight = 1.5f;
    public float duration = 1.0f;
    private float coins;

    void Start()
    {
        animator.SetBool("isWalking", true);
        cameraTransform = Camera.main.transform;
    }   

    void Update()
    {
        List<GameObject> defenses = gameManager.defenses;


        // Get closest defense to the monster
        GameObject closestDef = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject defense in defenses)
        {
            float distance = Vector3.Distance(defense.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestDef = defense;
            }
        }

        Vector3 direction = (closestDef.transform.position - transform.position).normalized;

        Vector3 increment = speed * Time.deltaTime * direction;

        if (animator.GetBool("isWalking"))
        {
            transform.position += increment;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
    }

    public void applyDifficultySettings(MonsterSettings settings)
    {
        this.damage *= settings.DamageModifier;
        this.speed *= settings.SpeedModifier;
        this.coins = settings.AbsoluteMoneyEarned;
        this.health *= settings.HPModifier;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Defenses") && !currentDefense)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
            currentDefense = collision.gameObject;
        } 

    }


    public void DealDamage()
    {
        if (!currentDefense) {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            currentDefense = null;
            return;
        }
        currentDefense.GetComponent<DefensiveStructure>().TakeDamage(damage);
    }

    public void ScavengerDealDamage()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Monsters"))
        {
            gameObject.layer = LayerMask.NameToLayer("Monsters");
        }
        DealDamage();
    }

    public void SpitterDealDamage()
    {
        DealDamage();
        currentDefense.GetComponent<DefensiveStructure>().Stun(stunTime);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !animator.GetBool("died"))
        {
            animator.SetBool("died", true);
            ShowCoinPopup(gameObject.transform.position, this.coins);
            gameManager.updateMoney(this.coins, true);
        }
    }

    public void KillEnemy()
    {
        gameManager.killMonster();
        Destroy(gameObject);
    }

    public void ShowCoinPopup(Vector3 position, float coinAmount)
    {
        GameObject popup = Instantiate(coinPopupPrefab, position, Quaternion.identity);
        TextMeshPro textMeshPro = popup.GetComponent<TextMeshPro>();
        textMeshPro.text = $"+{(float)Math.Round(coinAmount, 2)}"; // Set the pop-up text

        Vector3 direction = cameraTransform.position - popup.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        popup.transform.rotation = lookRotation;

        StartCoroutine(PopUpAnimation(popup.transform));
    }

    private IEnumerator PopUpAnimation(Transform popupTransform)
    {
        Vector3 startPosition = popupTransform.position;
        Vector3 targetPosition = startPosition + Vector3.up * maxHeight;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp to the target position
            popupTransform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            // Make the text face the camera
            popupTransform.LookAt(cameraTransform);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        Destroy(popupTransform.gameObject);
    }
}
