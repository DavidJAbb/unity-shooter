/* Simple camera shake script - attach to camera object */
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // How long the object should shake for
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    Vector3 originalPos;


    void Awake()
    {
		originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
			transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
			transform.localPosition = originalPos;
        }
    }
}
