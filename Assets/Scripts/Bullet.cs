using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public int Damage = 1;
    public bool IsEnemyBullet = false;
    public Vector3 Direction;

    void Start()
    {
        if (IsEnemyBullet)
        {
            if (Direction == Vector3.zero)
            {
                Direction = (SpaceShip.Instance.transform.position - transform.position).normalized;
            }
        }
    }

    void Update()
    {
        if (IsEnemyBullet)
        {
            transform.position += Direction * Speed * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(0, Speed * Time.deltaTime, 0);
        }
    }

    private void OnBecameInvisible()
    {
        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnemyBullet)
        {
            return;
        }
        else
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
