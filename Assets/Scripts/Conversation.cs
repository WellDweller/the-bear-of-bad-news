using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Conversation : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnConversationEnd { get; private set; }
    [field:SerializeField] public UnityEvent OnRoundEnd { get; private set; }

    [SerializeField] Dialog dialog;
    private int currentEncounterIndex = 0;

    [Header("Text bubbles for questions")]
    [SerializeField] TextBubble patientQuestion;
    [SerializeField] TextBubble bearAnswer;

    [Header("Other config")]
    [SerializeField] MinigameLoader loader;

    [SerializeField, Min(1)] int loops;

    int currentLoop;
    bool gameOver;

    void Start()
    {
        Debug.Log("dialog: ");
        Debug.Log(dialog);
        Debug.Log("dialog.Data:");
        Debug.Log(dialog.Encounters);
        // Debug.Log(dialog.Data.Keys);
        // encounterStageNames = dialog.Data.Keys.ToList();
        // Debug.Log(encounterStageNames);
    }

    private Encounter GetCurrentEncounterData()
    {
        return dialog.Encounters[currentEncounterIndex];
    } 

    public void StartConversation()
    {
        Encounter currentEncounter = GetCurrentEncounterData();
        patientQuestion.Reset();
        bearAnswer.Reset();

        patientQuestion.Text = currentEncounter.encounterRounds[currentLoop].question;
        patientQuestion.StartTyping();
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public void EndConversation()
    {
        currentLoop += 1;

        if (gameOver || currentLoop >= loops)
        {
            HideAll();
            OnConversationEnd?.Invoke();
        }
        else
            OnRoundEnd?.Invoke();
    }

    public void HideAll()
    {
        patientQuestion.Hide();
        bearAnswer.Hide();
    }
}
