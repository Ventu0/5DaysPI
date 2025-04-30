using UnityEditor.ShortcutManagement;
using UnityEngine;

public class SceneTimeController : MonoBehaviour
{
    public delegate void OnPauseGame();
    public OnPauseGame onPauseGame;
    public static SceneTimeController instance;
    public float sceneTime;
    void Start()
    {
        sceneTime = Time.timeScale;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if(sceneTime == 0)
        {
            onPauseGame?.Invoke();
        }
    }
    public bool isPaused()
    {
        if(sceneTime == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
