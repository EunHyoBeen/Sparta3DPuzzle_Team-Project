using UnityEngine;

public class DestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���� ������ �ν��Ͻ��� ã��
                _instance = FindObjectOfType<T>();

                // �ν��Ͻ��� ���ٸ� ���ο� GameObject�� ������ �ν��Ͻ��� �߰�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    // �ڽ� Ŭ�������� Awake �޼��带 �������̵��� ���, base.Awake()�� ȣ���ϵ��� �ȳ�
    protected virtual void Awake()
    {
        // �̹� �ν��Ͻ��� �����ϸ� �ߺ� ���� ����
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
