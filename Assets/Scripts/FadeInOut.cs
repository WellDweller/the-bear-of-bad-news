using System.Collections;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnFadeInComplete { get; private set; }

    [field:SerializeField] public UnityEvent OnFadeOutComplete { get; private set; }
    
    CanvasGroup thingToFade;

    [Header("In")]
    [SerializeField] AnimationCurve inCurve;
    [SerializeField] float inFromAlpha;
    [SerializeField] float inTime;

    [Header("Out")]
    [SerializeField] AnimationCurve outCurve;
    [SerializeField] float outToAlpha;
    [SerializeField] float outTime;

    float startingAlpha;

    void Awake()
    {
        thingToFade = GetComponent<CanvasGroup>();
        startingAlpha = thingToFade.alpha;
        thingToFade.alpha = inFromAlpha;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoTransition(startingAlpha, inCurve, inTime, OnFadeInComplete));
    }



    public void FadeOut()
    {
        StartCoroutine(DoTransition(outToAlpha, outCurve, outTime, OnFadeOutComplete));
    }

    IEnumerator DoTransition(float targetAlpha, AnimationCurve animationCurve, float duration, UnityEvent unityAction)
    {
        float startingAlpha = thingToFade.alpha;
        float startTime = Time.time;
        float progress = 0f;

        while (progress < 1f)
        {
            var lerpValue = animationCurve.Evaluate(progress);
            thingToFade.alpha = Mathf.Lerp(startingAlpha, targetAlpha, lerpValue);
            progress = (Time.time - startTime) / duration;
            yield return null;
        }

        thingToFade.alpha = targetAlpha;

        unityAction?.Invoke();
    }
}
