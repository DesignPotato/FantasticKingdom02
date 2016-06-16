using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcherArrow : MonoBehaviour {
    public bool IsFlying = false;
    public bool IsActive;
    public GameObject Target;
    public float FlyingSpeed;
    public Vector3 targetScale;
    //public float MaxLifePeriod = 20.0f;
    public float LifeExpiryTime;
    public Transform PrototypeArrow;

    private Vector3 arrowStoragePlace = new Vector3(0, -100, 0);
    private Transform parentArcherTransform;

    // Use this for initialization
    void Start () {
        parentArcherTransform = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
        if (!IsActive)
        {
            if (PrototypeArrow != null)
            {
                transform.position = PrototypeArrow.position;
                transform.rotation = PrototypeArrow.rotation;
            }
            return;
        }

        // Reset arrow  
        if (Time.time > LifeExpiryTime)
            Reset();

        if (IsFlying && Target != null)
        {
			Vector3 targetPos = Target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * FlyingSpeed);
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale , Time.deltaTime * FlyingSpeed);
        }
    }

    private void Reset()
    {
        // todo recycle rather than destory.
        //Destroy(gameObject);
        IsActive = false;
        //transform.position = parentArrowTransform.position; //arrowStoragePlace;
        transform.parent = parentArcherTransform;
        //transform.localPosition = new Vector3(0, 0, 0);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            //Debug.Log("Arrow hit");
            IsFlying = false;
            //this.transform.position = col.contacts[0].point;
            this.GetComponent<Collider>().isTrigger = true;
            GetComponent<Rigidbody>().detectCollisions = false;
            transform.parent = col.transform;
            var enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(30);
        }
    }
}
