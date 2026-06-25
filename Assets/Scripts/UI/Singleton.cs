using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance = null;
    private static bool isQuit = false;
    public static T Instance
    {
        get
        {
            // 애플리케이션 종료 시에는 null 반환
            if (isQuit)
                return null;

            if(instance == null)
            {
                instance = FindFirstObjectByType<T>();
                
                // 씬 전체에서 T 컴포넌트가 없으면 새로 생성
                if(instance == null)
                {
                    GameObject inst = new GameObject(typeof(T).Name);
                    instance = inst.AddComponent<T>();
                    DontDestroyOnLoad(inst);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // 기존 할당된 인스턴스가 존재하지 않으면 현재 오브젝트를 할당
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        // 이미 인스턴스가 존재하면 파괴
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        // 애플리케이션이 종료됨을 알림
        isQuit = true;
    }
}
