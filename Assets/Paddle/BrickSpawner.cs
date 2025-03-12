using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brickPrefab;    // Prefab del ladrillo
    public int rows = 5;              // Número de filas
    public int columns = 10;          // Número de columnas
    public float spacing = 0.6f;      // Espaciado entre ladrillos
    public Vector2 startPos = new Vector2(-7.5f, 3f);  // Posición inicial de la cuadrícula

    void Start()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = new Vector3(startPos.x + col * spacing, startPos.y - row * spacing, 0);
                Instantiate(brickPrefab, pos, Quaternion.identity);
            }
        }
    }
}
