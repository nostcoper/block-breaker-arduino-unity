using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    private Camera mainCamera;
    private float leftBoundary;
    private float rightBoundary;
    private float paddleHalfWidth;
    [SerializeField] public IPaddleController Controller;
    
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
        if (newController is  KeyboardController){
            Controller.SetSpeed(speed);
        }
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

        Controller.Movement(this.gameObject, leftBoundary, rightBoundary);
    }
}