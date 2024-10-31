using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : Singleton<DataManager>
{    
    // 싱글톤 선언
    protected override void Awake()
    {
        base.Awake();
    }

    // 달성한 Scene 정보를 저장할 데이터 클래스 정의
    [System.Serializable]
    public class TopSceneData
    {
        public string sceneName;
    }

    public string GetTopSceneName()
    {
        string filePath = "./Assets/Json/topScene.json";

        // 파일이 존재하지 않는다면
        if (!File.Exists(filePath))
        {
            SetTopScene("Stage1");
        }

        // json파일 읽어서 Scene Load
        string json = File.ReadAllText(filePath);
        TopSceneData data = JsonUtility.FromJson<TopSceneData>(json);
        return data.sceneName;
    }

    public void LoadTopScene()
    {
        // 파일 경로 지정
        string filePath = "./Assets/Json/topScene.json";

        // 파일이 존재하지 않는다면
        if (!File.Exists(filePath))
        {
            SetTopScene("Stage1");
        }

        // json파일 읽어서 Scene Load
        string json = File.ReadAllText(filePath);
        TopSceneData data = JsonUtility.FromJson<TopSceneData>(json);
        SceneManager.LoadScene(data.sceneName);
    }

    public void SetTopScene(string name)
    {
        // 매개변수 읽어서 json 작성
        TopSceneData data = new TopSceneData { sceneName = name };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("./Assets/Json/topScene.json", json);
    }
}
