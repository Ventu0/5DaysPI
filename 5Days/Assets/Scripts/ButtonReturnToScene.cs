using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonReturnToScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    public void ReturnScene()
    {
        SceneManager.LoadScene(sceneName);
        SceneTimeController.instance.onPauseGame?.Invoke();
        Time.timeScale = 1f;    
    }
}
