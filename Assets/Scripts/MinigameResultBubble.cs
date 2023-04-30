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



    public void TypeMinigameResult(MinigameResult result)
    {
        if (result.text != null)
        {
            textBubble.Text = result.text;
        }

        textBubble.StartTyping();
    }
}
