using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class ReturnScene : MonoBehaviour
{
    [Header("Configurações animation")]
    [SerializeField] float duration = 1.5f;
    [SerializeField] float characterSpeed = 3;
    [SerializeField] string sceneName;
    [SerializeField] Animator fadeAnimation;
    public static ReturnScene instance;
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
    private void Start()
    {
        fadeAnimation.gameObject.SetActive(false);
    }
    public void ReturnSceneBTN()
    {
        QuestController.instance.menu.SetActive(true);
        DiaENoite.instance.dontDestroyObject.SetActive(true);
        SceneManager.UnloadSceneAsync(sceneName);
        SceneTimeController.instance.onPauseGame?.Invoke();
        Time.timeScale = 1f;
    }
    public IEnumerator RunAnimation(BasePersonagem[] characters)
    {
        float iterador = 0;
        fadeAnimation.gameObject.SetActive(true);
        fadeAnimation.SetBool("Stay", true);
        InteractButtonsController.instance.menu.SetActive(false);
        while (iterador < duration)
        {
            for(int i = 0; i < characters.Length; i++)
            {
                Transform characterTransform = characters[i].transform;
                characters[i].GetComponent<SpriteRenderer>().flipX = true;
                iterador += Time.deltaTime;
                characterTransform.Translate(Vector3.left * characterSpeed * iterador / duration);
                yield return null;
            }
        }
        ReturnSceneBTN();
    }
}