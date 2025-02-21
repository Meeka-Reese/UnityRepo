using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private TextMeshProUGUI timeText;

    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        // Get the current time
        System.DateTime currentTime = System.DateTime.Now;

        // Format the time as "hh:mm tt" (12-hour format with AM/PM)
        string formattedTime = currentTime.ToString("hh:mm tt");

        // Set the formatted time to the TextMeshProUGUI object
        timeText.text = formattedTime;
    }
}