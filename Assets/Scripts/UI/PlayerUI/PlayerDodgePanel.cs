using UnityEngine;

public class PlayerDodgePanel : MonoBehaviour
{
    [SerializeField] private DodgePanel[] panels; // 회피 게이지 패널들
    [SerializeField] private int maxDodgeCount;

    private void Awake()
    {
        // 패널 할당이 되어있지 않다면 자식 오브젝트 패널들을 추가
        if(panels.Length == 0)
        {
            Debug.Log("패널추가");
            panels = transform.GetComponentsInChildren<DodgePanel>();
        }
    }
    private void Start()
    {
        MaxDodgeCountChanged(maxDodgeCount);
        DodgeUIUpdate(Random.Range(0, maxDodgeCount), Random.Range(0f, 1f));
    }

    /// <summary>
    /// 제공되는 회피 카운트와 값을 통해서 회피 UI를 갱신하는 메서드 입니다.
    /// </summary>
    /// <param name="count">회피 횟수</param>
    /// <param name="amount">회피 값</param>
    public void DodgeUIUpdate(int count, float amount)
    {
        // IndexOutofRange 예외처리
        int maxCount = Mathf.Min(maxDodgeCount, count);

        // count 만큼의 dodgePanel은 꽉 차있는 상태
        for(int i = 0; i < maxCount; i++)
            panels[i].FillUpdate(1f);

        // 나머지 패널들은 비워주기
        for(int i = maxCount; i < maxDodgeCount; i++)
            panels[i].FillUpdate(0f);

        // 
        if (maxCount >= maxDodgeCount)
            return;
        
        // amount 는 항상 1보다 작다고 가정하고 구현했습니다.
        panels[maxCount].FillUpdate(amount);
    }
    /// <summary>
    /// 최대 회피 개수 변경시 발생할 이벤트에 할당하게 될 메서드 입니다.
    /// </summary>
    /// <param name="maxCount">변경하고자 하는 최대 회피 개수</param>
    public void MaxDodgeCountChanged(int maxCount)
    {
        //if (maxCount == maxDodgeCount) return;

        maxDodgeCount = maxCount;

        // 전체 패널 투명하게
        for (int i = 0; i < panels.Length; i++)
            HidePanel(i);

        // 최대 회피 개수만큼만 보이게 하기
        for (int i = 0; i < maxDodgeCount; i++)
            ShowPanel(i);
    }
    private void HidePanel(int index)
    {
        panels[index].GetComponent<CanvasGroup>().alpha = 0f;
    }
    private void ShowPanel(int index)
    {
        panels[index].GetComponent<CanvasGroup>().alpha = 1f;
    }
}
