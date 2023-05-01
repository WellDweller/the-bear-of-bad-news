using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasherGame : Minigame
{
    [SerializeField] RectTransform outerRect;
    [SerializeField] RectTransform arrow;
    [SerializeField] RectTransform timerFill;
    [SerializeField] MedicalFormBox medicalFormBox;
    [SerializeField] TextBubble[] textBubbles;

    [SerializeField] float x = 0;
    [SerializeField] float step = 10;
    [SerializeField] float degradeSpeed = 50;
    [SerializeField] float minX = 0;
    [SerializeField] float maxX = 100;

    public float duration = 3f;
    [SerializeField] float elapsedTime = 0f;

    [SerializeField] int stage = 0;

    // [SerializeField] string[,] dialog = { { "He eventually", "He", "Boy" }, { "succumbed to", "was killed by", "got got by" }, { "his sickness", "the wending arm of fate", "a blunder in the operating room" } };
    [SerializeField] string response = "";


    // Start is called before the first frame update
    void Start()
    {
        randomizeStage();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.enabled) return;
        updateTimer();

        x -= Time.deltaTime * degradeSpeed;

        if (x <= minX)
        {
            x = minX;
        }
        else if (x >= maxX)
        {
            x = maxX;
        }

        if (InputSystem.InputActions.UI.Submit.WasPerformedThisFrame())
        {
            x += step;
        }

        var totalWidth = outerRect.rect.width;
        var halfWidth = totalWidth / 2;

        // TODO: need to lerp this?
        var remapped = Remap(x, minX, maxX, 0 - halfWidth, halfWidth);
        arrow.localPosition = new Vector3(remapped, arrow.localPosition.y, arrow.localPosition.z);
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
            evaluateArrow();
        }
    }

    void evaluateArrow()
    {
        var percent = x / 100;

        string part = "";
        int points = 0;
        if (medicalFormBox.UnderlineRange.IsInRange(percent))
        {
            part = dialog[stage, 0];
            points = 2;
        }
        else if (medicalFormBox.HighlightRange.IsInRange(percent))
        {
            points = 1;
            part = dialog[stage, 1];
        }
        else
        {
            part = dialog[stage, 2];
        }

        elapsedTime = 0;
        CompleteStage(stage, part, points);
        randomizeStage();
        PauseForDuration(1f);
        stage += 1;

        if (stage >= dialog.GetLength(0))
        {
            // EndGame(result);
            this.enabled = false;
            stage = 0;
            response = "";
            return;
        }
    }

    void randomizeStage()
    {
        x = minX;
        medicalFormBox.HighlightRange = new MinMaxRange(.35f);
        medicalFormBox.UnderlineRange = new MinMaxRange(.15f);
        degradeSpeed = Random.Range(25, 45);
    }

    public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromRange = fromMax - fromMin;
        float toRange = toMax - toMin;
        float valueScaled = (value - fromMin) / fromRange;
        return toMin + (valueScaled * toRange);
    }
}
