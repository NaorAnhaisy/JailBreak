using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 120f; // Total time in seconds

    public Text timerText;
    public Image fillImage;
    public int failedMissionScene;

    public Color fullColor = Color.blue;
    public Color emptyColor = Color.red;
    private float currentTime;

    private void Start()
    {
        // Check if the current scene is Scene 1
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Activate the TimerScript and associated game object
            gameObject.SetActive(true);
            currentTime = totalTime;
        }
        else
        {
            // Deactivate the TimerScript and associated game object in other scenes
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        try
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                // hide life bar
                LifeBarManager.Instance.gameObject.SetActive(false);

                SceneManager.LoadScene(failedMissionScene);
            } else
            {
                UpdateTimerDisplay();
            }

            // Change the color based on remaining time
            fillImage.color = Color.Lerp(emptyColor, fullColor, currentTime / totalTime);
        } catch { }
    }

    private void UpdateTimerDisplay()
    {
        try
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

            timerText.text = timerString;
        } catch {}
    }
}