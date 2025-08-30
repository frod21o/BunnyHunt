using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;

public class RabbitController : MonoBehaviour
{
    public GameController gameController;
    private Animator animator;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    public Vector2 startPosition;  // Początkowa pozycja
    public Vector2 endPosition;    // Końcowa pozycja
    public float speed = 5f;       // Prędkość poruszania się zająca

    private bool isMoving = true; // Flaga sprawdzająca, czy zając się porusza

    public void Run()
    {
        // Ustawienie początkowej pozycji obiektu

        // Rozpoczęcie ruchu
        isMoving = true;
        gameObject.SetActive(true);
        spriteRenderer.sprite = defaultSprite;

        animator.SetBool("caught", false);
        animator.Rebind();
        transform.position = startPosition;
        col.enabled = true;
        // spriteRenderer.enabled = true;
    }

    void Update()
    {
        if (isMoving)
        {
            // Obliczanie wektora kierunku
            Vector2 direction = (endPosition - (Vector2)transform.position).normalized;

            // Ruch w kierunku końcowej pozycji
            transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);

            // Sprawdzenie, czy zając dotarł na miejsce
            if ((Vector2)transform.position == endPosition)
            {
                // Zając dotarł na miejsce, znikamy
                isMoving = false;
                Destroy(gameObject);
                gameObject.SetActive(false); // Znika obiekt (można to zastąpić inną akcją, jeśli chcesz np. wyłączyć zająca)
            }
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Run();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        isMoving = false;
        if (other.CompareTag("Player")) // Sprawdza, czy dotknął gracz
        {
            Collect();
        }
    }

    private void Collect()
    {
        gameController.RabbitCaught();
        animator.SetBool("caught", true); // Uruchamia animację
        col.enabled = false; // Wyłącza collider, żeby nie wykrywać więcej kolizji
    }

    public void OnAnimationEnd(){
        // gameObject.SetActive(false);
        // animator.SetBool("caught", false);
        // spriteRenderer.enabled = false;
        // Run();
        Destroy(gameObject);
    }

    // // Wywołaj na końcu animacji w Animation Event
    // public void ResetItem()
    // {
    //     animator.SetBool("caught", false);
    //     col.enabled = true;
    //     gameObject.SetActive(true);
    //     spriteRenderer.enabled = true; // Pokazuje ponownie obiekt
    // }
}