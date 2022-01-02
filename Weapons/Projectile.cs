using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public LayerMask collisionMask;

    [Header("Projectile Stats")]
    public float speed = 20.0f;
    public float range = 2f;
    public float dropFactor = 2.5f;

    [Header("Damage")]
    public float damage = 1;

	private float _skinWidth = 0.1f;
    private bool _isActive = false;
    private float _aliveTime;

    // Distance travelled
    private Vector3 startPos;
    private float distanceTravelled;


    public void Init()
    {
        _isActive = true;
        startPos = transform.position;

        CheckCollisions(0); // Check initial collisions...
    }


	void Update()
	{
        if (_isActive)
        {
            _aliveTime += Time.deltaTime;

            if(_aliveTime > 5f)
            {
                _isActive = true;
                RemoveFromWorld();
                return;
            }

            distanceTravelled = Vector3.Distance(startPos, transform.position);
            Fly();
        }
	}
    
    
    void Fly()
    {
        float moveDistance = speed * Time.deltaTime;
        Vector3 direction = Vector3.forward;

        CheckCollisions(moveDistance);

        // If the distance is greater than the weapon's range
        if (distanceTravelled > range)
        {
            // Apply bullet drop
            direction.y -= dropFactor * Time.deltaTime;
        }

        // transform.rotation = Quaternion.LookRotation(direction); // Rotate in direction
        transform.Translate(direction * moveDistance); // Move by the move direction and distance
    }  


	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance + _skinWidth, collisionMask))
		{
            _isActive = false;

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Send the 'Hit' to the shot object...
                damageable.TakeHit(new Hit(damage, hit.point, transform.forward));
            }
            
            RemoveFromWorld();
		}
	}


    void RemoveFromWorld()
    {
        gameObject.SetActive(false);
    }
}
