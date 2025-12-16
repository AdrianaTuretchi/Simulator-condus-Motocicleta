using UnityEngine;
using UnityEngine.UI; // Necesar pentru a lucra cu Text și Button
using UnityEngine.SceneManagement; // Necesar pentru a reîncărca scena
using TMPro;
public class UIManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    //public Text finalScoreText; // Textul unde va fi afișat scorul
    public TextMeshProUGUI finalScoreText; // <- Folosiți acest tip
    // CRITIC: Numele scenei curente (trebuie să corespundă cu numele din Build Settings)
    private string currentSceneName;

    void Start()
    {
        // Ascunde ecranul de Game Over la începutul jocului
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Păstrează numele scenei curente pentru a putea reîncărca
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void ShowGameOver(float score)
    {
        Transform cameraTransform = Camera.main.transform;
        transform.position = cameraTransform.position + (cameraTransform.forward * 2.0f);
        transform.LookAt(transform.position + cameraTransform.forward);
        // 1. Afișează panoul
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 2. Afișează scorul final
        if (finalScoreText != null)
        {
            finalScoreText.text = "Distanta Parcursa: " + Mathf.FloorToInt(score) + " m";
        }
    }

    // Această funcție va fi apelată de butonul de restart
    public void RestartGame()
    {
        // Reîncarcă scena curentă pentru a reseta jocul
        SceneManager.LoadScene(currentSceneName);
    }
}