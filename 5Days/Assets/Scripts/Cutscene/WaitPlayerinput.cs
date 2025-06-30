using UnityEngine;
using UnityEngine.Playables;
public class WaitPlayerinput : MonoBehaviour
{
    PlayableDirector director;
    bool waitingInput = false;
    void Start()
    {
        
    }
    public void Wait()
    {
        waitingInput = true;
        director.Pause();
    }
    void Update()
    {
       if(waitingInput && Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            director.Play();
            waitingInput = false;
        }
    }
}
