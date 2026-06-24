using UnityEngine;

public class LoadButton : MonoBehaviour
{
    [SerializeField] private SceneType sceneType;
    public void ClickLoadButton()
    {
        SceneLoadManager.Instance.LoadScene(sceneType);
    }
}
