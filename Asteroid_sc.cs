using UnityEngine;

public class Asteroid_sc : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed = 20.0f;

    [SerializeField]
    GameObject explosionPrefab;

    SpawnManager_sc spawnManager_Sc;

    void Start()
    {
        spawnManager_Sc = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager_sc>();
        if(spawnManager_Sc == null)
        {
            Debug.Log("Asteroid_sc: : Start, spawnManager_sc is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            spawnManager_Sc.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}
