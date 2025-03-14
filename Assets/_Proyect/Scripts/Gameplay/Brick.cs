using UnityEngine;
using System.Collections.Generic;
using System;

public class Brick : MonoBehaviour
{
    public int hitPoints = 1;             
    public GameObject destructionEffect; 
    public List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer spriteRenderer;
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            hitPoints--;
            if (hitPoints <= 0)
            {
                if (destructionEffect != null)
                {
                    Instantiate(destructionEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);

            } else{
                Debug.Log(sprites[3-hitPoints].name);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }
            
        }
    }
}
