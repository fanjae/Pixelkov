using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    //[SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        // 활성화 시 골드 갱신
        GoldTextUpdate();
        // Player 골드 획득 로직 추가 시 이벤트 할당 예정
    }
    private void OnDisable()
    {
        
    }
    public void GoldTextUpdate()
    {
        // 플레이어의 골드를 받아서 텍스트 추가
    }
}
