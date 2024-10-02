using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    SineWave,
    Circular,
    ZigZag
}

public class Complex : Enemy
{
    public MovementType movementType;
    public float Frequency = 1f;
    public float Amplitude = 1f;
    private float elapsedTime = 0f;
    private Vector3 startPosition;
    private Vector3 previousPosition;

    public Bullet BulletPrefab;
    public float FireRate = 0.5f;
    private float nextFireTime = 0f;
    public float BulletSpeed = 5f;

    public override void Start()
    {
        base.Start();
        startPosition = transform.position;
        previousPosition = transform.position;
    }

    protected override void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;

            Move();

            Vector2 movementDirection = (transform.position - previousPosition).normalized;

            RotateTowardsMovementDirection(movementDirection);

            if (isVisible && Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / FireRate;
            }

            previousPosition = transform.position;
        }
    }

    protected override void Move()
    {
        switch (movementType)
        {
            case MovementType.SineWave:
                MoveInSineWave();
                break;
            case MovementType.Circular:
                MoveInCircle();
                break;
            case MovementType.ZigZag:
                MoveInZigZag();
                break;
        }
    }

    private void MoveInSineWave()
    {
        float yMovement = Movement.y * Time.deltaTime;

        float xOffset = Amplitude * Mathf.Sin(Frequency * elapsedTime);

        transform.position = new Vector3(startPosition.x + xOffset, transform.position.y + yMovement, 0f);
    }

    private void MoveInCircle()
    {
        float x = startPosition.x + Amplitude * Mathf.Cos(Frequency * elapsedTime);
        float y = startPosition.y + Amplitude * Mathf.Sin(Frequency * elapsedTime);

        y -= Mathf.Abs(Movement.y * Time.deltaTime);

        transform.position = new Vector3(x, y, 0f);
    }

    private void MoveInZigZag()
    {
        float yMovement = Movement.y * Time.deltaTime;

        float xOffset = Amplitude * (Mathf.PingPong(Frequency * elapsedTime, 1f) - 0.5f);

        transform.position = new Vector3(startPosition.x + xOffset, transform.position.y + yMovement, 0f);
    }

    private void RotateTowardsMovementDirection(Vector2 movementDirection)
    {
        if (movementDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg + 90f;
            float currentAngle = transform.rotation.eulerAngles.z;

            float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, (RotationSpeed * 2f) * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bullet.Speed = BulletSpeed;
        bullet.IsEnemyBullet = true;

        Vector3 direction = -transform.up;

        bullet.Direction = direction.normalized;

        bullet.transform.rotation = transform.rotation;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        isMoving = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        isMoving = false;
    }
}
