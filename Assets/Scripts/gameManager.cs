/*using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    public GameObject gameOverPanel;    // Trage Panel-ul aici
    public TextMeshProUGUI finalScore;  // Trage Text-ul de scor final aici
    public Transform player;            // Trage jucatorul aici

    private float startZ;
    private bool gameHasEnded = false;

    void Start()
    {
        startZ = player.position.z;
        gameOverPanel.SetActive(false); // Ne asiguram ca e inchis la inceput
    }
    
    public void restartGame()
    {
        Debug.Log("Restartarea jocului...");
        
        // Reluam timpul (in cazul in care l-ai oprit cu Time.timeScale = 0)
        Time.timeScale = 1f; 

        // Incarcam scena curenta
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void EndGame()
    {
        if (gameHasEnded) return;

        gameHasEnded = true;
        
        // Calculam scorul final
        float distance = player.position.z - startZ;
        finalScore.text = "Ai parcurs: " + Mathf.Floor(distance).ToString() + " metri";

        // Afisam panoul
        gameOverPanel.SetActive(true);

        // Putem opri timpul in joc (optional)
        // Time.timeScale = 0f; 
    }

  public void RestartGame()
    {
        // Time.timeScale = 1f; // Re-activam timpul daca l-am oprit
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}*/
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;    
    public TextMeshProUGUI finalScore;  
    public Transform player;            

    private float startZ;
    private bool gameHasEnded = false;

    void Start()
    {
        if (player != null) startZ = player.position.z;
        if (gameOverPanel != null) gameOverPanel.SetActive(false); 
    }

    public void EndGame()
    {
        if (gameHasEnded) return;

        gameHasEnded = true;
        
        float distance = player.position.z - startZ;
        if (finalScore != null)
        {
            finalScore.text = "Ai parcurs: " + Mathf.Floor(distance).ToString() + " metri";
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // AM PASTRAT DOAR O SINGURA FUNCTIE DE RESTART
    public void RestartGame()
    {
        Debug.Log("Restartarea jocului...");
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}