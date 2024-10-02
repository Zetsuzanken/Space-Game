using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour
{
    public static SpaceShip Instance { get; private set; }

    public GameObject ParticlePrefab;

    public Bullet BulletPrefab;
    public float BulletDelay;
    private float NextSpawnTime;
    public int Lives = 3;
    public float InvincibilityDuration = 2.0f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private float survivalTime = 30.0f;
    private float timeSurvived = 0.0f;

    private Beam beam;

    private bool beamWasActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Lives = 3;
        NextSpawnTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        beam = GetComponentInChildren<Beam>();

        HUD.Instance.InitializeLives(Lives);
        HUD.Instance.SetLives(Lives);
    }

    void Update()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Bounds bounds = OrthographicBounds(Camera.main);

        mousepos = new Vector3(
            Mathf.Clamp(mousepos.x, bounds.min.x, bounds.max.x),
            Mathf.Clamp(mousepos.y, bounds.min.y, bounds.max.y),
            0);

        transform.position = mousepos;

        if (beam != null && beam.beamIsActive)
        {
            beamWasActive = true;
        }
        else
        {
            if (beamWasActive)
            {
                NextSpawnTime = Time.time + BulletDelay;
                beamWasActive = false;
            }

            if (Time.time >= NextSpawnTime)
            {
                Bullet bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity, null);
                NextSpawnTime = Time.time + BulletDelay;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (gameObject.activeSelf)
        {
            timeSurvived += Time.deltaTime;
            if (timeSurvived >= survivalTime)
            {
                HUD.Instance.ShowWinMessage();
                gameObject.SetActive(false);
            }
        }
    }

    //https://discussions.unity.com/t/calculating-2d-camera-bounds/77081/2
    public static Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible)
        {
            if (collision.gameObject.CompareTag("Beam"))
            {
                return;
            }

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                LoseLife();
                enemy.Destroy();
                return;
            }

            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null && bullet.IsEnemyBullet)
            {
                LoseLife();
                Destroy(bullet.gameObject);
                return;
            }
        }
    }

    public void AddLife(int amount)
    {
        Lives += amount;
        if (Lives > 3)
        {
            Lives = 3;
        }
        HUD.Instance.SetLives(Lives);
    }

    public void ReplenishEnergy(float amount)
    {
        if (beam != null)
        {
            beam.AddEnergy(amount);
        }
    }

    public void LoseLife()
    {
        Lives--;
        HUD.Instance.SetLives(Lives);

        if (Lives <= 0)
        {
            Instantiate(ParticlePrefab, transform.position, Quaternion.identity, null);
            gameObject.SetActive(false);
            HUD.Instance.ShowLoseMessage();
        }
        else
        {
            StartCoroutine(BecomeInvincible());
        }
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.4f);

        yield return new WaitForSeconds(InvincibilityDuration);

        isInvincible = false;
        spriteRenderer.color = originalColor;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Level 1")
        {
            SceneManager.LoadScene("Level 2");
        }
        else if (currentSceneName == "Level 2")
        {
            GameManager.Instance.ResetScore();
            SceneManager.LoadScene("Level 1");
        }
    }
}
