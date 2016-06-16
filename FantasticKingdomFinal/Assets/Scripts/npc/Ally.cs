using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ally : Unit {

    public GameObject goldPile;
    public Collider[] PotentialTargets;
    public LayerMask EnemiesLayer;
    public GameObject LocalTarget;
    public GameObject GlobalTarget;
    public GameObject Home; // Original Spawn Point
    public float LocalTargetBreachRadius = 5.0f;
    public float LocalTargetSeekRadius = 7.0f;
    public float PatrolLimitRadius = 30.0f;

    private Vector3? _activeTarget;
    private Animator _anim;

    // Use this for initialization
    public override void Start () {
        LocalTarget = null;
        agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _anim.SetBool("IsWalking", false);
	}
	
	// Update is called once per frame
	public override void Update () {
        if (LocalTarget == null)
        {
            LocalTarget = AllySpawner.SeekTarget(goldPile.transform, LocalTargetBreachRadius, LocalTargetSeekRadius, EnemiesLayer);
        }

        //if (LocalTarget == null && GlobalTarget == null)
        //{
        //    if (agent && agent.isOnNavMesh && agent.enabled)
        //    {
        //        //agent.Stop();
        //        //anim.SetBool("IsWalking", false);
        //    }

        //    return;   
        //}

        // Give up if the target enemy runs too far out.
        if (LocalTarget != null && Vector3.Distance(LocalTarget.transform.position, goldPile.transform.position) > PatrolLimitRadius)
        {
            LocalTarget = null;
        }

        // Give up if the target enemy runs too far out.
        if (GlobalTarget != null && Vector3.Distance(GlobalTarget.transform.position, goldPile.transform.position) > PatrolLimitRadius)
        {
            GlobalTarget = null;
        }

        if (LocalTarget == null && GlobalTarget == null)
        {
            _activeTarget = null;
        }
        else
        {
            _activeTarget = (LocalTarget ?? GlobalTarget).transform.position;
        }
        

        // No target so go home
        if (_activeTarget == null)
        {
            _activeTarget = Home.transform.position;
        }

        // Facing the target first
        _activeTarget = (_activeTarget + transform.position) / 2;
        var relativePos = _activeTarget.Value - this.GetComponent<Transform>().position;
        var rotation = Quaternion.LookRotation(relativePos);
        var current = this.GetComponent<Transform>().rotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * 2);

        if (agent && agent.isOnNavMesh && agent.enabled)
        {
            _anim.SetBool("IsWalking", true);
            agent.destination = _activeTarget.Value;
            agent.speed = (float)speed;
            agent.Resume();
        }
	}


}
