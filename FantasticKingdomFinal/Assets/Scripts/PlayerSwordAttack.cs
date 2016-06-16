using UnityEngine;
using System.Collections;

public class PlayerSwordAttack : MonoBehaviour {

    public int damage;
    public int powerDamage;
    public float timeBetweenAttacks = 0.5f;
    public static bool attacking = false;
    public static bool power = false;


    float timer;
    bool canAttack;


    void Awake () {
        canAttack = true;
	}
	
	void Update () {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks)
        {
            canAttack = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Damage(other.gameObject); 
        }
    }

    public void PowerAttack()
    {
        attacking = true;
        power = true;
    }

    public void QuickAttack()
    {
        attacking = true;
    }

    public void FinishAttack()
    {
        attacking = false;
        power = false;
    }

    void Damage(GameObject enemy)
    {
        if (canAttack && attacking && !power)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            eh.TakeDamage(damage);
            canAttack = false;
            timer = 0f;
        }
        if (canAttack && attacking && power)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            eh.TakeDamage(powerDamage);
            canAttack = false;
            timer = 0f;
        }
    }
}
