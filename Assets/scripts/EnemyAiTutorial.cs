﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAiTutorial : MonoBehaviour
{
    private float lerpSpeed = 5f; // Adjust for smoothness

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, whatIsObstacle;
    public float health, maxHealth = 100;
    public Transform BulletSpawn;
    public Slider healthBar; // Reference to the health bar slider

    Animator animator;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject bullet;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, playerVisible;

    private void Awake()
    {
        player = GameObject.Find("OVRCameraRig").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        health = maxHealth; // Initialize health
        if (healthBar) healthBar.value = health / maxHealth;
    }

    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        // Update health bar position
        //if (healthBar)
        //{
        //    healthBar.transform.position = transform.position + Vector3.up * 2f; // Adjust height
        //    healthBar.transform.LookAt(player); // Face player
        //}

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        Vector3 direction = transform.forward;

        Debug.DrawRay(BulletSpawn.position, direction * attackRange, Color.red, 1.0f);

        RaycastHit hit;

        if (Physics.Raycast(BulletSpawn.position, direction, out hit, attackRange))
        {
            if (hit.collider.gameObject.name == "OVRCameraRig") // Ensures enemy sees the player
            {
                playerVisible = true;
                Debug.Log(hit.collider.gameObject.name);
            }
        }
        else
        {
            playerVisible= false;
        }

        Debug.Log(playerVisible);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !playerVisible) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && playerVisible) AttackPlayer();

        /*bool hasClearSight = false;
        if (playerInSightRange)
        {
            RaycastHit hit;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, ~whatIsGround))
            {
                if (hit.transform == player) // Ensures enemy only sees player if no obstacle is in between
                {
                    hasClearSight = true;
                }
            }
        }

        if (!hasClearSight)
        {
            Patroling();
            return;
        }

        if (hasClearSight && !playerInAttackRange)
            ChasePlayer();
        else if (hasClearSight && playerInAttackRange)
            AttackPlayer();*/
    }
    
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        //controller.TakeDamage(10);
        //Make sure enemy doesn't move

        Vector3 direction = (player.position - BulletSpawn.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (!alreadyAttacked)
        {
            ///Attack code here
            GameObject spawnedBullet = Instantiate(bullet, BulletSpawn.position, Quaternion.identity);
            Rigidbody rb = spawnedBullet.GetComponent<Rigidbody>();

            rb.linearVelocity = direction * 32f;
            ///End of attack code
            
            agent.SetDestination(transform.position);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Destroy(spawnedBullet, 2f);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthBar)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, health / maxHealth, Time.deltaTime * lerpSpeed);
        }

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
        GameStatistics.Instance.Kills += 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
