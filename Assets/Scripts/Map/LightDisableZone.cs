using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDisableZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Light2D[] lights = collision.GetComponentsInChildren<Light2D>(true); // 조명 관련 정보 모두 찾아옴

        foreach (Light2D light in lights) // 비활성화
        {
            light.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Light2D[] lights = collision.GetComponentsInChildren<Light2D>(true); // 조명 관련 정보 모두 찾아옴

        foreach (Light2D light in lights) // 활성화
        {
            light.enabled = true;
        }
    }
}
