using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;



public class SliderBar : Minigame
{
    [SerializeField] MedicalFormBox medicalFormBox;

    [SerializeField] GameObject SliderBarFrame;
    [SerializeField] GameObject SliderBarArrow;
    [SerializeField] GameObject SliderBarGood;
    [SerializeField] GameObject SliderBarMed;

    [SerializeField] TextBubble[] textBubbles;

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

    [SerializeField] string[,] dialog = { { "i'm sorry, your", "ur", "ya" }, { "husband", "mate", "boi" }, { "passed away peacefully", "ate it", "ded" } };

    [SerializeField] string response = "";



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

        medicalFormBox.HighlightRange = new MinMaxRange(.35f);
        medicalFormBox.UnderlineRange = new MinMaxRange(.15f);
    }

    void Update()
    {
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

    void evaluateArrow(float x)
    {
        var percent = (x - minX) / (maxX - minX);
        
        // Reset
        if (stage >= dialog.GetLength(0))
        {
            result.text = response;
            EndGame();
            this.enabled = false;
            stage = 0;
            response = "";
            return;
        }

        string part = "";
        int points = 0;

        if (medicalFormBox.UnderlineRange.IsInRange(percent))
        {
            points = 2;
            part = dialog[stage, 0];
            SongManager.Instance?.PlaySFX("success");
        }
        else if (medicalFormBox.HighlightRange.IsInRange(percent))
        {
            points = 1;
            part = dialog[stage, 1];
            SongManager.Instance?.PlaySFX("medium");
        }
        else
        {
            part = dialog[stage, 2];
            SongManager.Instance?.PlaySFX("negative");
        }

        updateResponse(part, points);
        // print(part);
        stage += 1;
    }

    void updateResponse(string s, int points)
    {
        response = response + " " + s;
        result.text = response;
        result.score += points;

        if (stage < textBubbles.Length)
        {
            var bubs = textBubbles[stage];
            bubs.Text = s;
            Color color = Color.white;
            if (points == 2)
                color = Color.green;
            else if (points == 0)
                color = Color.red;
            bubs.StartTyping(color);
        }
    }
}
