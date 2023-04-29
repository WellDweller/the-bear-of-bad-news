using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;



public class SliderBar : Minigame
{
    [SerializeField] GameObject SliderBarFrame;
    [SerializeField] GameObject SliderBarArrow;
    [SerializeField] GameObject SliderBarGood;
    [SerializeField] GameObject SliderBarMed;
    [SerializeField] TMPro.TextMeshProUGUI textMesh;

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

        float goodWidth = SliderBarGood.GetComponent<RectTransform>().rect.width;
        goodMin = 0 - goodWidth / 2;
        goodMax = goodWidth / 2;

        float medWidth = SliderBarMed.GetComponent<RectTransform>().rect.width;
        medMin = 0 - medWidth / 2;
        medMax = medWidth / 2;
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

        // Reset
        if (stage >= dialog.GetLength(0))
        {
            EndGame(new MinigameResult { text = response });
            this.enabled = false;
            stage = 0;
            response = "";
            return;
        }

        string part = "";
        if (x >= goodMin && x <= goodMax)
        {
            part = dialog[stage, 0];
        }
        else if (x >= medMin && x <= medMax)
        {
            part = dialog[stage, 1];
        }
        else // badboi
        {
            part = dialog[stage, 2];
        }
        updateResponse(part);
        // print(part);
        stage += 1;
    }

    void updateResponse(string s)
    {
        response = response + " " + s;
        // print(response);
        textMesh.text = response;
        // textMeshPro.SetText(response);
    }
}
