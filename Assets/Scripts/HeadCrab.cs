using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeadCrab : MonoBehaviour
{
    public float ShrinkTime = 0.3f;
    public float crabForce = 15f;
    public float maxCrabSpeed = 2f;
    public float turnTorque = 15;
    public float jumpDistance = 2f;
    public float jumpForce = 5f;
    public float turnThresholdDegrees = 2.5f;

    [HideInInspector]
    public Transform target;
    public static int totalHeadCrabs = 0; 
    private Rigidbody _rigidbody;
    private AudioSource _source;
    private bool didJump = false;
    private Vector3 crabForward = Vector3.zero;
    private Vector3 forwardToTarget = Vector3.zero;

    private void Awake()
    {
        totalHeadCrabs++;
        _rigidbody = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + crabForward);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + forwardToTarget);
    }

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
            player.PlayHurtSound();
            Destroy(gameObject);
        }
    }

    private IEnumerator ShrinkAndDie()
    {
        float elapsed = 0f;

        while (elapsed <= ShrinkTime)
        {
            transform.localScale = (1f - elapsed / ShrinkTime) * Vector3.one;
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        totalHeadCrabs--;
    }
}
