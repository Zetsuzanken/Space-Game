using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject ParticlePrefab;

    public int Health = 3;
    public Vector2 Movement;
    public float RotationSpeed = 200f;
    protected bool isMoving = false;

    protected bool isVisible = false;

    public virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if (isMoving)
        {
            Move();
            RotateTowardsMovementDirection();
        }
    }

    protected virtual void Move()
    {
        transform.position += (Vector3)Movement * Time.deltaTime;
    }

    public virtual void RotateTowardsMovementDirection()
    {
        if (Movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(Movement.y, Movement.x) * Mathf.Rad2Deg + 90;
            float currentAngle = transform.rotation.eulerAngles.z;
            float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, RotationSpeed * Time.deltaTime);
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnBecameVisible()
    {
        isMoving = true;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Instantiate(ParticlePrefab, transform.position, Quaternion.identity, null);
        GameObject.Destroy(gameObject);
        GameManager.Instance.AddScore(10);
        HUD.Instance.SetScore(GameManager.Instance.Score);
    }
}
