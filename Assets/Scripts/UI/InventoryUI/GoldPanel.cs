using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    [SerializeField] private PlayerGoldController goldController;
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        // 활성화 시 골드 갱신
        // Player 골드 획득 로직 추가 시 이벤트 할당 예정
        if(goldController != null)
        {
            GoldTextUpdate(goldController.Gold);
            goldController.OnGoldChanged += GoldTextUpdate;
        }
    }
    private void OnDisable()
    {
        if(goldController != null)
        {
            goldController.OnGoldChanged -= GoldTextUpdate;
        }
    }
    public void GoldTextUpdate(int amount)
    {
        // 플레이어의 골드를 받아서 텍스트 추가
        goldText.text = amount.ToString();
    }
}
