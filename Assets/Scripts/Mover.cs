using System;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    public event Action<float> OnMove;

    public bool IsMoving { get; private set; }

    [field:SerializeField] public UnityEvent OnStartMoving { get; private set; }

    [field:SerializeField] public UnityEvent OnStopMoving { get; private set; }

    [SerializeField] float moveSpeed;

    // For testing; used like a button in the editor;
    [Header("Debugging/Testing")]
    [SerializeField] bool startMove;
    [SerializeField] bool stopMove;



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

        var moveAmount = moveSpeed * Time.deltaTime;
        OnMove?.Invoke(moveAmount);
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
