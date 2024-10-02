using UnityEngine;

public class Beam : MonoBehaviour
{
    public bool beamIsActive = false;

    private Camera mainCamera;

    public float maxEnergy = 100f;
    private float currentEnergy;

    public float energyDepletionRate = 50f;
    public float energyRechargeRate = 50f;

    public float rechargeThreshold = 25f;

    void Start()
    {
        mainCamera = Camera.main;
        currentEnergy = maxEnergy;
        UpdateEnergyBar();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ActivateBeam();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DeactivateBeam();
        }

        if (beamIsActive)
        {
            currentEnergy -= energyDepletionRate * Time.deltaTime;
            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                DeactivateBeam();
            }
        }
        else
        {
            if (currentEnergy < maxEnergy)
            {
                currentEnergy += energyRechargeRate * Time.deltaTime;
                if (currentEnergy > maxEnergy)
                {
                    currentEnergy = maxEnergy;
                }
            }
        }

        UpdateEnergyBar();
    }

    void ActivateBeam()
    {
        if (currentEnergy >= rechargeThreshold)
        {
            beamIsActive = true;
            GetComponent<Renderer>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void DeactivateBeam()
    {
        beamIsActive = false;
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!beamIsActive)
            return;

        if (collision.CompareTag("Player"))
        {
            return;
        }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && IsEnemyVisible(enemy))
        {
            enemy.Destroy();
        }
    }
    
    private bool IsEnemyVisible(Enemy enemy)
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);
        return viewportPoint.z > 0 && viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }

        UpdateEnergyBar();
    }

    private void UpdateEnergyBar()
    {
        float normalizedEnergy = currentEnergy / maxEnergy;
        HUD.Instance.SetEnergy(normalizedEnergy);

        if (currentEnergy < rechargeThreshold)
        {
            HUD.Instance.SetEnergyBarOpacity(0.5f); // 50% opacity
        }
        else
        {
            HUD.Instance.SetEnergyBarOpacity(1f); // Full opacity
        }
    }
}
