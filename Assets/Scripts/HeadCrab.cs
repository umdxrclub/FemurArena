using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy within FemurArena. 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class HeadCrab : MonoBehaviour
{
    /// <summary>
    /// The time over which to shrink the crab once it's shot.
    /// </summary>
    public float shrinkTime = 0.3f;
    
    /// <summary>
    /// The force used to push the crab forward.
    /// </summary>
    public float crabForce = 15f;
    
    /// <summary>
    /// The max speed of the crab.
    /// </summary>
    public float maxCrabSpeed = 2f;
    
    /// <summary>
    /// The max torque used to turn the crab.
    /// </summary>
    public float turnTorque = 15;
    
    /// <summary>
    /// The distance at which to make the crab jump.
    /// </summary>
    public float jumpDistance = 2f;
    
    /// <summary>
    /// The force used to make the crab jump.
    /// </summary>
    public float jumpForce = 5f;
    
    /// <summary>
    /// The threshold to apply torque on the crab. 
    /// </summary>
    public float turnThresholdDegrees = 2.5f;

    /// <summary>
    /// The target to have the crab follow.
    /// </summary>
    [HideInInspector]
    public Transform target;
    
    /// <summary>
    /// Counts the total amount of head crabs active within the scene.
    /// </summary>
    public static int totalHeadCrabs = 0; 
    
    /// <summary>
    /// The attached RigidBody component.
    /// </summary>
    private Rigidbody _rigidbody;
    
    /// <summary>
    /// The attached AudioSource component.
    /// </summary>
    private AudioSource _source;
    
    /// <summary>
    /// A flag that determines whether the crab has jumped yet.
    /// </summary>
    private bool didJump = false;
    
    /// <summary>
    /// The current forward vector of the crab.
    /// </summary>
    private Vector3 crabForward = Vector3.zero;
    
    /// <summary>
    /// The current forward vector to the target relative to the crab's position.
    /// </summary>
    private Vector3 forwardToTarget = Vector3.zero;

    private void Awake()
    {
        totalHeadCrabs++;
        _rigidbody = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // Only move the crab if there's a target to follow.
        if (target)
        {
            float distanceToTarget = Vector3.Distance(target.position, transform.position);

            // Add forward velocity.
            if (_rigidbody.velocity.magnitude < maxCrabSpeed)
                _rigidbody.AddRelativeForce(new Vector3(0f, 0f, crabForce));

            // Add turning force towards player.
            crabForward = transform.forward;
            forwardToTarget = (target.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(crabForward, forwardToTarget, Vector3.up);

            if (Mathf.Abs(angle) > turnThresholdDegrees)
            {
                float t = angle / 180f;
                float torque = Mathf.Lerp(turnTorque / 2f, turnTorque, Mathf.Abs(t));
                if (t < 0)
                    torque = -torque;

                _rigidbody.AddRelativeTorque(new Vector3(0f, torque, 0f));
            }

            // Add jump force.
            if (distanceToTarget <= jumpDistance && !didJump)
            {
                didJump = true;
                _rigidbody.AddRelativeForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
            }
        }
    }
    
    /// <summary>
    /// In the Scene View, this draws the crab's forward direction in blue and the direction to the target in yellow.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + crabForward);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + forwardToTarget);
    }

    /// <summary>
    /// Called when the crab collides with another RigidBody. We test if the crab has collided with a bullet or player.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            // Play hit sound.
            _source.Play();
            
            // Destroy the bullet.
            Destroy(collision.collider.gameObject);
            
            // Bullet struck the head crab, time to die.
            StartCoroutine(ShrinkAndDie());
        } 
        else if (collision.collider.gameObject.GetComponent<Player>() is Player player)
        {
            // Crab has struck player.
            player.PlayHurtSound();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// A coroutine that shrinks the crab over "shrinkTime" seconds and then destroys the crab.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShrinkAndDie()
    {
        float elapsed = 0f;

        while (elapsed <= shrinkTime)
        {
            transform.localScale = (1f - elapsed / shrinkTime) * Vector3.one;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }

    /// <summary>
    /// Decrements the total head crab static variable.
    /// </summary>
    private void OnDestroy()
    {
        totalHeadCrabs--;
    }
}
