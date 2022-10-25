using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1.0f;
    public float powerUpStrength = 15.0f;
    public bool hasPowerUp = false;
    public GameObject powerUpIndicator;

    private Rigidbody playerRigidbody;
    private GameObject focalPoint;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    private void reverse()
    {
        playerRigidbody.velocity = -1 * playerRigidbody.velocity;  // Vector3.zero;
        playerRigidbody.angularVelocity = -1 * playerRigidbody.velocity;  // Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRigidbody.AddForce(forwardInput * speed * focalPoint.transform.forward);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            reverse();
        }

        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.14f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            StartCoroutine(PowerupCountdownRoutine());
            powerUpIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
        } else if (other.CompareTag("Wall"))
        {
            reverse();
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        hasPowerUp = false;
        powerUpIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = enemyRigidbody.transform.position - transform.position;

            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }
}
