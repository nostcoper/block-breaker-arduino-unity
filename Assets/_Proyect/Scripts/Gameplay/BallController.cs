using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 8f;
    public float minVerticalVelocity = 2.0f;
    public float paddleImpactRandomFactor = 0.2f;
    public float wallImpactRandomFactor = 0.1f;
    private Rigidbody2D rb;
    private bool isLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if (Input.GetKeyDown(KeyCode.Space))
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
        GameObject paddle = GameObject.FindWithTag("Paddle");
        if (paddle != null)
        {
            transform.position = paddle.transform.position + new Vector3(0, 0.5f, 0);
        }
    }
}