using UnityEngine;
using System.Collections.Generic;

public class Brick : MonoBehaviour
{
    public int hitPoints = 1;             
    public GameObject destructionEffect; 
    public List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip DestroyEffect;
    [SerializeField] private AudioClip HitEffect;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            HandleBallCollision();
        }
    }

    private void HandleBallCollision()
    {
        hitPoints--;
        
        if (hitPoints <= 0)
        {
            DestroyBrick();
        }
        else
        {
            UpdateSprite();
        }

        UpdateGameManager();
    }

    private void UpdateSprite()
    {
        if (hitPoints > 0 && hitPoints <= sprites.Count)
        {
            spriteRenderer.sprite = sprites[hitPoints - 1];
        }
    }

    private void DestroyBrick()
    {
        SpawnDestructionEffect();
        
        if (GameManager.Instance != null)
        {
            AudioManager.Instance.PlayOneShotSound(HitEffect);
            GameManager.Instance.removeBrick(gameObject);
        }
        
        Destroy(gameObject);
    }

    private void SpawnDestructionEffect()
    {
        if (destructionEffect != null)
        {
            GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity);
            ParticleSystemRenderer particleRenderer = effect.GetComponent<ParticleSystemRenderer>();
            
            if (particleRenderer != null)
            {
                particleRenderer.sortingOrder = spriteRenderer.sortingOrder + 10;
            }
        }
    }

    private void UpdateGameManager()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(50);
            GameManager.Instance.CheckAllBlocksDestroyed();
        }
    }
}