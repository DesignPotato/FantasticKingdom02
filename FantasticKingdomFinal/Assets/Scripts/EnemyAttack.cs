using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    GameObject player;
    GameObject target;
    AllyHealth targetHealth;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    Animator anim;
    bool playerInRange;
    bool targetInRange;
    float timer;


    void Awake () {
        //player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        //Debug.Log("player: " + player + " health: " + playerHealth);
        //Audio

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
            player = other.gameObject;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        if (other.gameObject.tag == "Ally")
        {
            if (!target)
            {
                targetInRange = true;
                target = other.gameObject;
                targetHealth = target.GetComponent<AllyHealth>();
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
        if(other.gameObject == target)
        {
            targetInRange = false;
            target = null;
            targetHealth = null;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack(0);
        }
        if (timer >= timeBetweenAttacks && targetInRange && enemyHealth.currentHealth > 0)
        {
            Attack(1);
        }
    }


    void Attack(int target)
    {   

        Debug.Log("goblin attacking player");
        timer = 0f;
        if (target == 0)
        {
            if (playerHealth.currentHealth > 0)
            {
                playerHealth.takeDamage(attackDamage);
            }
        }
        else if (target == 1)
        {
            if (targetHealth.currentHealth > 0)
            {
                targetHealth.takeDamage(attackDamage);
            }
        }
        if (anim)
        {
            anim.SetTrigger("Attacking");
        }
    }
}
