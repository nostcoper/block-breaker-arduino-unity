using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardController", menuName = "Controllers/Keyboard")]
public class KeyboardController : ScriptableObject, IPaddleController
{
    private float speed;
    public bool GetLaunchInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public float GetMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        return horizontal;
    }

    public void Movement(GameObject Paddle, float leftBoundary, float rightBoundary){
        float input = GetMovementInput();
        Vector3 pos = Paddle.transform.position;
        pos.x += input * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
        Paddle.transform.position = pos;
    }

    public void SetSpeed(float value){
        speed = value;
    }
}
