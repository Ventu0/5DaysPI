using UnityEngine;
using UnityEngine.SceneManagement;
public class EntrarEmCasa : MonoBehaviour
{
    [SerializeField] string sceneName;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
