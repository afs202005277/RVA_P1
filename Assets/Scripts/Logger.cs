using UnityEngine;
using System.IO;
using TMPro;

public static class Logger
{
    private static string logFilePath;
    private static TextMeshProUGUI logText;
    

    // Call this method in an initialization script to set up the logger
    public static void Initialize(TextMeshProUGUI path)
    {
        logFilePath = "/downloads/baseDefensegameLogs.txt";
        //path.text = logFilePath;
        Debug.Log(logFilePath);
        Log("Logger Initialized");
        logText = path;
    }

    public static void Log(string message)
    {
        //logText.text += message + "\n";
        // Write log to file
        //using (StreamWriter writer = new StreamWriter(logFilePath, true))
        //{
        //    writer.WriteLine($"{System.DateTime.Now}: {message}");
        //}
        logText.text += message + "\n";
    }

    public static void ClearLogs()
    {
        // Clear the log file
        if (File.Exists(logFilePath))
        {
            File.WriteAllText(logFilePath, string.Empty); // Clear the log file
        }
    }

    public static string DisplayLogs()
    {
        // Read the logs from the file and return as a string
        if (File.Exists(logFilePath))
        {
            return File.ReadAllText(logFilePath);
        }
        return string.Empty; // Return an empty string if no logs exist
    }
}
