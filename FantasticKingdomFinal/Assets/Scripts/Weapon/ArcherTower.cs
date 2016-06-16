using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArcherTower : MonoBehaviour {

    public GameObject Target;
    public float FireInterval = 0.2f;
    public float FireRadius0 = 1.0f;
    public float FireRadius1 = 3.0f;
    public float FireRadius2 = 6.0f;
    public LayerMask EnemiesLayer;

    private Transform _bowTransform;
    private Transform _arrowTransform;
    private float _nextFireTime;
    private IList<ArcherArrow> _arrowPool;
    private bool _isEnemiesTooClose;
    private Collider[] _targets;
    //private int _enemiesLayer;

    // Use this for initialization
    void Start () {
        //_nextFireTime = Time.time + FireInterval;
        _bowTransform = transform.FindChild("archer_tower_bow");
        _arrowTransform = transform.FindChild("archer_tower_arrow");
        _arrowPool = new List<ArcherArrow>();
        //_enemiesLayer = LayerMask.("Enemies");
    }

    // Update is called once per frame
    void Update() {
        // Seek
        _isEnemiesTooClose = Physics.OverlapSphere(transform.position, FireRadius1, EnemiesLayer).Length > 0;

        if (Target == null && !_isEnemiesTooClose)
        {
            return;
        }

        if (Target == null && _isEnemiesTooClose)
        {
            _targets = Physics.OverlapSphere(transform.position, FireRadius2, EnemiesLayer);
            Target = _targets[Random.Range(0, _targets.Length)].gameObject;
            _nextFireTime = Time.time + FireInterval;
        }

        // Aim
        if (Target != null)
        {
            Vector3 deltaPos = Target.transform.position - _bowTransform.position;
            var deltaRotation = Quaternion.LookRotation(deltaPos);
            var bowStartRotation = _bowTransform.rotation;
            var arrowStartRotation = _arrowTransform.rotation;

            float rotationSpeed = Time.deltaTime * 1.5f;
            _bowTransform.localRotation = Quaternion.Slerp(bowStartRotation, deltaRotation, rotationSpeed);
            _arrowTransform.localRotation = Quaternion.Slerp(arrowStartRotation, deltaRotation, rotationSpeed);

            // Fire
            if (_nextFireTime < Time.time)
            {
                _nextFireTime += FireInterval;

                // try to recycle an arrow
                var activeArrow = (from a in _arrowPool
                                   where !a.IsActive
                                   select a.gameObject).FirstOrDefault();

                if (activeArrow != null)
                {
                    activeArrow.GetComponent<Collider>().isTrigger = false;
                    activeArrow.GetComponent<Rigidbody>().detectCollisions = true;

                    //activeArrow.transform.rotation = _arrowTransform.rotation;
                }
                else
                {
                    activeArrow = (GameObject)Instantiate(_arrowTransform.gameObject, _arrowTransform.position, _arrowTransform.rotation);
                    _arrowPool.Add(activeArrow.GetComponent<ArcherArrow>());
                }

                activeArrow.transform.parent = transform;
                //activeArrow.transform.position = _arrowTransform.position;
                activeArrow.transform.localScale = _arrowTransform.localScale;
                //activeArrow.transform.localRotation = _arrowTransform.localRotation;
                activeArrow.GetComponent<ArcherArrow>().PrototypeArrow = _arrowTransform;
                activeArrow.GetComponent<ArcherArrow>().Target = Target;
                activeArrow.GetComponent<ArcherArrow>().FlyingSpeed = 60.0f;
                activeArrow.GetComponent<ArcherArrow>().IsFlying = true;
                activeArrow.GetComponent<ArcherArrow>().IsActive = true;
                activeArrow.GetComponent<ArcherArrow>().LifeExpiryTime = Time.time + FireInterval * 2;
            }
        }
    }
}
