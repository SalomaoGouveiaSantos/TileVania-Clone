using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 1.0f;

    private string TAG_ENEMY = "Enemy";

    private Rigidbody2D bulletRb;
    private PlayerMovement player;
    private float xSpeed;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        bulletRb = GetComponent<Rigidbody2D>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }
    private void Update()
    {
        bulletRb.velocity = new Vector2(xSpeed, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TAG_ENEMY)
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
