using UnityEngine;

public class Sleep : MonoBehaviour
{
    [SerializeField] NPC[] npc;
    public static Sleep instance;
    private void Awake()
    {   
        instance = this;
    }
    void Start()
    {
        for(int i = 0; i < npc.Length; i++)
        {
            int index = i;
            DiaENoite.instance.onNightStart += () => npc[index].canBeInteracted = true;
        }
    }
    public void SleepForTheDay()
    {
        Player player = Player.instance;
        DiaENoite dayNight = DiaENoite.instance;
        dayNight.ResetTime();
        dayNight.relogioScript.NextDay();
        
        player.SaveBedPos();
        player.SaveBedScene();
    }
}
