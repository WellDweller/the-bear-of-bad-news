using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggler : MonoBehaviour
{
    [SerializeField] float wiggleIntensity;
    [SerializeField] Vector2 scale;
    [SerializeField] Vector2 offset;
    
    Vector3 realPosition;

    // Start is called before the first frame update
    void Start()
    {
        realPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (wiggleIntensity == 0)
        {
            transform.localPosition = realPosition;
        }

        Vector3 wiggleOffset = new() {
            x = Mathf.Sin(Time.time * scale.x + offset.x) * wiggleIntensity,
            y = Mathf.Cos(Time.time * scale.y + offset.y) * wiggleIntensity,
        };

        transform.localPosition = realPosition + wiggleOffset;
    }

    public void SetWiggleIntensity(float val)
    {
        wiggleIntensity = val;
    }
}
