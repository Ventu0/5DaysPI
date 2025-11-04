using UnityEngine;
using System.IO;
public class TimeData
{
    public int currentDay;

    public int currentHour;
    public int currentMinute;
}
public class DayNightSave : MonoBehaviour
{
    [SerializeField] string path;

    RelogioScript clockScript;
    DiaENoite dayNight;
    private void Awake()
    {
        path = Application.persistentDataPath + path;
    }
    void Start()
    {
        clockScript = GetComponent<RelogioScript>();
        dayNight = GetComponent<DiaENoite>();
        PauseMenuController.instance.onSave += Save;
    }
    public void Save()
    {
        TimeData timeData = new TimeData();

        timeData.currentDay = clockScript.currentDay;
        timeData.currentHour = clockScript.GetCurrentHour();
        timeData.currentMinute = clockScript.GetCurrentMinute();
        string json = JsonUtility.ToJson(timeData, true);
        File.WriteAllText(path, json);
    }
    public void Load()
    {
        if (File.Exists(path))
        {
            
        }
    }
    public (bool has,TimeData data) HasSave()
    {
        if (!File.Exists(path))
        {
            return (false, null);
        }
        string json = File.ReadAllText(path);
        TimeData timeData = JsonUtility.FromJson<TimeData>(json);

        return (File.Exists(path),timeData);
    }
    
}
