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

    void Start()
    {
    }

    public void Speak()
    {
        // They're mashing the button, they want to quit now
        if (stage >= 3)
        {
            EndGame(this.result);
            return;
        }

        // Pick a random response
        int idx = Random.Range(0, 3);
        string text = dialog[stage, idx];
        int score = 2 - idx;

        CompleteStage(stage, text, score);

        // Check whether we're done and prepare the result text
        if (stage == dialog.GetLength(0) - 1)
        {
            this.enabled = false;
            stage = 0;
        }

        randomizeButtonText();

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
    }

}

