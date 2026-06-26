using UnityEngine;


namespace Enemy1
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        public int Damage => damage;
    }
}
