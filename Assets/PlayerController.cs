using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Actions controls;
    private AudioSource audioSource; // Zmienna do przechowywania AudioSource
    public AudioClip walkingSound; // Dźwięk chodzenia
    private bool isWalking = false; // Flaga, czy postać się porusza

    void Awake()
    {
        controls = new Actions();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Pobieramy komponent AudioSource
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = Vector2.zero;
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        // Odbijanie lustrzane postaci
        if (movement.x > 0)
            transform.localScale = new Vector3(0.4f, 0.4f, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-0.4f, 0.4f, 1);

        // Odtwarzanie lub zatrzymywanie dźwięku chodzenia
        if (movement.x != 0 && !isWalking) // Jeżeli postać zaczyna chodzić i jeszcze nie gra dźwięk
        {
            StartWalkingSound();
        }
        else if (movement.x == 0 && isWalking) // Jeżeli postać przestaje się poruszać
        {
            StopWalkingSound();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed; // Ustawienie prędkości postaci
    }

    // Funkcja wywoływana, gdy postać zaczyna chodzić
    private void StartWalkingSound()
    {
        isWalking = true;
        audioSource.clip = walkingSound;
        audioSource.time = 4;
        audioSource.loop = true; // Zapętlamy dźwięk
        audioSource.Play(); // Rozpoczynamy odtwarzanie dźwięku
    }

    // Funkcja wywoływana, gdy postać przestaje chodzić
    private void StopWalkingSound()
    {
        isWalking = false;
        audioSource.Stop(); // Zatrzymujemy dźwięk
    }
}
