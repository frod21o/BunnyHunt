using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI; // Konieczne do pracy z UI, np. Text

public class GameController : MonoBehaviour
{
    public GameObject rabbitPrefab; // Prefab zająca
    public TilemapObjectPlacer tilemap;
    public List<Vector3> positions; // Lista pozycji
    public float minTime = 1f; // Minimalny czas pomiędzy pojawieniem się nowych zajęcy
    public float maxTime = 2f; // Maksymalny czas pomiędzy pojawieniem się nowych zajęcy
    public TMP_Text timerText; // Dołączony obiekt Text w Unity
    public float initialTime = 120f;
    private float timeRemaining = 120f; // Czas początkowy w sekundach
    public int initialRabbits = 20;
    private int rabbitsToCatch = 20;
    private int lvl = 1;
    public bool gameRunning = true;

    public GameObject endText;
    public Slider volumeSlider;

    private void Start()
    {
        tilemap.Generate();
        positions = tilemap.GetBurrowPositions();
        Restart();
    }

    public void Restart() {
        timeRemaining = initialTime; // Czas początkowy w sekundach
        rabbitsToCatch = initialRabbits;
        lvl = 1;
        gameRunning = true;
        endText.SetActive(false);
        StartCoroutine(SpawnRabbits());
    }

    private IEnumerator SpawnRabbits()
    {
        while (gameRunning)
        {
            // Losowanie czasu między kolejnymi zającami
            float spawnDelay = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(spawnDelay);

            // Tworzymy nowego zająca
            // Debug.Log("spawning a rabbit");
            GameObject newRabbit = Instantiate(rabbitPrefab, transform.position, Quaternion.identity);
            newRabbit.GetComponent<RabbitController>().gameController = this;

            // Losowanie dwóch różnych pozycji z listy
            Vector2 startPosition = positions[Random.Range(0, positions.Count)];
            Vector2 endPosition = positions[Random.Range(0, positions.Count)];

            // Upewniamy się, że pozycje startowa i końcowa są różne
            while (startPosition == endPosition)
            {
                endPosition = positions[Random.Range(0, positions.Count)];
            }

            // Przypisanie pozycji do zająca
            RabbitController rabbitController = newRabbit.GetComponent<RabbitController>();
            rabbitController.startPosition = startPosition;
            rabbitController.endPosition = endPosition;

            // Wywołanie metody Run() na zającu
            // rabbitController.Run();
        }
    }

    void Update()
    {
        if(rabbitsToCatch <= 0)
            NextLevel();
        // Sprawdzamy, czy timer jest włączony i czy czas nie jest ujemny
        if (gameRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Zmniejszamy czas w zależności od klatek
        }
        else if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            gameRunning = false; // Zatrzymujemy odliczanie
            OnTimerEnd(); // Wywołujemy funkcję po zakończeniu odliczania
        }
        UpdateDisplay(); // Aktualizujemy wyświetlany tekst timera
        GetComponent<AudioSource>().volume = volumeSlider.value;
    }

    public void RabbitCaught(){
        rabbitsToCatch--;
    }

    void NextLevel(){
        lvl++;
        timeRemaining = initialTime / lvl; // Czas początkowy w sekundach
        rabbitsToCatch = initialRabbits + 2 * lvl;
    }

    void UpdateDisplay()
    {
        // Wyświetlamy pozostały czas w sekundach, zaokrąglony do 2 miejsc po przecinku
        timerText.text = "Poziom " + lvl + "\nPozostały czas: " + timeRemaining.ToString("F2") + "\nKróliki do złapania: " + rabbitsToCatch;
    }

    void OnTimerEnd()
    {
        endText.SetActive(true);
        Debug.Log("Koniec gry");
    }
}
