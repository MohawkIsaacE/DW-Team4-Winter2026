using System;
using UnityEngine;


public enum Item { Pizza, Donut, Spicy, Chips }

[Serializable]

public struct ItemData
{
    public Item itemName;
}
