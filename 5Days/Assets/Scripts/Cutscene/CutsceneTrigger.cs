using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            director.Play();
            collision.GetComponent<Player>().canMove = false;
            CutsceneController.instance.whisperSound.Stop();
        }
    }
}
