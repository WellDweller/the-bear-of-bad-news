using System;
using UnityEngine;
using UnityEngine.Events;



public class StageManager : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnSceneReady { get; private set; }

    [SerializeField] Mover bear;

    void Start()
    {
        var startPosition = bear.transform.position;
        float offset = Camera.main.pixelWidth / 100f;
        bear.transform.position -= new Vector3(offset, 0, 0);

        float trackedMovement = 0;

        Action<float> handleMove = null;
        handleMove = (float amount) => {
            trackedMovement += amount;
            if (trackedMovement >= offset)
                bear.StopMoving();
        };
        bear.OnMove += handleMove;

        UnityAction handleStopMoving = null;
        handleStopMoving = () => {
            bear.OnMove -= handleMove;
            bear.OnStopMoving.RemoveListener(handleStopMoving);

            OnSceneReady?.Invoke();
        };
        bear.OnStopMoving.AddListener(handleStopMoving);

        bear.StartMoving();
    }
}
