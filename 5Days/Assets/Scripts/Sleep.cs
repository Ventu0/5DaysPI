using UnityEngine;

public class Sleep : MonoBehaviour
{
    [SerializeField] bool canSleep;
    void Start()
    {
        
    }
    public void SleepForTheDay()
    {
        DiaENoite.instance.ResetTime();
        DiaENoite.instance.relogioScript.NextDay();
    }
}
