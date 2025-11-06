using UnityEngine;

public class PassScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void PassTheScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
