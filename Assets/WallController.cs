using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallController : DefensiveStructure
{

    private List<Collider> archerColliders = new List<Collider>();
    private List<Transform> archerTransforms = new List<Transform>();
    private List<Animator> archerAnimators = new List<Animator>();
    private List<Vector3> archerHighestPoints = new List<Vector3>();
    private float archerRotationSpeed = 30f;
    public GameObject arrowPrefab;

    private bool _canAttack = true;
    private float _stunTimer = 0f;

    void Start()
    {
        base.Initialize();
        Transform[] children = GetComponentsInChildren<Transform>(true); // true to include inactive children
        foreach (var child in children)
        {
            if (child.CompareTag("Archer"))
            {
                archerTransforms.Add(child);

                Collider collider = child.GetComponent<Collider>();
                if (collider != null)
                {
                    archerColliders.Add(collider);
                    archerHighestPoints.Add(collider.bounds.max);
                }

                Animator animator = child.GetComponent<Animator>();
                if (animator != null)
                {
                    archerAnimators.Add(animator);
                    AdjustAnimationSpeeds(animator);
                }
            }
        }
    }

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
        foreach (Animator animator in archerAnimators)
        {
            animator.SetTrigger(currentTarget != null ? "TargetAcquired" : "TargetLost");
        }
        if (currentTarget != null)
        {
            for (int i = 0; i < archerTransforms.Count; i++)
            {
                Transform archer = archerTransforms[i];
                Vector3 directionToTarget = currentTarget.transform.position - archer.position;
                directionToTarget.y = 0;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    archer.rotation = Quaternion.Slerp(archer.rotation, targetRotation, Time.deltaTime * archerRotationSpeed);
                }

                Vector3 arrowPosition = archer.transform.position;
                arrowPosition.y = (float)(archer.position.y + (highestPoint.y - archer.position.y) * 0.75);
                GameObject arrow = Instantiate(arrowPrefab, arrowPosition, archer.rotation);

                ArrowController arrowScript = arrow.GetComponent<ArrowController>();
                arrowScript.setTarget(currentTarget);
            }
            foreach (Animator animator in archerAnimators)
            {
                animator.SetTrigger("shoot");
            }
        }
    }

    protected override void DestroyObject()
    {
        Debug.Log("Tower Destroyed!");
        base.DestroyObject();
    }

    void AdjustAnimationSpeeds(Animator animator)
    {
        AnimationClip animation1Clip = animator.runtimeAnimatorController.animationClips[1];
        AnimationClip animation2Clip = animator.runtimeAnimatorController.animationClips[2];

        // Compute the ratio based on the original length (frames) of the animations
        float animation1Length = (float)(animation1Clip.length - 0.1 * animation1Clip.length); // Total time of animation1
        float animation2Length = (float)(animation2Clip.length); // Total time of animation2

        float totalLength = animation1Length + animation2Length;

        // Calculate the fractions of attackSpeed for each animation based on their original length
        float animation1Duration = (animation1Length / totalLength) * attackCooldown;
        float animation2Duration = (animation2Length / totalLength) * attackCooldown;

        // Calculate the speed multipliers for each animation
        float animation1Speed = animation1Clip.length / animation1Duration;
        float animation2Speed = animation2Clip.length / animation2Duration;

        // Set the speeds dynamically in the animator
        animator.SetFloat("drawArrowMultiplier", animation1Speed);
        animator.SetFloat("recoilMultiplier", animation2Speed);
    }
}

