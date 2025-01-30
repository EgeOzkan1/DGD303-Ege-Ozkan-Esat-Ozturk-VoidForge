using UnityEngine;
using UnityEngine.SceneManagement;  // For loading scenes
using UnityEngine.UI;  // For handling UI elements

public class MainMenuController : MonoBehaviour
{
    public Button startButton;   // Reference to the Start button
    public Button quitButton;    // Reference to the Quit button
    public Button creditsButton; // Reference to the Credits button

    private void Start()
    {
        // Add listeners for buttons
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        creditsButton.onClick.AddListener(ShowCredits);
    }

    private void Update()
    {
        // Check for the Escape key press to return to the main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    // Method to start the game (load the HordeHighway scene)
    private void StartGame()
    {
        // Replace "HordeHighway" with the name of your actual gameplay scene
        SceneManager.LoadScene("HordeHighway");
    }

    // Method to quit the game
    private void QuitGame()
    {
        Debug.Log("Quit Game");  // Log for testing
        Application.Quit();      // Quit the application (works in build, not in the editor)
    }

    // Method to show credits (load the credits scene)
    private void ShowCredits()
    {
        // Replace "Credits" with the actual name of your credits scene
        SceneManager.LoadScene("Credits");
    }

    // Method to return to the main menu (load the main menu scene)
    private void ReturnToMainMenu()
    {
        // Replace "MainMenu" with the actual name of your main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
