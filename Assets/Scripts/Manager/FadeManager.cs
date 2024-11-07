using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : Singleton<FadeManager>
{
    public Image fadeImage;  // Canvas 안에 Image 할당
    public float fadeDuration = 1.0f; // 페이드 효과 지속 시간

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());  // 시작 시 페이드 인
    }

    // 페이드 인 효과
    public IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        float timer = 0;

        while (timer < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }

    // 페이드 아웃 효과 및 씬 전환
    public IEnumerator FadeOut(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        float timer = 0;

        while (timer < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;

        // 씬 전환 후 페이드 인 시작
        SceneManager.LoadScene(sceneName);
        yield return FadeIn();
    }

    // 씬 전환을 호출하는 함수
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }
}
