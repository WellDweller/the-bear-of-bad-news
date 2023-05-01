using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScalePopper : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnPop { get; private set; }

    [SerializeField] float popMagnitude;
    [SerializeField] AnimationCurve popCurve;
    [SerializeField] float popTime;

    public void PopIt()
    {
        StartCoroutine(DoTransition(popMagnitude, popCurve, popTime, OnPop));
    }

    IEnumerator DoTransition(float magnitude, AnimationCurve animationCurve, float duration, UnityEvent unityAction)
    {
        Vector3 startingScale = transform.localScale;
        Vector3 targetScale = startingScale * magnitude;

        float startTime = Time.time;
        float progress = 0f;

        while (progress < 1f)
        {
            var lerpValue = animationCurve.Evaluate(progress);
            transform.localScale = Vector3.Lerp(startingScale, targetScale, lerpValue);
            progress = (Time.time - startTime) / duration;
            yield return null;
        }

        transform.localScale = startingScale;

        unityAction?.Invoke();
    }
}
