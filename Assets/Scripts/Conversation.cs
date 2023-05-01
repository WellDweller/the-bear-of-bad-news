using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Conversation : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnConversationEnd { get; private set; }
    [field:SerializeField] public UnityEvent OnRoundEnd { get; private set; }

    [SerializeField] List<Encounter> encounters;

    [Header("Text bubbles for questions")]
    [SerializeField] TextBubble patientQuestion;
    [SerializeField] TextBubble bearAnswer;
    [SerializeField] TextBubble patientResponse;

    [Header("Other config")]
    [SerializeField] MinigameLoader loader;

    // which person are we talking to
    int encounterIndex = 0;
    // which round of the conversation are we in
    int roundIndex = 0;

    IEncounter currentEncounter;
    IEncounterRound currentEncounterRound;

    int currentLoop; // It would be better to hook this into some global game state keeping track of encounters instead
    bool gameOver;


    void Awake()
    {
        SetupStateForNextRound();
    }



    public void StartConversation()
    {
        patientQuestion.Reset();
        bearAnswer.Reset();
        patientResponse.Reset();

        patientQuestion.Text = currentEncounterRound.Question;
        patientQuestion.StartTyping();
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
        HideAll();

        roundIndex += 1;

        if (roundIndex >= currentEncounter.Rounds.Count)
        {
            roundIndex = 0;
            encounterIndex += 1;

            if (encounterIndex >= encounters.Count)
            {
                encounterIndex = 0;
                // game over, go to bear;
                OnConversationEnd?.Invoke();
            }
            else
            {
                // encounter over, walk to next encounter
                SetupStateForNextRound();
                OnRoundEnd?.Invoke();
            }
        }
        else
        {
            SetupStateForNextRound();
            StartConversation();
            // round over, start next conversation with same person

        }
    }

    public void HideAll()
    {
        patientQuestion.Hide();
        bearAnswer.Hide();
        patientResponse.Hide();
    }

    void SetupStateForNextRound()
    {
        currentEncounter = encounters[encounterIndex];
        currentEncounterRound = currentEncounter.Rounds[roundIndex];
    }
}
