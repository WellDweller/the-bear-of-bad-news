using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class TextBubble : MonoBehaviour
{
    public string Text
    {
        get => text;
        set
        {
            if (isTyping)
                throw new System.Exception("Can't change text when typing in progress!");
            text = value;
        }
    }

    // unity events allow wiring up stuff in the editor
    [field:SerializeField] public UnityEvent OnDialogStart { get; private set; }

    [field:SerializeField] public UnityEvent OnDialogEnd { get; private set; }

    [SerializeField] bool startVisible;
    [SerializeField] bool startTypingImmediately;
    [SerializeField] float delayBefore;
    [SerializeField] float delayBetweenCharacters;
    [SerializeField] float delayAfter;
    [SerializeField, TextArea] string text;
    [SerializeField] TMPro.TextMeshProUGUI textMesh;

    Image bubbleImage;


    CanvasGroup canvasGroup;

    bool isTyping;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        bubbleImage = GetComponent<Image>();
        Reset();
    }



    public void Reset()
    {
        if (!startVisible)
            textMesh.text = "";

        if (startTypingImmediately)
            StartTyping();
        else if (!startVisible)
            Hide();
        else
            Show(Color.white);
    }

    public void StartTyping()
    {
        StartTyping(Color.white);
    }

    public void StartTyping(Color color)
    {
        if (isTyping)
            throw new System.Exception("Already typing!");

        Show(color);
        StartCoroutine(Typewrite(text));
    }

    public void Show(Color color)
    {
        bubbleImage.color = color;
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }

    IEnumerator Typewrite(string text)
    {
        OnDialogStart?.Invoke();
        isTyping = true;
        StringBuilder sb = new(text.Length);

        if (delayBefore > 0f)
            yield return new WaitForSeconds(delayBefore);

        SongManager.Instance?.PlayTyping();
        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            textMesh.text = sb.ToString();
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
        SongManager.Instance?.PauseTyping();

        if (delayBefore > 0f)
            yield return new WaitForSeconds(delayAfter);

        isTyping = false;
        OnDialogEnd?.Invoke();
    }
}
