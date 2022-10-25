using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;

    private Rigidbody physics;
    private GameObject target;

    void Start()
    {
        physics = GetComponent<Rigidbody>();
        target = GameObject.Find("Player Goal");
    }

    void Update()
    {
        Vector3 targetVector = (target.transform.position - transform.position).normalized;
        physics.AddForce(speed * Time.deltaTime * targetVector);
    }
}
