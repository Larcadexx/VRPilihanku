using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Referensi UI")]
    public Image fadeImage;

    [Header("Pengaturan Transisi")]
    public float fadeDuration = 1.0f;
    public Color fadeColor = Color.black;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (fadeImage != null)
            {
                fadeImage.color = fadeColor;
                fadeImage.gameObject.SetActive(true);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void PindahScene(string namaScene)
    {
        StartCoroutine(ProsesTransisi(namaScene));
    }

    private IEnumerator ProsesTransisi(string namaScene)
    {
        yield return StartCoroutine(Fade(0, 1));

        AsyncOperation operasi = SceneManager.LoadSceneAsync(namaScene);
        while (!operasi.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(Fade(1, 0));
    }

    private IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null) yield break;

        fadeImage.gameObject.SetActive(true);
        float timer = 0;
        Color tempColor = fadeColor;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            tempColor.a = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadeImage.color = tempColor;
            yield return null;
        }

        tempColor.a = endAlpha;
        fadeImage.color = tempColor;

        if (endAlpha <= 0)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }
}