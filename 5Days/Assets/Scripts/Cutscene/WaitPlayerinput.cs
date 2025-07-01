using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
public class WaitPlayerinput : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] UnityEvent onInputReceived;
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
            onInputReceived.Invoke();
            print("Despausado");
        }
    }
}
