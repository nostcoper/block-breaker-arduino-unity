using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 8f;
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
                rb.linearVelocity = new Vector2(0, speed);
            }
        }
    }

    void FixedUpdate()
    {
        if (isLaunched)
        {
            Vector2 velocity = rb.linearVelocity;
            if (Mathf.Abs(velocity.y) < 0.1f)
            {
                float verticalOffset = Random.Range(0.5f, 1f);
                velocity.y = verticalOffset;
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
            float x = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.x);
            Vector2 dir = new Vector2(x, 1).normalized;
            rb.linearVelocity = dir * speed;
        }
        else
        {
            Vector2 vel = rb.linearVelocity;
            vel.x += Random.Range(-0.3f, 0.3f);
            vel.y += Random.Range(-0.2f, 0.2f);
            rb.linearVelocity = vel.normalized * speed;
        }
    }

    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleWidth)
    {
        return (ballPos.x - paddlePos.x) / paddleWidth;
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
