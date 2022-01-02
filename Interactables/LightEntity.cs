using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Light class - to start a light on or off just set the light component in the editor. Enabled is on, disabled is off.
public class LightEntity : MonoBehaviour
{
    public enum LightType
    {
        ON_OR_OFF,
        PULSE
    }
    public LightType _type;

    // PULSING LIGHT //
    public AnimationCurve curve;
    public float period = 2.0f;
    private float _timer;

    // General Private Vars
    private float _maxIntensity;
    private Light _light;

    // Would be nice to have some presets here
    // SLOW PULSE
    // FLICKER etc. - see Half Life


    // Use this for initialization
    void Start()
    {
        _light = GetComponent<Light>();
        _maxIntensity = _light.intensity;
    }


    // Update is called once per frame
    void Update()
    {
        if (_type == LightType.PULSE && _light.enabled)
        {
            Pulse();
        }
    }

    // LIGHT CONTROLS
    public void TurnOn()
    {
        _light.enabled = true;
    }

    public void TurnOff()
    {
        _light.enabled = false;
        _timer = 0;
    }

    public void Toggle()
    {
        if (_light.enabled)
        {
            TurnOff();
        }
        else if (!_light.enabled)
        {
            TurnOn();
        }
    }

    // LIGHT TYPES

    // Lerp to the max value then snap back to zero.
    void Pulse()
    {
        // Increment the timer
        _timer += Time.deltaTime;

        // Reset the timer if necessary
        if (_timer > period)
            _timer = 0;

        // Get a value from the curve
        float curvedValue = curve.Evaluate(_timer / period);

        // Set the intensity
        _light.intensity = curvedValue * _maxIntensity;
    }
}