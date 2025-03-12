using UnityEngine;

public class Walls : MonoBehaviour
{
    public float bounciness = 1.0f; // Factor de rebote

    void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            // Crea un material físico con fricción 0 y rebote definido
            PhysicsMaterial2D wallMaterial = new PhysicsMaterial2D("WallMaterial");
            wallMaterial.friction = 0f;
            wallMaterial.bounciness = bounciness;
            col.sharedMaterial = wallMaterial;
        }
    }
}
