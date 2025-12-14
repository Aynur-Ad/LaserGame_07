using UnityEngine;

public class Enemy_sc : MonoBehaviour
{
    [SerializeField]
    private int speed = 4;

    Player_sc player_sc;

    Animator animator;

    AudioSource audioSource;


    // Start is called once
    private void Start()
    {
        /* bunu ontriggerenter'da da yazabilirdik(else if kýsmý) ama öyle olsaydý her çarpýþmada çaðrýlýrdý
        ve unity gereksiz yere meþgul olurdu */
        player_sc = GameObject.Find("Player").GetComponent<Player_sc>();

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            Debug.Log("Enemy_sc::Start");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (this.transform.position.y < -5.5f)
        {
            // Destroy(this.gameObject);
            float randomX = Random.Range(-9.5f, 9.5f);
            float y = 7.4f;
            this.transform.position = new Vector3(randomX, y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Çarpýþma: " + other.tag);

        if (other.tag == "Player")
        { 
            // Player'in canini bir eksilt
            // Player_sc player_sc = other.transform.GetComponent<Player_sc>(); - startýn içinde çaðrýldýðý için gerek yok artýk
            if(player_sc != null)
            {
                player_sc.Damage();
            }
            // patlama animasyonu ve sonrasýnda kendini yok et
            animator.SetTrigger("OnEnemyDeath");
            // hýzý sýfýrla
            speed = 0;
            // patlama sesi
            audioSource.Play();
            Destroy(this.gameObject, 2.3f);
        }
        else if(other.tag == "Laser")
        {
            // Çarpýþtýðý nesneyi yok et
            Destroy(other.gameObject);
            
            // Puan arttýr
            if (player_sc != null)
            {
                player_sc.AddScore(10);
            }

            animator.SetTrigger("OnEnemyDeath");
            // hýzý sýfýrla
            speed = 0;
            // patlama sesi
            audioSource.Play();
            Destroy(this.gameObject, 2.3f);
        }
    }
}
