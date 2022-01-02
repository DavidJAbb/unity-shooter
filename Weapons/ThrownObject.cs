using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    private AudioSource _audioSource;
    private int _collisions;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision)
    {
        _collisions++;
        Debug.Log($"Number of collisions: {_collisions}");

        // Play a sound if the colliding objects had a big impact.
        if (collision.relativeVelocity.magnitude > 2)
            _audioSource.Play();
    }

}
