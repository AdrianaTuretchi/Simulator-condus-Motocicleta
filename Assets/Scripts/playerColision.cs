using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Aici vom trage obiectul care are scriptul GameManager din scena
    public GameManager gameManager; 

    private void OnTriggerEnter(Collider other)
    {
        // Verificam daca obiectul de care ne-am izbit are tag-ul "Obstacle"
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Am lovit un obstacol!");

            // 1. Oprim controlul jucatorului (sa nu mai poata vira/accelera)
            if (GetComponent<KeyboardMotorcycle>()) GetComponent<KeyboardMotorcycle>().enabled = false;
            if (GetComponent<OculusMotorcycle>()) GetComponent<OculusMotorcycle>().enabled = false;

            // 2. Anuntam GameManager-ul ca s-a terminat jocul
            if (gameManager != null)
            {
                gameManager.EndGame();
            }
        }
    }
}

/*using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager; // Trage obiectul GameManager aici in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Oprim scripturile de miscare
            if (GetComponent<KeyboardMotorcycle>()) GetComponent<KeyboardMotorcycle>().enabled = false;
            if (GetComponent<OculusMotorcycle>()) GetComponent<OculusMotorcycle>().enabled = false;

            // Activam ecranul de Game Over
            if (gameManager != null)
            {
                gameManager.EndGame();
            }

            // Daca vrei sa se reseteze singur dupa 4 secunde:
            Invoke("AutoRestart", 4f);
        }
    }

    void AutoRestart()
    {
        gameManager.RestartGame();
    }
}*/