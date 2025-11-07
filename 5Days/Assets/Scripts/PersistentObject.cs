using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        FadeController.instance.FadeInForHowMuchTime(3);
        print("Bed2");
        yield return new WaitForSecondsRealtime(1f);
        print("Bed1");
        if (Player.instance != null)
            Player.instance.LoadOnLastBed(true);
        yield return new WaitForSeconds(0.1f);

        if (Sleep.instance != null)
            Sleep.instance.SleepForTheDay(true);
    }
    public IEnumerator HardcoreModeLost()
    {
        FadeController.instance.FadeInForHowMuchTime(2);
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("Menu");
        yield return new WaitForSecondsRealtime(0.5f);
        DeleteSave.instance.Deletar();
        MenuController.instance.DeactivateContinueButton();
    }

    void Update()
    {
        
    }
}
