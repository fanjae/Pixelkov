using UnityEngine;

/// <summary>
/// HUD UI를 관리하기 위한 컴포넌트 입니다.
/// </summary>
public class PlayerHUDController : MonoBehaviour
{
    // 플레이어를 할당 예정

    [Header("플레이어 HUD")]
    [SerializeField] private PlayerHpPanel hpPanel; // 체력 패널
    [SerializeField] private PlayerDodgePanel dodgePanel;   // 회피 패널
    private void Awake()
    {
        // 캐싱 확인
        
        // 추후에 플레이어 캐싱 확인 로직 추가 예정
        
        if(hpPanel == null) hpPanel = FindAnyObjectByType<PlayerHpPanel>();
        if(dodgePanel == null) dodgePanel = FindAnyObjectByType<PlayerDodgePanel>();
    }
    private void OnEnable()
    {
        // 플레이어가 null이 아니면 이벤트 할당 예정
    }
    private void Start()
    {
        // UI 초기화
    }
    private void Update()
    {
        // 플레이어가 null이 아니면 dodgePanel에 갱신하는 함수를 호출
    }
    private void OnDisable()
    {
        // 만약 HUD를 끄거나 씬 전환되는 상황이 생기면 이벤트 해제
    }
}
