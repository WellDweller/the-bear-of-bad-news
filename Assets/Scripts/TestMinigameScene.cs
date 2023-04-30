using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class TestMinigameScene : Minigame
{

    [SerializeField] int stage = 0;

    [SerializeField] string[][] dialog = { new[] { "i'm sorry, your", "ur", "ya" }, new[] { "husband", "mate", "boi" }, new[] { "passed away peacefully", "ate it", "ded" } };

    [SerializeField] List<string> response = new List<string>();

    [SerializeField] TextBubble[] textBubbles;

    void Start()
    {
    }

    public void Speak()
    {
        // They're mashing the button, they want to quit now
        if (stage >= 3) {
            EndGame(this.result);
            return;
        }

        // Pick a random response
        int idx = Random.Range(0, dialog[stage].Length);
        string text = dialog[stage][idx];
        response.Add(text);

        // Play sounds and choose color
        Color color = Color.white;
        if (idx == 0)
        {
            // Green
            color = new Color(150 / 255f, 200 / 255f, 100 / 255f);
            SongManager.Instance?.PlaySFX("success");
        } else if (idx == 1)
        {
            SongManager.Instance?.PlaySFX("medium");
        } else if (idx == 2)
        {
            // Red
            color = new Color(250 / 255f, 130 / 255f, 110 / 255f);
            SongManager.Instance?.PlaySFX("negative");
        }

        // Update text bubbles
        var bubs = textBubbles[stage];
        bubs.Text = text;
        bubs.StartTyping(color);


        // Check whether we're done and prepare the result text
        if (stage == 2)
        {
            string joined = string.Join(" ", response);
            this.result = new MinigameResult { text = joined };
            this.enabled = false;
            stage = 0;
            response = new List<string>();
        }

        stage++;
    }

    void Update()
    {
    }

}

