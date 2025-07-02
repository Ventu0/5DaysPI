using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class EntrarEmCasa : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] bool usePlayerSavedPosition;
    [SerializeField] UnityEvent onChangeScene;
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
            Player player = Player.instance;
            if (usePlayerSavedPosition)
            {
                if (player.lastSavedPosition == Vector2.zero)
                {
                    player.transform.position = Vector2.zero;
                } else
                    player.transform.position = player.lastSavedPosition;
            }
            else
            {
                player.SavePosition();
                player.transform.position = Vector2.zero;
            }
            onChangeScene?.Invoke();
        }
    }
}
