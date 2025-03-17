using UnityEngine;
using System.Collections.Generic;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;
    public int rows = 5;
    public int columns = 10;
    public float SpacingX = 1.6f;
    public float SpacingY = 0.6f;
    public int currentProgression = -1;
    public List<GameObject> bricks = new List<GameObject>();

    public int currentHitpoint = 1;
    
    public void SpawnBricks()
    {
        IncreaseProgression();
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
                
                // Sistema de progresión que aumenta la vida según la fila y la progresión
                if (row < currentProgression)
                {
                    brick.GetComponent<Brick>().hitPoints = currentHitpoint + 1;
                }
                else
                {
                    brick.GetComponent<Brick>().hitPoints = currentHitpoint;
                }
                
                // Add to the list
                bricks.Add(brick);
            }
        }
        
        // Register the bricks with GameManagerUI
        if (GameManagerUI.instance != null)
        {
            GameManagerUI.instance.RegisterBricks(bricks);
        }
    }
    
    // Method to increase progression
    public void IncreaseProgression()
    {
        currentProgression++;
        
        // Si la progresión alcanza el número de filas, aumenta currentHitpoint y reinicia progresión
        if (currentProgression >= rows)
        {
            currentHitpoint++;
            currentProgression = 0;  // Reinicia la progresión para comenzar un nuevo ciclo
            
            // Opcional: Añadir límite a la vida máxima si lo deseas
            // if (currentHitpoint > 5) currentHitpoint = 5;
        }
    }
}