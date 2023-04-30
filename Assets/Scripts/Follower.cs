using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] Transform follow;

    Vector3 initialOffset;

    void Awake()
    {
        initialOffset = transform.position - follow.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.position + initialOffset;
    }
}
