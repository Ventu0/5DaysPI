using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.IO;


public class MenuController : MonoBehaviour
{
    [SerializeField] Button loadGameButton;
    [SerializeField] Sprite clickSprite;
    [SerializeField] Transform characterTransform;
    [SerializeField] float jumpQuantity = 10f;
    [SerializeField] float bobbingDuration = 0.5f;
    Image objectImage;
    Sprite originalSprite;
    Coroutine routine;
    void Start()
    {
        objectImage = characterTransform.GetComponent<Image>();
        originalSprite = objectImage.sprite;
        loadGameButton.onClick.AddListener(LoadGameButton);
        loadGameButton.enabled = ChecarSePossuiSave();
    }
    #region MenuButtons
    public void NewGameButton()
    {
        if (ChecarSePossuiSave())
        {
            string pasta = Application.persistentDataPath;
            string[] arquivos = Directory.GetFiles(pasta, "*.json");

            for(int i = 0; i < arquivos.Length; i++)
            {
                File.Delete(arquivos[i]); //se já houver um save, deleta ele
            }
        }
        SceneManager.LoadScene("CasaDianas");
    }
    public void LoadGameButton()
    {
        Loader.instance.Carregar();
    }
    public bool ChecarSePossuiSave()
    {
        string caminho = Application.persistentDataPath;
        string[] arquivosJson = Directory.GetFiles(caminho, "*.json");
        return arquivosJson.Length > 0;
    }
    public void OptionsButton()
    {

    }
    public void ExitButton()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
    #endregion
    public void OnCharacterClick()
    {
        if (routine != null)
            return; 
        else if (routine == null)
            routine = StartCoroutine(BounceEffect(characterTransform));

    }
    IEnumerator BounceEffect(Transform tranform)
    {
        objectImage.sprite = clickSprite;
        float iterador = 0;
        Vector2 newPos = new Vector2(tranform.position.x, tranform.position.y + jumpQuantity);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }

        yield return null;

        iterador = 0;
        newPos = new Vector2(tranform.position.x, tranform.position.y - jumpQuantity);
        while (iterador < bobbingDuration)
        {
            iterador += Time.deltaTime / bobbingDuration;
            tranform.position = Vector2.Lerp(tranform.position, newPos, iterador);
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        routine = null;
        objectImage.sprite = originalSprite;
    }
}
