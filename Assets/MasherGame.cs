using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasherGame : Minigame
{
    [SerializeField] RectTransform outerRect;
    [SerializeField] RectTransform arrow;
    [SerializeField] RectTransform timerFill;

    [SerializeField] List<MedicalFormBox> medicalFormBoxes;
    [SerializeField] float yOffsetBetweenBoxes = 40;
    [SerializeField] Transform transformToScootch;
    [SerializeField] AnimationCurve scootchCurve;

    [SerializeField] float x = 0;
    [SerializeField] float step = 10;
    [SerializeField] float degradeSpeed = 50;
    [SerializeField] float minX = 0;
    [SerializeField] float maxX = 100;

    public float duration = 3f;
    [SerializeField] float elapsedTime = 0f;

    [SerializeField] int stage = 0;

    // [SerializeField] string[,] dialog = { { "He eventually", "He", "Boy" }, { "succumbed to", "was killed by", "got got by" }, { "his sickness", "the wending arm of fate", "a blunder in the operating room" } };


    // Start is called before the first frame update
    void Start()
    {
        foreach (var box in medicalFormBoxes)
            randomizeStage(box);
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

        var targetBox = medicalFormBoxes[stage];

        if (targetBox.UnderlineRange.IsInRange(percent))
        {
            part = dialog[stage, 0];
            points = 2;
        }
        else if (targetBox.HighlightRange.IsInRange(percent))
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
        updateBetweenStages();
        PauseForDuration(1f);
        StartCoroutine(Scootch(1f, transformToScootch));
        StartCoroutine(Scootch(1f, arrow, -1));
        stage += 1;

        if (stage >= dialog.GetLength(0))
        {
            // EndGame(result);
            this.enabled = false;
            stage = 0;
            return;
        }
    }

    void randomizeStage(MedicalFormBox box)
    {
        box.HighlightRange = new MinMaxRange(.35f);
        box.UnderlineRange = new MinMaxRange(.15f);
    }

    void updateBetweenStages()
    {
        x = minX;
        degradeSpeed = Random.Range(30, 60);
    }

    public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromRange = fromMax - fromMin;
        float toRange = toMax - toMin;
        float valueScaled = (value - fromMin) / fromRange;
        return toMin + (valueScaled * toRange);
    }

    IEnumerator Scootch(float duration, Transform thing, float direction = 1f)
    {
        var startTime = Time.time;
        var startPosition = thing.localPosition;
        var targetPosition = startPosition + new Vector3(0, yOffsetBetweenBoxes * direction, 0);
        float progress = 0;

        while (progress < 1f)
        {
            var lerpValue = scootchCurve.Evaluate(progress);
            thing.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpValue);
            yield return null;
            progress = (Time.time - startTime) / duration;
        }

        thing.localPosition = targetPosition;
    }
}
