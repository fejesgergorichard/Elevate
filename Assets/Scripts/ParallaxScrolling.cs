using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public GameObject Camera;

    [SerializeField] 
    public float ParallaxEffect = 1f;

    private float _length;
    private float _startPosition;

    void Start()
    {
        _startPosition = transform.position.y;
        _length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void FixedUpdate()
    {
        // TODO add both directions maybe
        float temp = (Camera.transform.position.y * (1 - ParallaxEffect));
        float dist = (Camera.transform.position.y * ParallaxEffect);

        transform.position = new Vector3(transform.position.x, _startPosition + dist, transform.position.z);

        if (temp > _startPosition + _length)
        {
            _startPosition += _length;
        }
        // this might be unnecessary
        else if (temp < _startPosition - _length)
        {
            _startPosition -= _length;
        }
    }
}
