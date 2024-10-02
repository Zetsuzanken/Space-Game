using UnityEngine;

public class EnergyPowerUp : MonoBehaviour
{
    public float energyToAdd = 100f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpaceShip player = collision.GetComponent<SpaceShip>();
        if (player != null)
        {
            player.ReplenishEnergy(energyToAdd);
            Destroy(gameObject);
        }
    }
}
