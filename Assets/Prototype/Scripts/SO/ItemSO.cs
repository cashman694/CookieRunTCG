using UnityEngine;

namespace Prototype
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public int attack;
        public int health;
        public int level;
        public Sprite sprite;
    }

    [CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Object/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public Item[] items;
    }
}
