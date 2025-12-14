using System.Collections;
using UnityEngine;

public class Player_sc : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;

    private float speedMultiplier = 2;

    private float nextFire = 0;

    [SerializeField]
    private float fireRate = 0.25f;

    [SerializeField]
    private bool isTripleShotActive = false;

    // [SerializeField]
   // private bool isSpeedBonusActive = false;

    [SerializeField]
    private bool isShieldBonusActive = false;

    [SerializeField]
    private GameObject laserPrefab;

    [SerializeField]
    private GameObject tripleLaserPrefab;

    [SerializeField]
    private GameObject shieldVisualizer;

    [SerializeField]
    GameObject rightEngine, leftEngine;

    [SerializeField]
    AudioClip laserSoundClip;

    AudioSource audioSource;

    [SerializeField]
    UIManager_sc uiManager_sc;

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        uiManager_sc = GameObject.Find("Canvas").GetComponent<UIManager_sc>();

        if(uiManager_sc == null)
        {
            Debug.LogError("Player_sc::Start !Hata - uiManager_sc NULL deðerine sahip!");
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("Player_sc::Start !Hata - audioSource NULL deðerine sahip!");
        }
        else
        {
            audioSource.clip = laserSoundClip;
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            FireLaser();
        }
    }

    void FireLaser()
    {
        if (!isTripleShotActive)
        {
            Instantiate(laserPrefab, (this.transform.position + new Vector3(0, 1.05f, 0)), Quaternion.identity);
        }
        else
        {
            Instantiate(tripleLaserPrefab, this.transform.position, Quaternion.identity);
        }

        // Play the laser audio clip
        audioSource.Play();


    }

    // Update is called once per frame
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * speed * Time.deltaTime);

        transform.position=new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f,0),0);

        /* if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        */

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        // koruma kalkaný aktifse can azalmiyor ama kalkan pasif duruma donuyor
        if (isShieldBonusActive)
        {
            isShieldBonusActive = false;
            shieldVisualizer.SetActive(false);
            return;
        }
        // kalkan aktif degilse can bi azalsin
        lives--;

        if(lives == 2)
        {
            rightEngine.SetActive(true);
        }
        else if(lives == 1)
        {
            leftEngine.SetActive(true);
        }

        if (uiManager_sc != null)
        {
            uiManager_sc.UpdateLives(lives);
        }

        if (lives == 0)
        {
            SpawnManager_sc spawnManager_sc = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager_sc>();

            if(spawnManager_sc != null)
            {
                spawnManager_sc.OnPlayerDeath();
            }
            else
            {
                Debug.LogError("Player_sc::Damage spawnManager_sc  is NULL");
            }

            Destroy(this.gameObject);
        }
            
    }

    public void AddScore(int points)
    {
        score += points;
        uiManager_sc.UpdateScore(score);
    }

    public void TripleShotActive()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotCancelRoutine());
    }

    public void SpeedBonusActive()
    {
        // isSpeedBonusActive = true;
        speed *= speedMultiplier;
        StartCoroutine(SpeedBonusCancelRoutine());
    }

    public void ShieldBonusActive()
    {
        isShieldBonusActive = true;
        shieldVisualizer.SetActive(true);
    }

    IEnumerator TripleShotCancelRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isTripleShotActive = false;
    }

    IEnumerator SpeedBonusCancelRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        // isSpeedBonusActive = false;
        speed /= speedMultiplier;
    }
  

}
