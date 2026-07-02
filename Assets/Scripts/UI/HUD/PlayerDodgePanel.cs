using UnityEngine;

public class PlayerDodgePanel : MonoBehaviour
{
    [SerializeField] private GameObject[] panels; // 회피 게이지 패널들
    [SerializeField] private int maxDodgeCount;

    /// <summary>
    /// 제공되는 회피 카운트를 통해서 회피 UI를 갱신하는 메서드 입니다.
    /// </summary>
    /// <param name="count">회피 횟수</param>
    /// <param name="amount">회피 값</param>
    public void DodgeUIUpdate(int count)
    {
        // IndexOutofRange 예외처리
        int maxCount = Mathf.Min(panels.Length, count);

        // count 만큼의 dodgePanel은 꽉 차있는 상태
        for (int i = 0; i < maxCount; i++)
            panels[i].SetActive(true);

        // 나머지 패널들은 비워주기
        for(int i = maxCount; i < panels.Length; i++)
            panels[i].SetActive(false);
    }
}
