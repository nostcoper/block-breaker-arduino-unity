using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;       // Velocidad de movimiento
    public float boundary = 8f;     // Límite en el eje X

    void Update()
    {
        float input = Input.GetAxis("Horizontal");
        Vector3 pos = transform.position;
        pos.x += input * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -boundary, boundary);
        transform.position = pos;
    }
}
