using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InfiniteHallway : MonoBehaviour
{
    public bool IsMoving { get; private set; }



    [field:SerializeField] public UnityEvent OnStartMoving { get; private set; }

    [field:SerializeField] public UnityEvent OnStopMoving { get; private set; }

    [SerializeField] float moveSpeed;

    // For testing; used like a button in the editor;
    [Header("Debugging/Testing")]
    [SerializeField] bool startMove;
    [SerializeField] bool stopMove;
    
    [SerializeField] SpriteRenderer hallwaySprite;

    float walkedDistance;
    float spriteWidth;
    Vector3 startingPosition;
    Vector3 clonedOffset;
    SpriteRenderer clonedHallwaySprite;


    void Awake()
    {
        var clone = Instantiate(hallwaySprite);
        clone.transform.SetParent(transform);
        clone.transform.position = hallwaySprite.transform.position;
        clonedHallwaySprite = clone;

        startingPosition = hallwaySprite.transform.position;
        spriteWidth = hallwaySprite.bounds.size.x;
        clonedOffset = new(spriteWidth, 0, 0);
    }



    // Update is called once per frame
    void Update()
    {
        if (startMove)
        {
            startMove = false;
            StartMoving();
        }
        if (stopMove)
        {
            stopMove = false;
            StopMoving();
        }

        if (!IsMoving)
            return;
        
        walkedDistance += moveSpeed * Time.deltaTime;
        walkedDistance %= spriteWidth;

        Vector3 offset = new(-walkedDistance, 0, 0);
        var newPosition = startingPosition + offset;
        hallwaySprite.transform.position = newPosition;
        clonedHallwaySprite.transform.position = newPosition + clonedOffset;
    }



    public void StartMoving()
    {
        if (IsMoving)
            return;
        IsMoving = true;
        OnStartMoving?.Invoke();
    }

    public void StopMoving()
    {
        if (!IsMoving)
            return;
        IsMoving = false;
        OnStopMoving?.Invoke();
    }
}
