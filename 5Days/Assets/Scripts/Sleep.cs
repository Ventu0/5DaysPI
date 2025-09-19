using UnityEngine;

public class Sleep : MonoBehaviour
{
    NPC npc;
    public static Sleep instance;
    private void Awake()
    {   
        instance = this;
    }
    void Start()
    {
        npc = GetComponent<NPC>();
        DiaENoite.instance.onNightStart += () => npc.canBeInteracted = true;
    }
    public void SleepForTheDay()
    {
        DiaENoite.instance.ResetTime();
        DiaENoite.instance.relogioScript.NextDay();
    }
}
