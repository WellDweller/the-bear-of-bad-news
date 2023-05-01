using System.Collections;
using UnityEngine;



public class Shaker : MonoBehaviour
{
    public bool IsShaking { get; private set; }

    [SerializeField] AnimationCurve curve;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeIntensity;
    

    public void Shake()
    {
        StartCoroutine(Shaking(shakeDuration, shakeIntensity));
    }

    IEnumerator Shaking(float duration, float intensity)
    {
        IsShaking = true;
        var startPosition = transform.position;
        float startTime = Time.time;
        float progress = 0f;

        while (progress < 1f)
        {
            float strength = curve.Evaluate(progress) * intensity;
            transform.position = startPosition + (Random.insideUnitSphere * strength);
            yield return null;
            progress = (Time.time - startTime) / duration;
        }

        transform.position = startPosition;
        IsShaking = false;
    }
}
