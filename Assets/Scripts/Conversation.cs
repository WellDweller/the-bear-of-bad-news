using UnityEngine;
using UnityEngine.Events;



public class Conversation : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnConversationEnd { get; private set; }
    [field:SerializeField] public UnityEvent OnRoundEnd { get; private set; }

    [Header("Text bubbles for questions")]
    [SerializeField] TextBubble patientQuestion;
    [SerializeField] TextBubble bearAnswer;
    [SerializeField] TextBubble patientResponse;

    [Header("Other config")]
    [SerializeField] MinigameLoader loader;

    [SerializeField, Min(1)] int loops;

    int currentLoop;
    bool gameOver;


    public void StartConversation()
    {
        patientQuestion.Reset();
        bearAnswer.Reset();
        patientResponse.Reset();

        patientQuestion.StartTyping();
    }

    public void GameOver()
    {
        HideAll();
        gameOver = true;
    }

    public void EndConversation()
    {
        currentLoop += 1;

        if (gameOver || currentLoop >= loops)
            OnConversationEnd?.Invoke();
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
