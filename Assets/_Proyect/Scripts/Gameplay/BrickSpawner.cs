using UnityEngine;
using System.Collections.Generic;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;
    public int rows = 5;
    public int columns = 10;
    public float SpacingX = 1.6f;
    public float SpacingY = 0.6f;
    public int currentProgression = 0;
    public List<GameObject> bricks = new List<GameObject>();

    public int currentHitpoint = 1;
    
    public void SpawnBricks()
    {
        bricks.Clear();
        float totalWidth = (columns - 1) * SpacingX;
        float screenCenterX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0)).x;
        Vector2 startPos = new Vector2(screenCenterX - (totalWidth / 2), 4f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = new Vector3(startPos.x + col * SpacingX, startPos.y - row * SpacingY, 0);

                GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity);
                
                if (row < currentProgression)
                {
                    brick.GetComponent<Brick>().hitPoints = currentHitpoint + 1;
                }
                else
                {
                    brick.GetComponent<Brick>().hitPoints = currentHitpoint;
                }
                
                bricks.Add(brick);
            }
        }
        
        if (GameManagerUI.instance != null)
        {
            GameManagerUI.instance.RegisterBricks(bricks);
        }
    }
    
    public void IncreaseProgression()
    {
        BallController ball = GameObject.FindWithTag("Ball").GetComponent<BallController>();
        currentProgression++;
        if (currentProgression >= rows)
        {
            if (currentHitpoint < 3){
                currentHitpoint++;
            }else{
                ball.speed+=0.5f;
            }
            
            currentProgression = 0;
        }

        ball.speed+=0.5f;
    }
}