using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviour
{
    public enum BearState
    {
        Idle,
        Chase,
        Attack,
        Death
    }

    private LifeBarManager lifeBarManagerInstance;
    private NavMeshAgent navMeshAgent;
    private int shotsTaken = 0;
    public BearState currentState = BearState.Idle;
    public Animator bearAnimator;
    public Transform playerTransform;
    public float chaseDistance = 10f;
    public float attackDistance = 2f;
    private bool isAttacking = false;

    void Start()
    {
        lifeBarManagerInstance = LifeBarManager.Instance;
        navMeshAgent = GetComponent<NavMeshAgent>();
        bearAnimator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case BearState.Idle:
                bearAnimator.SetBool("Run Forward", false);

                if (distanceToPlayer <= chaseDistance)
                {
                    currentState = BearState.Chase;
                }
                break;

            case BearState.Chase:
                bearAnimator.SetBool("Run Forward", true);

                Vector3 chaseDirection = (playerTransform.position - transform.position).normalized;
                transform.LookAt(playerTransform);
                transform.position += chaseDirection * Time.deltaTime;

                if (distanceToPlayer <= attackDistance)
                {
                    currentState = BearState.Attack;
                }
                break;

            case BearState.Attack:
                if (!isAttacking)
                {
                    // An array of available attack animation triggers
                    string[] attackAnimations = { "Attack1", "Attack2", "Attack3", "Attack5" };

                    // Choose a random attack animation from the available list
                    int randomIndex = Random.Range(0, attackAnimations.Length);
                    string randomAttackTrigger = attackAnimations[randomIndex];


                    // Set the attack trigger to play the attack animation
                    bearAnimator.SetTrigger(randomAttackTrigger);
                    isAttacking = true;

                    // Start a coroutine to transition back to Chase state after a delay
                    StartCoroutine(TransitionBackToChase());
                }
                break;
            case BearState.Death:
                // Set animation to Death
                bearAnimator.SetTrigger("Death");
                break;
        }
    }

    IEnumerator TransitionBackToChase()
    {
        // Wait for a delay to ensure you have time to see the attack animation
        yield return new WaitForSeconds(1.5f); // Adjust the duration as needed

        // update life
        lifeBarManagerInstance.UpdateLife(-25);

        // Reset the attack trigger and transition back to Chase state
        bearAnimator.ResetTrigger("Attack1");
        isAttacking = false;
        currentState = BearState.Chase;
    }

    void Die()
    {
        currentState = BearState.Death;
        bearAnimator.SetBool("Run Forward", false);
        bearAnimator.SetBool("Death", true);
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }
        enabled = false;
    }

    public void OnShot()
    {
        shotsTaken++;
        if (shotsTaken >= 5)
        {
            Die();
        } else
        {
            bearAnimator.SetTrigger("GetHitFront");
        }
    }
}
