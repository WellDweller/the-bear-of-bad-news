using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlideInOut : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnSlideInComplete { get; private set; }

    [field:SerializeField] public UnityEvent OnSlideOutComplete { get; private set; }
    
    [Header("In")]
    [SerializeField] Vector3 inDirection;
    [SerializeField] AnimationCurve inCurve;
    [SerializeField] float inDistance;
    [SerializeField] float inTime;

    [Header("Out")]
    [SerializeField] Vector3 outDirection;
    [SerializeField] AnimationCurve outCurve;
    [SerializeField] float outDistance;
    [SerializeField] float outTime;

    Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.localPosition;
        transform.localPosition -= inDirection * inDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoTransition(startPosition, inCurve, inTime, OnSlideInComplete));
    }



    public void SlideOut()
    {
        var targetPosition = startPosition + (outDirection * outDistance);
        StartCoroutine(DoTransition(targetPosition, outCurve, outTime, OnSlideOutComplete));
    }

    public void Reset()
    {
        transform.localPosition = startPosition;
    }

    IEnumerator DoTransition(Vector3 targetPosition, AnimationCurve animationCurve, float duration, UnityEvent unityAction)
    {
        Vector3 startPosition = transform.localPosition;
        float startTime = Time.time;
        float progress = 0f;

        while (progress < 1f)
        {
            var lerpValue = animationCurve.Evaluate(progress);
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpValue);
            progress = (Time.time - startTime) / duration;
            yield return null;
        }

        transform.localPosition = targetPosition;

        unityAction?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
