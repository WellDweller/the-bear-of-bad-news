using UnityEngine;

public class Doodad : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
