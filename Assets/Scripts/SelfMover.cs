using UnityEngine;



[RequireComponent(typeof(Mover))]
public class SelfMover : MonoBehaviour
{
    [SerializeField] Vector3 direction;

    Mover mover;



    // Start is called before the first frame update
    void Awake()
    {
        mover = GetComponent<Mover>();
        mover.OnMove += Move;
    }

    void OnDestroy()
    {
        mover.OnMove -= Move;
    }



    void Move(float amount)
    {
        transform.position += (direction * amount);
    }
}
