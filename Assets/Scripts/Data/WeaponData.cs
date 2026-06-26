using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon")]
public class WeaponData : ItemData
{
    [Header("Weapon")]
    [SerializeField] private int damage;
    [SerializeField] private int capacity;
    [SerializeField] private float reloadTime;
    [SerializeField] private int ammoItemId;

    public int Damage => damage;
    public int Capacity => capacity;
    public float ReloadTime => reloadTime;
    public int AmmoItemId => ammoItemId;

    public override void Use(Player player)
    {
        // player.EquipWeapon(this);
    }

    // 무기 데이터
    // - 데미지
    // - 탄창 용량
    // - 장전 시간
    // - 총알 아이템 ID
}