using UnityEngine;

[CreateAssetMenu(menuName = "Item/Ammo")]
public class AmmoData : ItemData
{
    public override bool Use(Player player)
    {
        return false;
    }

    // 총알 데이터
}
