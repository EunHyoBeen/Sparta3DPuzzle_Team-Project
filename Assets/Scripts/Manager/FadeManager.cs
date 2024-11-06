using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : Singleton<FadeManager>
{
    public Image fadeImage;  // Canvas �ȿ� Image �Ҵ�
    public float fadeDuration = 1.0f; // ���̵� ȿ�� ���� �ð�

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());  // ���� �� ���̵� ��
    }

    // ���̵� �� ȿ��
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

    // ���̵� �ƿ� ȿ�� �� �� ��ȯ
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

        // �� ��ȯ �� ���̵� �� ����
        SceneManager.LoadScene(sceneName);
        yield return FadeIn();
    }

    // �� ��ȯ�� ȣ���ϴ� �Լ�
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }
}
