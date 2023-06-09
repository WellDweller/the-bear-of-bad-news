using System.Collections.Generic;
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

    [SerializeField] List<Mover> patientPrefabs;

    int currentPatientIndex = 0;

    Person activePerson;

    float movedDistance;
    Mover nextPatient;



    void Awake()
    {
        hallway.OnMove += TrackDistance;
        var firstPatient = InstaniateCurrentPrefab();
        firstPatient.transform.position = currentPatient.transform.position;
        Destroy(currentPatient.gameObject);
        currentPatient = firstPatient;
        activePerson = firstPatient.GetComponent<Person>();

    }

    void OnDestroy()
    {
        hallway.OnMove -= TrackDistance;
    }


    public void GotoNextPatient()
    {
        movedDistance = 0;
        currentPatientIndex += 1;
        currentPatientIndex %= patientPrefabs.Count;
        nextPatient = InstaniateCurrentPrefab();
        var distanceToMove = distanceBetweenPatients * currentPatient.MoveSpeed;
        nextPatient.transform.position = currentPatient.transform.position + new Vector3(distanceToMove, 0, 0);

        activePerson.Weep();
        bear.Walk(0);
        hallway.StartMoving();
        currentPatient.StartMoving();
        nextPatient.StartMoving();
        conversationController.HideAll();
        OnStartMoving?.Invoke();
    }

    Mover InstaniateCurrentPrefab()
    {
        var nextPrefab = patientPrefabs[currentPatientIndex];
        return Instantiate(nextPrefab);
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
        // not happy with how this is wired up, but we can't just wire this in the editor because the sad person object
        // gets destroyed and replaced throughout the game
        activePerson = nextPatient.GetComponent<Person>();
        nextPatient = null;
        OnStopMoving?.Invoke();
        bear.Idle(0);
    }
}
