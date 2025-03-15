using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    private Camera mainCamera;
    private float leftBoundary;
    private float rightBoundary;
    private float paddleHalfWidth;
    public IPaddleController Controller;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            paddleHalfWidth = renderer.bounds.extents.x;
        }
        else
        {
            paddleHalfWidth = transform.localScale.x / 2;
        }
        CalculateScreenBoundaries();
    }
    
    public void SetController(IPaddleController newController)
    {
        Controller = newController;
    }

    void CalculateScreenBoundaries()
    {
        Vector3 screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z - mainCamera.transform.position.z));
        Vector3 screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z - mainCamera.transform.position.z));
        leftBoundary = screenLeft.x + paddleHalfWidth;
        rightBoundary = screenRight.x - paddleHalfWidth;
    }
    
    void FixedUpdate()
    {
        float input = Controller.GetMovementInput();
        Vector3 pos = transform.position;
        pos.x += input * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
        transform.position = pos;
    }
}