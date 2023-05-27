using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeScript : MonoBehaviour
{
    public string sceneName; // Name of the scene to load
    public float delayInSeconds = 5f; // Delay in seconds before scene change

    private float timer = 0f;
    private bool sceneChangeTriggered = false;

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if the timer has reached the specified delay and the scene change hasn't been triggered yet
        if (timer >= delayInSeconds && !sceneChangeTriggered)
        {
            // Set the scene change flag to true
            sceneChangeTriggered = true;

            // Load the new scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
