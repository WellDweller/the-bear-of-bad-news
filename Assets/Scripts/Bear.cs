using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bear : MonoBehaviour
{
    [field:SerializeField] public UnityEvent OnLookAtClipboard { get; private set; }

    [field:SerializeField] public UnityEvent OnIdle { get; private set; }

    [field:SerializeField] public UnityEvent OnWalk { get; private set; }

    [SerializeField] float lookAtClipboardDelay;

    Animator animator;



    void Awake()
    {
        animator = GetComponentInChildren<Animator>();        
    }

    

    public void LookAtClipboard(float delay)
    {
        StartCoroutine(PlayAnimation("LookAtClipboard", delay, lookAtClipboardDelay, OnLookAtClipboard));
    }

    public void Idle(float delay)
    {
        StartCoroutine(PlayAnimation("Idle", delay, 0, OnIdle));
    }

    public void Walk(float delay)
    {
        StartCoroutine(PlayAnimation("Walk", delay, 0, OnWalk));
    }

    IEnumerator PlayAnimation(string animationName, float delay, float duration, UnityEvent eventToInvoke)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        animator.Play(animationName);

        if (duration > 0)
            yield return new WaitForSeconds(duration);

        eventToInvoke?.Invoke();
    }
}
