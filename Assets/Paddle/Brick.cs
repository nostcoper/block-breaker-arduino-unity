using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hitPoints = 1;             // Impactos necesarios para destruir el ladrillo
    public GameObject destructionEffect;  // Efecto opcional al destruir el ladrillo

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
                // Suma 1 punto por ladrillo destruido
                if (GameManager.Intance != null)
                {
                    GameManager.Intance.AddScore(1);
                }
                Destroy(gameObject);
            }
        }
    }
}
