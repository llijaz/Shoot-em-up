using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new Item", menuName = "Custom/Item")]
public class ItemBehaviour : ScriptableObject
{
    new public string name = "new Item";

    public Sprite sprite;

    public bool applyToBullet = true;

    public UnityEvent trigger;
}
