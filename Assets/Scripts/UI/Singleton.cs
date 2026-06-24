using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance = null;
    private static bool isQuit = false;
    public static T Instance
    {
        get
        {
            if (isQuit)
                return null;

            if(instance == null)
            {
                instance = FindFirstObjectByType<T>();

                if(instance == null)
                {
                    GameObject inst = new GameObject(typeof(T).Name);
                    instance = inst.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        isQuit = true;
    }
}
