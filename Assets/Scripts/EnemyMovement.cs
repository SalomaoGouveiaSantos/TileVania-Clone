using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float enemySpeed = 1f;
    private Rigidbody2D enemyRb;
    private void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        enemyRb.velocity = new Vector2(enemySpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemySpeed = -enemySpeed;
        FlipEnemyFacing();
    }
    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRb.velocity.x)), 1f);
    }
}
