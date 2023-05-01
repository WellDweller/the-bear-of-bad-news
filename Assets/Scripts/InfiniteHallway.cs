using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Mover))]
public class InfiniteHallway : MonoBehaviour
{
    [SerializeField] SpriteRenderer hallwaySprite;
    [SerializeField] List<Sprite> doodadOptions;
    [SerializeField] List<Doodad> doodads;
    [SerializeField] float moveDoodadWhenLessThanX;
    [SerializeField] float doodadSpacing;

    float walkedDistance;
    float spriteWidth;
    Vector3 startingPosition;
    Vector3 clonedOffset;
    SpriteRenderer clonedHallwaySprite;

    Mover mover;


    void Awake()
    {
        var clone = Instantiate(hallwaySprite);
        clone.transform.SetParent(transform);
        clone.transform.position = hallwaySprite.transform.position;
        clonedHallwaySprite = clone;

        startingPosition = hallwaySprite.transform.position;
        spriteWidth = hallwaySprite.bounds.size.x;
        clonedOffset = new(spriteWidth, 0, 0);
        mover = GetComponent<Mover>();
        mover.OnMove += Move;

        foreach (var doodad in doodads)
        {
            RandomizeDoodad(doodad);
        }
    }

    void OnDestroy()
    {
        mover.OnMove -= Move;
    }



    // Update is called once per frame
    void Move(float amount)
    {
        walkedDistance += amount;
        walkedDistance %= spriteWidth;

        Vector3 offset = new(-walkedDistance, 0, 0);
        var newPosition = startingPosition + offset;
        hallwaySprite.transform.position = newPosition;
        clonedHallwaySprite.transform.position = newPosition + clonedOffset;

        Vector3 doodadMove = new(-amount, 0, 0);
        foreach (var doodad in doodads)
        {
            doodad.transform.position += doodadMove;
            if (doodad.transform.position.x <= moveDoodadWhenLessThanX)
            {
                doodad.transform.position += new Vector3(doodadSpacing * doodads.Count, 0, 0);
                RandomizeDoodad(doodad);
            }
        }
    }

    void RandomizeDoodad(Doodad doodad)
    {
        doodad.spriteRenderer.sprite = doodadOptions[Random.Range(0, doodadOptions.Count)];
    }
}
