using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    // variables
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targettingRange = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f; // Bullets per seconds

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        // find the next target if the current one is lost
        if (target == null)
        {
            FindTarget();
            return;
        }

        // aim the cannon towards the current target
        RotateTowardsTarget();

        // fire at the target
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            // manage attack speed
            timeUntilFire += Time.deltaTime;

            // ready?
            if (timeUntilFire >= 1f / bps)
            {
                // FIRE!
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    // FIRE!
    private void Shoot()
    {
        // create the bullet from the end of the cannon sprite
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        // send invisible code-laser-beams all around until one encounters an enemy
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targettingRange, (Vector2)
            transform.position, 0f, enemyMask);

        // take the first valid target that is found
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    // establish a turret radius
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targettingRange;
    }

    // turn the cannon towards current target
    private void RotateTowardsTarget()
    {
        // set angle to face the target (it got weird and i had to subtract 90 degrees so that it faced correctly)
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        // convert the angle to a Quaternion (not sure what a quaternion is yet still)
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        // make the rotation smoooooooth
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // draw the radius in the scene editor so i know the radius is working correctly
    private void OnDrawGizmosSelected()
    { 
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targettingRange);

    }

}
