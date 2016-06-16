using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyHealth : HealthScriptAbstract {

    public int startingHealth = 50;
    public int currentHealth;
    public IList<GameObject> Squad;

 
    Rigidbody rb;
    Animator anim;
    Animator bloodAnim;
    NavMeshAgent agent;

    bool isDead;

    //Audio Components  
       AudioSource allySound;
       public AudioClip[] damagedNoises;
       public AudioClip[] deathNoises;
       private AudioClip selectedNoise;
 

    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody>();
        allySound = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = startingHealth;
        bloodAnim = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void takeDamage(int amount)
    {
        if (isDead)
            return;

        if (!allySound.isPlaying){
            selectedNoise = damagedNoises[Random.Range(0, damagedNoises.Length)];
            allySound.PlayOneShot(selectedNoise);
            bloodAnim.SetTrigger("gettingAttacked");
        }
        currentHealth -= amount;
        //if (rb && agent)
        //{
        //    agent.enabled = false;
        //    rb.drag = 1;
        //    rb.AddForce(new Vector3(0, 6f, 0), ForceMode.VelocityChange);
        //}
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;

        if (!allySound.isPlaying){
            selectedNoise = deathNoises[Random.Range(0, deathNoises.Length)];
            allySound.PlayOneShot(selectedNoise);
        }        

        if (anim)
            anim.SetTrigger("Die");
        //capsuleCollider.isTrigger = true;
        if (agent)
            agent.enabled = false;
        if (Squad != null && Squad.Contains(gameObject))
            Squad.Remove(gameObject);
        Destroy(gameObject, 2.5f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (agent.enabled == false)
        {
            if (other.gameObject.tag == "Ground")
            {
                rb.drag = Mathf.Infinity;
                agent.enabled = true;
                Debug.Log("enabled agent");
            }
        }

    }
}
