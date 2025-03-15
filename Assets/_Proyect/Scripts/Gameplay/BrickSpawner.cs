using UnityEngine;

using System.Collections.Generic;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;
    public int rows = 5;
    public int columns = 10;
    public float SpacingX = 0.6f;
    public float SpacingY = 0.6f;

    //PRUEBA
    private List<GameObject> bricks = new List<GameObject>();


    void Start()
    {
        float totalWidth = (columns - 1) * SpacingX;
        float screenCenterX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0)).x;
        Vector2 startPos = new Vector2(screenCenterX - (totalWidth / 2), 3f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = new Vector3(startPos.x + col * SpacingX, startPos.y - row * SpacingY, 0);


                GameObject brick = Instantiate(brickPrefab, pos, Quaternion.identity);

                //Agregar a la lista
                bricks.Add(brick);
            }

            // Registrar los ladrillos con GameManagerUI
            if (GameManagerUI.instance != null)
            {
                GameManagerUI.instance.RegisterBricks(bricks);
            }
        }
    }
}
