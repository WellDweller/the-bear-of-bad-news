using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Conversation : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnConversationEnd { get; private set; }
    [field:SerializeField] public UnityEvent OnRoundEnd { get; private set; }

    [SerializeField] Dialog dialog;

    [Header("Text bubbles for questions")]
    [SerializeField] TextBubble patientQuestion;
    [SerializeField] TextBubble bearAnswer;
    [SerializeField] TextBubble patientResponse;

    [Header("Other config")]
    [SerializeField] MinigameLoader loader;

    int currentEncounterIndex = 0;
    EncounterRoundData currentEncounterRound;
    int loops;

    int currentLoop; // It would be better to hook this into some global game state keeping track of encounters instead
    bool gameOver;

    private void Start() {
        loops = dialog.dialogData.encounters.Count();
    }

    private EncounterRoundData GetCurrentEncounterRoundData()
    {
        return dialog.dialogData.encounters[currentLoop].encounterRounds[currentEncounterIndex];
    } 

    public void StartConversation()
    {
        currentEncounterRound = GetCurrentEncounterRoundData();
        patientQuestion.Reset();
        bearAnswer.Reset();
        patientResponse.Reset();

        patientQuestion.Text = currentEncounterRound.question;
        patientQuestion.StartTyping();
        currentEncounterIndex += 1;
    }

    public void ConfigureMinigame(Minigame game)
    {
        game.ConfigureGame(currentEncounterRound);
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public void EndConversation()
    {
        currentLoop += 1;
        currentEncounterIndex = 0;

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
        patientResponse.Hide();
    }
}
