/* Attach to shell rigidbody object - launches rigidbody with randomised force */
using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
	public float forceMin;
	public float forceMax;
	Rigidbody myRigidbody;

	// Use this for initialization
	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody>();
		float force = Random.Range(forceMin, forceMax);
		myRigidbody.AddForce(transform.forward * force);
		myRigidbody.AddTorque(Random.insideUnitSphere * force);
	}
}
