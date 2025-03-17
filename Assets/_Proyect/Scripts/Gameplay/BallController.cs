using UnityEngine;
using UnityEngine.UIElements;

public class BallController : MonoBehaviour
{
    public float speed = 8f;
    public float minVerticalVelocity = 2.0f;
    public float paddleImpactRandomFactor = 0.2f;
    public float wallImpactRandomFactor = 0.1f;
    private Rigidbody2D rb;
    private bool isLaunched = false;
    [SerializeField] AudioClip HitBrickEffect;
    [SerializeField] AudioClip HitAnyEffect;
    public GameObject destructionEffect; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0.2f;
    }

    void Update()
    {
        if (!isLaunched)
        {
            GameObject paddle = GameObject.FindWithTag("Paddle");
            if (paddle != null)
            {
                transform.position = paddle.transform.position + new Vector3(0, 0.5f, 0);
            }
            if (paddle.GetComponent<PaddleController>().Controller.GetLaunchInput())
            {
                isLaunched = true;
                LaunchBall();
            }
        }
    }

    void LaunchBall()
    {
        float randomAngle = Random.Range(-30f, 30f);
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
        rb.linearVelocity = direction * speed;
        
        // Añade un poco de rotación inicial
        rb.angularVelocity = Random.Range(-90f, 90f);
    }

    void FixedUpdate()
    {
        if (isLaunched)
        {
            if (rb.linearVelocity.magnitude != speed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * speed;
            }

            Vector2 velocity = rb.linearVelocity;
            float absY = Mathf.Abs(velocity.y);
            
            if (absY < minVerticalVelocity)
            {
                float sign = Mathf.Sign(velocity.y);
                if (sign == 0) sign = 1;
                
                velocity.y = sign * minVerticalVelocity;
                rb.linearVelocity = velocity.normalized * speed;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLaunched)
            return;

        if (collision.gameObject.CompareTag("Paddle"))
        {
            HandlePaddleCollision(collision);
        }
        else
        {
            HandleWallOrBlockCollision();
        }
        AplicarGiroEnColision(collision);

        if (collision.gameObject.CompareTag("Brick")){
            AudioManager.Instance.PlayOneShotSound(HitBrickEffect);
        }else{
            AudioManager.Instance.PlayOneShotSound(HitAnyEffect);
        }
    }

    void AplicarGiroEnColision(Collision2D collision)
    {
        Vector2 normalImpacto = collision.contacts[0].normal;
        float factorGiro = Vector2.Dot(normalImpacto, rb.linearVelocity.normalized);
        rb.angularVelocity += factorGiro * 20f; 
    }

    void HandlePaddleCollision(Collision2D collision)
    {
        float hitFactor = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.x);
        float angle = hitFactor*45f;
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
        direction.x += Random.Range(-paddleImpactRandomFactor, paddleImpactRandomFactor);
        rb.linearVelocity = direction.normalized * speed;
    }

    void HandleWallOrBlockCollision()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x += Random.Range(-wallImpactRandomFactor, wallImpactRandomFactor);
        velocity.y += Random.Range(-wallImpactRandomFactor, wallImpactRandomFactor);
        rb.linearVelocity = velocity.normalized * speed;
    }

    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleWidth)
    {
        return (paddlePos.x - ballPos.x) / (paddleWidth / 2);
    }

    public void ResetBall()
    {
        isLaunched = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        GameObject paddle = GameObject.FindWithTag("Paddle");
        if (paddle != null)
        {
            transform.position = paddle.transform.position + new Vector3(0, 0.5f, 0);
        }
    }
    public void SpawnDeathEffect(){
        if (destructionEffect != null)
        {
            GameObject effect = Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }
    }
}