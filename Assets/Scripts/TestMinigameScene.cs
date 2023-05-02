using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class TestMinigameScene : Minigame
{

    [SerializeField] int stage = 0;

    // [SerializeField] string[][] dialog = { new[] { "i'm sorry, your", "ur", "ya" }, new[] { "husband", "mate", "boi" }, new[] { "passed away peacefully", "ate it", "ded" } };

    [SerializeField] TMPro.TextMeshProUGUI[] buttonTextMeshes;

    string[] buttonText = { "RROOOAAARRRR!", "GRRROOWWLLL!", "HUUUFFFFF!", "SNNAARRRRLL!", "YOOOWWWL!", "WHUUUUFFF" };

    [Header("Timer stuff")]
    [SerializeField] RectTransform timerFill;
    [SerializeField] RectTransform outerRect;
    public float duration = 3f;
    [SerializeField] float elapsedTime = 0f;

    void Start()
    {
    }

    public void Speak()
    {
        if (!IsPlaying)
            return;

        // They're mashing the button, they want to quit now
        if (stage >= 3)
        {
            CompleteGame();
            return;
        }

        // Pick a random response
        int idx = Random.Range(0, 3);
        string text = dialog[stage, idx];
        int score = 2 - idx;

        elapsedTime = 0;
        CompleteStage(stage, text, score);
        randomizeButtonText();
        PauseForDuration(.5f);

        stage++;
    }

    void randomizeButtonText()
    {
        // Shuffle array
        for (int i = 0; i < buttonText.Length; i++)
        {
            string temp = buttonText[i];
            int randomIndex = Random.Range(i, buttonText.Length);
            buttonText[i] = buttonText[randomIndex];
            buttonText[randomIndex] = temp;
        }
        for (int i = 0; i < buttonTextMeshes.Length; i++)
        {
            buttonTextMeshes[i].text = buttonText[i];
        }
    }

    void Update()
    {
        if (stage < 3)
        {
            updateTimer();
        }
    }

    void updateTimer()
    {
        var totalWidth = outerRect.rect.width;
        elapsedTime += Time.deltaTime;
        float lerpVal = Mathf.Clamp01(elapsedTime / duration);
        float rightEdge = Mathf.Lerp(0f, totalWidth, lerpVal);
        timerFill.offsetMax = new Vector2(-rightEdge, timerFill.offsetMax.y);

        if (lerpVal == 1)
        {
            // evaluateArrow(arrowX);
            Speak();
        }
    }

}

