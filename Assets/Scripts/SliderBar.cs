using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;



public class SliderBar : Minigame
{
    [SerializeField] MedicalFormBox medicalFormBox;

    [SerializeField] List<MedicalFormBox> medicalFormBoxes;
    [SerializeField] float yOffsetBetweenBoxes = 40;
    [SerializeField] Transform transformToScootch;
    [SerializeField] AnimationCurve scootchCurve;

    [SerializeField] GameObject SliderBarFrame;
    [SerializeField] GameObject SliderBarArrow;
    [SerializeField] GameObject SliderBarGood;
    [SerializeField] GameObject SliderBarMed;

    private float minX;
    private float maxX;

    private RectTransform SliderArrowTransform;


    [SerializeField] float lerpDuration = 2;

    private float timeElapsed;
    private float arrowX = 0f;

    private float startValue;
    private float endValue;
    [SerializeField] int direction = 1;

    [Header("Target Ranges")]
    [SerializeField] float goodMin;
    [SerializeField] float goodMax;
    [SerializeField] float medMin;
    [SerializeField] float medMax;

    [SerializeField] int stage = 0;

    // [SerializeField] string[,] dialog = { { "i'm sorry, your", "ur", "ya" }, { "husband", "mate", "boi" }, { "passed away peacefully", "ate it", "ded" } };

    [SerializeField] string response = "";

    [Header("Timer stuff")]
    [SerializeField] RectTransform timerFill;
    [SerializeField] RectTransform outerRect;
    public float duration = 3f;
    [SerializeField] float elapsedTime = 0f;

    void Start()
    {
        float frameWidth = SliderBarFrame.GetComponent<RectTransform>().rect.width;
        minX = 0 - frameWidth / 2;
        maxX = frameWidth / 2;
        SliderArrowTransform = SliderBarArrow.GetComponent<RectTransform>();
        startValue = minX;
        endValue = maxX;

        // float goodWidth = SliderBarGood.GetComponent<RectTransform>().rect.width;
        // goodMin = 0 - goodWidth / 2;
        // goodMax = goodWidth / 2;

        // float medWidth = SliderBarMed.GetComponent<RectTransform>().rect.width;
        // medMin = 0 - medWidth / 2;
        // medMax = medWidth / 2;

        lerpDuration = Random.Range(0.3f, 0.7f);


        foreach (var box in medicalFormBoxes)
            randomizeStage(box);
    }

    void randomizeStage(MedicalFormBox box)
    {
        box.HighlightRange = new MinMaxRange(.35f);
        box.UnderlineRange = new MinMaxRange(.15f);
    }

    void Update()
    {
        if (!IsPlaying)
            return;

        if (stage == dialog.GetLength(0))
        {
            return;
        }
        updateTimer();
        if (timeElapsed < lerpDuration)
        {
            arrowX = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            if (direction == 1)
            {
                startValue = maxX;
                endValue = minX;
                direction = -1;
            }
            else
            {
                startValue = minX;
                endValue = maxX;
                direction = 1;
            }
            timeElapsed = 0;
        }
        SliderArrowTransform.localPosition = new Vector3(arrowX, SliderArrowTransform.localPosition.y, SliderArrowTransform.localPosition.z);

        if (InputSystem.InputActions.UI.Submit.WasPerformedThisFrame())
        {
            evaluateArrow(arrowX);
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
            evaluateArrow(arrowX);
        }
    }

    void evaluateArrow(float x)
    {
        var percent = (x - minX) / (maxX - minX);

        // Reset
        if (stage >= dialog.GetLength(0))
        {
            result.text = response;
            CompleteGame();
            this.enabled = false;
            stage = 0;
            response = "";
            return;
        }

        string part = "";
        int points = 0;

        var targetBox = medicalFormBoxes[stage];

        if (targetBox.UnderlineRange.IsInRange(percent))
        {
            points = 2;
            part = dialog[stage, 0];
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
        PauseForDuration(1f);
        StartCoroutine(Scootch(1f));

        // print(part);
        stage += 1;
    }

    IEnumerator Scootch(float duration)
    {
        var startTime = Time.time;
        var startPosition = transformToScootch.localPosition;
        var targetPosition = startPosition + new Vector3(0, yOffsetBetweenBoxes, 0);
        float progress = 0;

        while (progress < 1f)
        {
            var lerpValue = scootchCurve.Evaluate(progress);
            transformToScootch.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpValue);
            yield return null;
            progress = (Time.time - startTime) / duration;
        }

        transformToScootch.localPosition = targetPosition;
    }
}
