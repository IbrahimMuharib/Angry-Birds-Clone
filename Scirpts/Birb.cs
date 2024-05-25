using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birb : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private CircleCollider2D circleCollider2D;

    private bool hasBeenLaunched = false;
    private bool shouldFaceDirection = true;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        rigidBody2D.isKinematic = true;
        circleCollider2D.enabled = false;
    }
    public void LaunchBirb(Vector2 direction, float force)
    {
        rigidBody2D.isKinematic = false;
        circleCollider2D.enabled = true;
        rigidBody2D.AddForce(direction * force, ForceMode2D.Impulse);

        hasBeenLaunched = true;
    }

    private void FixedUpdate()
    {
        if (hasBeenLaunched && shouldFaceDirection)
        {
            transform.right = rigidBody2D.velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        shouldFaceDirection = false;

    }


}
