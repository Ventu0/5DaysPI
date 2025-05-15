using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField]public CinemachineVirtualCamera cinemachineCamera;
    public static CameraController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        cinemachineCamera.LookAt = Player.instance.transform;
        cinemachineCamera.Follow = Player.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
