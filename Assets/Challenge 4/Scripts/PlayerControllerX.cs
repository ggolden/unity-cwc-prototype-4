using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public float speed = 500;
    public float boost = 10;
    public ParticleSystem boostEffect;
    public int powerupDuration = 5;
    public GameObject powerupIndicator;

    private readonly float hitStrength = 20;
    private readonly float powerupHitStrength = 70;
    private readonly Vector3 powerupAdjust = new(0, -0.6f, 0);

    private Rigidbody physics;
    private GameObject focalPoint;
    private bool hasPowerup;

    void Start()
    {
        physics = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        physics.AddForce(speed * Time.deltaTime * verticalInput * focalPoint.transform.forward);

        if (Input.GetKeyDown(KeyCode.Space)) {
            physics.AddForce(focalPoint.transform.forward * boost, ForceMode.Impulse);
            boostEffect.Play();
        }

        powerupIndicator.transform.position = transform.position + powerupAdjust;
    }

    private void OnTriggerEnter(Collider other)
    {
        IEnumerator PowerupCooldown()
        {
            yield return new WaitForSeconds(powerupDuration);
            hasPowerup = false;
            powerupIndicator.SetActive(false);
        }

        if (other.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
            Destroy(other.gameObject);
        }
    }

    //IEnumerator PowerupCooldown()
    //{
    //    yield return new WaitForSeconds(powerupDuration);
    //    hasPowerup = false;
    //    powerupIndicator.SetActive(false);
    //}

    private Vector3 hitVector(GameObject other)
    {
        Vector3 awayFromPlayer = other.transform.position - transform.position;

        if (hasPowerup)
        {
            return awayFromPlayer * powerupHitStrength;
        }
        else
        {
            return awayFromPlayer * hitStrength;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyPhysics = other.gameObject.GetComponent<Rigidbody>();
            enemyPhysics.AddForce(hitVector(other.gameObject), ForceMode.Impulse);
        }
    }
}
