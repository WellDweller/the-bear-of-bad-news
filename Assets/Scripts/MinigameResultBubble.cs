using UnityEngine;



[RequireComponent(typeof(TextBubble))]
public class MinigameResultBubble : MonoBehaviour
{
    [SerializeField] MinigameLoader loader;

    TextBubble textBubble;



    void Awake()
    {
        textBubble = GetComponent<TextBubble>();
    }



    public void TypeMinigameResult()
    {
        textBubble.Text = loader.LastResult.text;
        textBubble.StartTyping();
    }
}
