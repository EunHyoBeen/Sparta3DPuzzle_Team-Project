using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 현재 씬에서 인스턴스를 찾음
                _instance = FindObjectOfType<T>();

                // 인스턴스가 없다면 새로운 GameObject를 생성해 인스턴스를 추가
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();

                    // 싱글톤 오브젝트가 씬 전환에도 파괴되지 않도록 설정
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    // 자식 클래스에서 Awake 메서드를 오버라이드할 경우, base.Awake()를 호출하도록 안내
    protected virtual void Awake()
    {
        // 이미 인스턴스가 존재하면 중복 생성 방지
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
