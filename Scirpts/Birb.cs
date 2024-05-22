using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birb : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private CircleCollider2D circleCollider2D;

    private bool hasBeenLaunched = false;
    private bool shouldFaceDirection = true;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        rigidbody2D.isKinematic = true;
        circleCollider2D.enabled = false;
    }
    public void LaunchBirb(Vector2 direction, float force)
    {
        rigidbody2D.isKinematic = false;
        circleCollider2D.enabled = true;
        rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);

        hasBeenLaunched = true;
    }

    private void FixedUpdate()
    {
        if (hasBeenLaunched && shouldFaceDirection)
        {
            transform.right = rigidbody2D.velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        shouldFaceDirection = false;

    }


}
