using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject cameraRoot;
    [SerializeField]public CinemachineVirtualCamera cinemachineCamera;
    public static CameraController instance;
    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        else
        {
            Destroy(mainCamera.gameObject);
        }
        if (instance == null)
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
    void Update()
    {
        
    }
}
