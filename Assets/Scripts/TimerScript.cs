using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 120f; // Total time in seconds

    private Text timerText;
    private float currentTime;

    private void Start()
    {
        timerText = GetComponent<Text>();
        currentTime = totalTime;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            // Timer has reached zero, handle the event here
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = timerString;
    }
}