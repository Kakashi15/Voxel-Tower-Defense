using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject capsulePrefab;    // Prefab of the capsule to be shot
    public Transform target;             // Transform of the target to shoot at
    public float shootingInterval = 2f;  // Time interval between shots

    private float timeSinceLastShot;     // Time since the last shot
    public Transform shootPosition;
    public float shootForce;
    public bool canShoot = true;
    public float attackDistance = 5f;
    public Vector3 posUpdate;
    public Vector3 rotUpdate;

    public float rotationSpeed = 5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target && canShoot)
        {
            Vector3 targetDirection = target.position - transform.position;

            // Calculate the rotation towards the target using lerp
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Smoothly rotate towards the target using lerp
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        else
            return;

        timeSinceLastShot += Time.deltaTime;

        // Check if enough time has passed to shoot again
        if (timeSinceLastShot >= shootingInterval && canShoot)
        {
            Shoot();
            timeSinceLastShot = 0f;  // Reset the time since the last shot
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Collector"))
            target = other.transform;

        if (other.gameObject.tag.Equals("Collector") &&
            Vector3.Distance(transform.position, other.gameObject.transform.position) < attackDistance
            && canShoot)
        {
            canShoot = false;
            other.gameObject.GetComponent<UnitController>().OnUpdatePosition(gameObject, posUpdate, rotUpdate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag.Equals("Collector"))
        //{
        //    canShoot = false;
        //    collision.gameObject.GetComponent<UnitController>().OnUpdatePosition(gameObject, posUpdate, rotUpdate);
        //}
    }

    private void Shoot()
    {
        // Instantiate a new capsule at the current position and rotation
        GameObject capsule = Instantiate(capsulePrefab, shootPosition.position, shootPosition.rotation);

        // Calculate the direction towards the target
        Vector3 direction = (target.position - shootPosition.position).normalized;

        // Apply a force to the capsule to move it towards the target
        Rigidbody capsuleRigidbody = capsule.GetComponent<Rigidbody>();
        capsuleRigidbody.velocity = CalculateParabolicVelocity(direction, shootForce);
        //capsuleRigidbody.AddForce(direction * shootForce, ForceMode.Impulse);
    }

    private Vector3 CalculateParabolicVelocity(Vector3 direction, float initialSpeed)
    {
        // Calculate the distance to the target
        float distance = Vector3.Distance(transform.position, target.position);

        // Calculate the time of flight
        float timeOfFlight = distance / initialSpeed;

        // Calculate the vertical velocity component for parabolic motion
        float verticalVelocity = (target.position.y - transform.position.y + 0.5f * Physics.gravity.magnitude * timeOfFlight * timeOfFlight) / timeOfFlight;

        // Calculate the horizontal velocity component
        Vector3 horizontalVelocity = direction * initialSpeed;

        // Combine the vertical and horizontal velocities
        Vector3 velocity = horizontalVelocity + Vector3.up * verticalVelocity;

        return velocity;
    }
}
