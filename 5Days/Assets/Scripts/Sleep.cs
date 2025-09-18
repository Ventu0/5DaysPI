using UnityEngine;

public class Sleep : MonoBehaviour
{
    [SerializeField] bool canSleep;
    public static Sleep instance;
    private void Awake()
    {   
        instance = this;
    }
    void Start()
    {
        
    }
    public void SleepForTheDay()
    {
        DiaENoite.instance.ResetTime();
        DiaENoite.instance.relogioScript.NextDay();
    }
}
