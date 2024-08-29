using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform[] Waypoints;
    [SerializeField] private Transform playerTransform;
    private Animator animator;
    private NavMeshAgent agent;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        while (!isChasing)
        {
            int waypointIndex = Random.Range(0, Waypoints.Length);
            animator.SetBool("IsWalking", true);
            agent.SetDestination(Waypoints[waypointIndex].position);
            yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
            animator.SetBool("IsWalking", false);
            yield return new WaitForSeconds(3f);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (isChasing)
            return;
        if (other.CompareTag("Player"))
        {
            if (IsPlayerVisible())
            {
                isChasing = true;
                StopCoroutine(Patrol());
                StartCoroutine(ChasePlayer(5));
            }
        }
    }

    bool IsPlayerVisible()
    {
        RaycastHit hit;
        Vector3 direction = playerTransform.position - transform.position;
        int layerMask = ~LayerMask.GetMask("Ghost");
        if (Physics.Raycast(transform.position, direction.normalized, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator ChasePlayer(float chaseTime)
    {
        UIManager.instance.ShowSirenImage();
        float elapsedTime = 0f;
        agent.speed = 2.5f;
        animator.SetBool("IsWalking", true);
        while (elapsedTime < chaseTime)
        {
            if (playerTransform != null)
            {
                agent.SetDestination(playerTransform.position);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isChasing = false;
        UIManager.instance.HideSirenImage();
        agent.speed = 1.5f;
        animator.SetBool("IsWalking", false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(Patrol());
    }

    public void CallGhost()
    {
        if (isChasing)
            return ;
        isChasing = true;
        StopCoroutine(Patrol());
        StartCoroutine(ChasePlayer(15));
    }


}