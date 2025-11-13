using UnityEngine;

public class PassScene : MonoBehaviour
{
    [SerializeField] string sceneName = "BarcoCutscene";
    void Start()
    {
        
    }
    public void PassTheScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
