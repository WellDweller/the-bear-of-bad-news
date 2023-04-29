using UnityEngine;
using UnityEngine.Events;

public class PatientChanger : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnStartMoving { get; private set; }

    [field:SerializeField] public UnityEvent OnStopMoving { get; private set; }

    [SerializeField] float distanceBetweenPatients;
    [SerializeField] Mover hallway;
    [SerializeField] Mover currentPatient;
    [SerializeField] Bear bear;
    [SerializeField] Conversation conversationController;


    float movedDistance;
    Mover nextPatient;



    void Awake()
    {
        hallway.OnMove += TrackDistance;
    }


    public void GotoNextPatient()
    {
        movedDistance = 0;
        nextPatient = Instantiate(currentPatient);
        nextPatient.transform.position = currentPatient.transform.position + new Vector3(distanceBetweenPatients, 0, 0);

        bear.Walk(0);
        hallway.StartMoving();
        currentPatient.StartMoving();
        nextPatient.StartMoving();
        conversationController.HideAll();
        OnStartMoving?.Invoke();
    }

    void TrackDistance(float amount)
    {
        movedDistance += amount;
        if (movedDistance >= distanceBetweenPatients)
            StopMoving();
    }

    void StopMoving()
    {
        hallway.StopMoving();
        currentPatient.StopMoving();
        nextPatient.StopMoving();

        Destroy(currentPatient.gameObject);
        currentPatient = nextPatient;
        nextPatient = null;
        OnStopMoving?.Invoke();
        bear.Idle(0);
    }
}
