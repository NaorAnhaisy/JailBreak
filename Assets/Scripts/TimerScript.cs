using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 120f; // Total time in seconds

    public Text timerText;
    private float currentTime;
    public LifeBarController lifeBarController;
    public int failedMissionScene;

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
                Debug.Log("Game over");
                SceneManager.LoadScene(failedMissionScene);
            } else
            {
                UpdateTimerDisplay();
            }
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