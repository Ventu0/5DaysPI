using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Vector3 originalCameraPos;
    [SerializeField]public CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] bool staticCamera;
    public static CameraController instance;
    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;
            originalCameraPos = Camera.main.transform.position;
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
        if(staticCamera) return;
        cinemachineCamera.LookAt = Player.instance.transform;
        cinemachineCamera.Follow = Player.instance.transform;
    }
}
