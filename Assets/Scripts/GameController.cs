using UnityEngine;
using UnityEngine.SceneManagement;  // For loading scenes

public class GameController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the Main Menu scene
            SceneManager.LoadScene("MainMenu");
        }
    }
}
