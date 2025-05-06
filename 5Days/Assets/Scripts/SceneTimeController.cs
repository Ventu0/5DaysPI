using UnityEditor.ShortcutManagement;
using UnityEngine;

public class SceneTimeController : MonoBehaviour
{
    public delegate void OnPauseGame();
    public OnPauseGame onPauseGame;
    public static SceneTimeController instance;
    public float sceneTime;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        sceneTime = Time.timeScale;
    }
    void Update()
    {

    }
    public void PausarJogo()
    {
        sceneTime = 0;
        onPauseGame?.Invoke();
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
