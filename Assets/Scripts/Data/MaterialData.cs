using UnityEngine;

[CreateAssetMenu(menuName = "Item/MaterialData")]
public class MaterialData : ItemData
{
    public override void Use(Player player)
    {
        // 재료도 직접 사용은 안함
    }
}