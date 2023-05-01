using UnityEngine;



public class Person : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        Idle();
    }


    public void Weep()
    {
        animator.Play("Weep");
    }

    public void Idle()
    {
        animator.Play("Idle");
    }
}
