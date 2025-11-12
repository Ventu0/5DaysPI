using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEnder : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    void Start()
    {
        if(DeleteSave.instance != null)
            DeleteSave.instance.DeletarTudoDoDontDestroy();
    }
    public void EndCutscene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
