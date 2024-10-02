using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Bullet BulletPrefab;
    public float FireRate = 0.5f;
    private float nextFireTime = 0f;
    public float BulletSpeed = 5f;

    private Transform player;
    private bool isVisible = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isVisible && player != null)
        {
            RotateTowardsPlayer();

            if (Time.time > nextFireTime)
            {
                ShootAtPlayer();
                nextFireTime = Time.time + 1f / FireRate;
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ShootAtPlayer()
    {
        if (SpaceShip.Instance != null)
        {
            Vector3 direction = (SpaceShip.Instance.transform.position - transform.position).normalized;
            Bullet bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            bullet.Speed = BulletSpeed;
            bullet.IsEnemyBullet = true;
            bullet.Direction = direction;
            bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
    }

    void OnBecameVisible()
    {
        isVisible = true;
    }

    void OnBecameInvisible()
    {
        isVisible = false;
    }
}
