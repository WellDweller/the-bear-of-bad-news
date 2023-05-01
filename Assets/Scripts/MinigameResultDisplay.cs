using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinigameResultDisplay : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnComplete;

    [SerializeField] TextBubble[] textBubbles;

    public void HandleMinigameStageResult(MinigameResult result)
    {
        if (result.stage < 0)
            throw new System.Exception("Invalid result stage");
        if (result.stage >= textBubbles.Length)
            throw new System.Exception($"No textbubble configured for stage {result.stage}");

        var bubs = textBubbles[result.stage];
        bubs.Text = result.text;
        Color color = Color.white;
        if (result.score == 2)
        {
            SongManager.Instance?.PlaySFX("success");
            // Green
            color = new Color(150 / 255f, 200 / 255f, 100 / 255f);
        }
        else if (result.score == 1)
        {
            SongManager.Instance?.PlaySFX("medium");
            // Yellow
            color = new Color(227 / 255f, 218 / 255f, 87 / 255f);
        }
        else if (result.score == 0)
        {
            SongManager.Instance?.PlaySFX("negative");
            // Red
            color = new Color(250 / 255f, 130 / 255f, 110 / 255f);
        }

        bubs.StartTyping(color);
    }

    public void Complete()
    {
        OnComplete?.Invoke();
    }
}
