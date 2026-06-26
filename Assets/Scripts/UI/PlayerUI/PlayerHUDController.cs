using UnityEngine;

/// <summary>
/// HUD UI를 관리하기 위한 컴포넌트 입니다.
/// </summary>
public class PlayerHUDController : MonoBehaviour
{
    [Header("플레이어 오브젝트 할당")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("플레이어 HUD")]
    [SerializeField] private PlayerHpPanel hpPanel; // 체력 패널
    [SerializeField] private PlayerDodgePanel dodgePanel;   // 회피 패널

    private int curDodgeCount = -1;
    private void Awake()
    {
        // 캐싱 확인
        if(hpPanel == null) hpPanel = FindAnyObjectByType<PlayerHpPanel>();
        if(dodgePanel == null) dodgePanel = FindAnyObjectByType<PlayerDodgePanel>();
        if (player == null) player = FindAnyObjectByType<Player>();
        if (playerHealth == null) playerHealth = FindAnyObjectByType<PlayerHealth>();
    }
    private void Start()
    {
        // UI 초기화
        if(player != null && dodgePanel != null)
        {
            dodgePanel.DodgeUIUpdate(player.CurrentDodgeCount);
            curDodgeCount = player.CurrentDodgeCount;
        }
        if(playerHealth != null && hpPanel != null)
        {
            hpPanel.Init(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }
    }
    private void OnEnable()
    {
        // 플레이어가 null이 아니면 이벤트 할당 예정
        if(playerHealth == null ||  hpPanel == null) return;

        // 이벤트 할당 로직 추가
    }
    private void OnDisable()
    {
        // 만약 HUD를 끄거나 씬 전환되는 상황이 생기면 이벤트 해제
        if(playerHealth == null ||  hpPanel == null) return;

        // 이벤트 취소 로직 추가
    }
    private void Update()
    {
        // 이벤트로 연결하기 전 임시 업데이트 로직
        if(playerHealth != null && hpPanel != null)
        {
            hpPanel.HpUIUpdate(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        // 플레이어가 null이 아니면 dodgePanel에 갱신하는 함수를 호출
        if (player == null || dodgePanel == null) return;

        // 현재 회피 수가 동일하면 갱신하지 않음
        if (player.CurrentDodgeCount == curDodgeCount) return;

        dodgePanel.DodgeUIUpdate(player.CurrentDodgeCount);
        curDodgeCount = player.CurrentDodgeCount;
    }
}
