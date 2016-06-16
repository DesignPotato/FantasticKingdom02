using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyAttack : MonoBehaviour {
    public float attackRate = 0.5f;
    public Animator anim;
    private float _nextAttack = 0.0f;
    public int AttackDamage = 50; // This should be set by the spawner and Black smith
    private Ally _allyScript;

    //Audio Components 
    AudioSource allySound;
    public AudioClip[] attackNoises;
	private AudioClip selectedNoise;
    
    void Awake()
    {
        //this script must be on child of ally
        anim = transform.parent.GetComponent<Animator>();
        _allyScript = (Ally)gameObject.GetComponentInParent(typeof(Ally));
		allySound = gameObject.transform.parent.gameObject.GetComponent<AudioSource> ();
    }

    //private GameObject Target
    //{
    //    get
    //    {
    //        return ((Ally)gameObject.GetComponentInParent(typeof(Ally))).LocalTarget;
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        Attack(other);
    }

    void OnTriggerStay(Collider other)
    {
        Attack(other);
    }

    private void Attack(Collider col)
    {
        if ((col.gameObject == _allyScript.LocalTarget || col.gameObject == _allyScript.GlobalTarget) && Time.time > _nextAttack)
        {
            var enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(10);
            _nextAttack = Time.time + attackRate;

            if (!allySound.isPlaying){
            selectedNoise = attackNoises[Random.Range(0, attackNoises.Length)];
            allySound.PlayOneShot(selectedNoise);
        }

            if(anim)
                anim.SetTrigger("Attack");
            Debug.Log("Attack");
        }
    }
}
