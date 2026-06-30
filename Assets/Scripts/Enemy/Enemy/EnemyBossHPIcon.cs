using UnityEngine;

public class EnemyBossHPIcon : MonoBehaviour
{
    private float rotationSpeed = 600.0f;
    void Update()
    {
        //회전
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
