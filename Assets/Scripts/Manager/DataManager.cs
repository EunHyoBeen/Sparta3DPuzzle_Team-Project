using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager>
{    
    // �̱��� ����
    protected override void Awake()
    {
        base.Awake();
    }

    // �޼��� Scene ������ ������ ������ Ŭ���� ����
    [System.Serializable]
    public class TopSceneData
    {
        public string sceneName;
    }

    public string GetTopSceneName()
    {
        string filePath = "./Assets/Json/topScene.json";

        // ������ �������� �ʴ´ٸ�
        if (!File.Exists(filePath))
        {
            SetTopScene("Stage1");
        }

        // json���� �о Scene Load
        string json = File.ReadAllText(filePath);
        TopSceneData data = JsonUtility.FromJson<TopSceneData>(json);
        return data.sceneName;
    }

    public void LoadTopScene()
    {
        // ���� ��� ����
        string filePath = "./Assets/Json/topScene.json";

        // ������ �������� �ʴ´ٸ�
        if (!File.Exists(filePath))
        {
            SetTopScene("Stage1");
        }

        // json���� �о Scene Load
        string json = File.ReadAllText(filePath);
        TopSceneData data = JsonUtility.FromJson<TopSceneData>(json);
        SceneManager.LoadScene(data.sceneName);
    }

    public void SetTopScene(string name)
    {
        // �Ű����� �о json �ۼ�
        TopSceneData data = new TopSceneData { sceneName = name };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("./Assets/Json/topScene.json", json);
    }
}
