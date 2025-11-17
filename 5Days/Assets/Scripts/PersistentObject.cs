using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
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
    public IEnumerator LoseSequence(bool applyDebuff = false, Action OnSequenceEnd = null)
    {
        FadeController.instance.FadeInForHowMuchTime(3);
        print("Bed2");
        yield return new WaitForSecondsRealtime(1f);
        print("Bed1");
        if (Player.instance != null)
            Player.instance.LoadOnLastBed(true);
        yield return new WaitForSeconds(0.1f);

        if (Sleep.instance != null)
            Sleep.instance.SleepForTheDay(true);
        //if(applyDebuff) 
        OnSequenceEnd?.Invoke();
    }
    public IEnumerator HardcoreModeLost()
    {
        FadeController.instance.FadeInForHowMuchTime(2);
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("Lost");
        yield return new WaitForSecondsRealtime(0.5f);
        DeleteSave.instance.Deletar();
    }
}
