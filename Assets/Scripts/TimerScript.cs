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
        currentTime = totalTime;
    }

    private void Update()
    {
        try
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
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