/* Attach to light for flicker effect - useful for candles, neon lights etc. */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;

    private Light _light;
    private float[] _smoothing = new float[20];


    void Start()
    {
        _light = GetComponent<Light>();

        for(int i = 0; i < _smoothing.Length; i++)
        {
            _smoothing[i] = 0.0f;
        }
    }


    void Update()
    {
        float sum = 0.0f;

        for (int i = 1; i < _smoothing.Length; i++)
        {
            _smoothing[i - 1] = _smoothing[i];
            sum += _smoothing[i - 1];
        }

        _smoothing[_smoothing.Length - 1] = Random.Range(minIntensity, maxIntensity);
        sum += _smoothing[_smoothing.Length - 1];

        _light.intensity = sum / _smoothing.Length;
    }
}
