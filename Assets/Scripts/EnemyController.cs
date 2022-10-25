using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 0.5f;

    private Rigidbody enemyRigidbody;
    private GameObject player;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        enemyRigidbody.AddForce(moveDirection * speed);

        if (transform.position.y < -10.0f)
        {
            Destroy(gameObject);
        }
    }
}
