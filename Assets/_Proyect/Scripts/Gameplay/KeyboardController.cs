using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardController", menuName = "Controllers/Keyboard")]
public class KeyboardController : ScriptableObject, IPaddleController
{
    public bool GetLaunchInput()
    {
        throw new System.NotImplementedException();
    }

    public float GetMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        return horizontal;
    }
}
