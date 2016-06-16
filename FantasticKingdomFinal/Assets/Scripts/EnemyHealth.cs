using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    GameObject goldPile;
    public int value = 25;//temp

   
    Rigidbody rb;
    Animator anim;
    GoldPile gold;
    NavMeshAgent agent;
    Vector3 agentDest;

    //CapsuleCollider capsuleCollider;
    bool isDead;
    

    //Audio Components
    AudioSource enemySound;
    public AudioClip[] damagedNoises;
    public AudioClip[] deathNoises;
    private AudioClip selectedNoise;

    Animator bloodAnim;


    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody>();
        bloodAnim = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //capsuleCollider = GetComponent<CapsuleCollider>();
        goldPile = GameObject.Find("GoldPile");
        if (goldPile)
            gold = goldPile.GetComponent<GoldPile>();
        currentHealth = startingHealth;

        //Audio
        enemySound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        if(!isDead){
            if (!enemySound.isPlaying){
                selectedNoise = damagedNoises[Random.Range(0, damagedNoises.Length)];
                enemySound.PlayOneShot(selectedNoise);
                bloodAnim.SetTrigger("gettingAttacked");
            }
        }
        currentHealth -= amount;
        if (rb && agent)
        {
            agentDest = agent.destination;
            agent.enabled = false;
            rb.drag = 1;
            rb.AddForce(new Vector3(0, 6f, 0), ForceMode.VelocityChange);
        }
        if (currentHealth <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;
        selectedNoise = deathNoises[Random.Range(0, deathNoises.Length)];
        enemySound.PlayOneShot(selectedNoise);
        if (anim)
            anim.SetTrigger("Die");
        //capsuleCollider.isTrigger = true;
        if (gold)
        {
            gold.addGold(value);
            gold.addKill();
        }
        if (agent)
            agent.enabled = false;
        Destroy(gameObject, 2.5f);
    }

    void OnCollisionEnter(Collision other)
    {
        if (agent.enabled == false && isDead == false)
        {
            if (other.gameObject.tag == "Ground")
            {
                rb.drag = Mathf.Infinity;
                agent.enabled = true;
                agent.destination = agentDest;
                agent.Resume();
            }
        }
        
    }
}
