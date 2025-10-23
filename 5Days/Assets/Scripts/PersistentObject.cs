using System.Collections;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    public static PersistentObject instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        
    }
    public IEnumerator FadeSequence()
    {
        FadeController.instance.FadeInForHowMuchTime(2);
        print("Bed2");
        yield return new WaitForSecondsRealtime(1f);
        print("Bed1");
        if (Player.instance != null)
            Player.instance.LoadOnLastBed();
        yield return new WaitForSeconds(0.5f);

        if (Sleep.instance != null)
            Sleep.instance.SleepForTheDay();
    }

    void Update()
    {
        
    }
}
