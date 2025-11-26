using UnityEngine;

public class PassScene : MonoBehaviour
{
    [SerializeField] string sceneName = "BarcoCutscene";
    [SerializeField] bool endByBoolean = false;
    [SerializeField] bool booleanValue = false;
    void Start()
    {
        
    }
    public void PassTheScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    private void Update()
    {
        if(!endByBoolean) return;
        if (booleanValue)
        {
            PassTheScene();
        }
    }
}
