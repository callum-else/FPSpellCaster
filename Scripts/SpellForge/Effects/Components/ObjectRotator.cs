using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _constantRotationRate;
    [SerializeField, Range(0f, 1f)] private float _startRotationRandomness;

    private void Start()
    {
        if (_startRotationRandomness != 0f)
            transform.Rotate(_constantRotationRate * (Random.Range(0.1f, 1f + _startRotationRandomness) * 10f));
    }

    private void FixedUpdate()
    {
        transform.Rotate(_constantRotationRate * 10f * Time.fixedDeltaTime);
    }
}
