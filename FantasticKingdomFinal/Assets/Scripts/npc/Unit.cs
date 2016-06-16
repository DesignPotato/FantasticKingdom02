using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour{
    
    //Some basic attributes
    protected double health;
    protected int armour;
    protected int magResist;
    protected int damage;
    public int speed;

    //For navigation
    protected NavMeshAgent agent;

    // Use this for initialization
    public abstract void Start();

    // Update is called once per frame
    public abstract void Update();

}
