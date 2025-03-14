using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;
    public int rows = 5;        
    public int columns = 10;     
    public float SpacingX = 0.6f;   
    public float SpacingY = 0.6f;     

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
                Instantiate(brickPrefab, pos, Quaternion.identity);
            }
        }
    }
}
