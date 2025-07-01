using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
public class WaitPlayerinput : MonoBehaviour
{
    public PlayableDirector director;
    [SerializeField] UnityEvent onInputReceived;
    public bool waitingInput = false;
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
            
            onInputReceived.Invoke();
            print("Despausado");
        }
    }
}
