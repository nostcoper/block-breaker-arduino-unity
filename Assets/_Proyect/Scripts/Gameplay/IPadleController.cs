using System;
using UnityEngine;

public interface IPaddleController
{
    float GetMovementInput();
    void Movement(GameObject Paddle, float leftBoundary, float rightBoundary);
    bool GetLaunchInput();

    void SetSpeed(float value);
}
