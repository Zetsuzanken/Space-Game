using UnityEngine;

public class Health : MonoBehaviour
{
    public int lifeToAdd = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpaceShip player = collision.GetComponent<SpaceShip>();
        if (player != null)
        {
            player.AddLife(lifeToAdd);
            Destroy(gameObject);
        }
    }
}
