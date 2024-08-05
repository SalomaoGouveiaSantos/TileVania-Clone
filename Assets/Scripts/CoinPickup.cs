using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;
    [SerializeField] private int coinValue;

    private GameSession gameSession;
    private bool isCoinPickup = false;

    private string TAG_PLAYER = "Player";

    private void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TAG_PLAYER && !isCoinPickup)
        {
            isCoinPickup = true;
            FindObjectOfType<GameSession>().UpdateTheScore(coinValue);
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            Destroy(gameObject);
            
        }
    }
}
