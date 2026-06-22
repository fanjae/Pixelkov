using System;
using UnityEngine;

public class PlayerGoldController : MonoBehaviour
{
    [SerializeField] private int initialGold = 0; // 시작 골드

    // 플레이어 보유 골드
    public int Gold { get; private set; }

    // 골드 값 변경에 대한 이벤트
    public event Action<int> OnGoldChanged;

    private void Awake()
    {
        // 시작 골드가 음수가 되지 않도록 보정
        Gold = Mathf.Max(0, initialGold);
    }

    // 지정한 금액 지불 가능
    public bool CanSpendGold(int amount)
    {
        return amount >= 0 && Gold >= amount;
    }

    // 골드 소모
    public bool SpendGold(int amount)
    {
        if (!CanSpendGold(amount)) return false;
        Gold -= amount;

        // 골드 변경 이벤트
        OnGoldChanged?.Invoke(Gold);

        return true;
    }

    // 골드를 획득한다
    public void AddGold(int amount)
    {
        if (amount <= 0) return;

        Gold += amount;

        // 골드 변경 이벤트
        OnGoldChanged?.Invoke(Gold);
    }
}