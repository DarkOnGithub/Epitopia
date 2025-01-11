using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;       // Nom de l'item
    public Sprite icon;           // Icône de l'item
    public string description;    // Description de l'item
    public bool isStackable;      // Peut-il être empilé ?
    public enum ItemType { Weapon, Tool, Armor }
    public ItemType itemType;     // Type d'item
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class WeaponItem : Item
{
    public int attackPower;       // Puissance d'attaque
    public float attackSpeed;     // Vitesse d'attaque
}

[CreateAssetMenu(fileName = "New Tool", menuName = "Inventory/Tool")]
public class ToolItem : Item
{
    public int miningPower;       // Puissance pour miner
    public int durability;        // Durabilité de l'outil
}

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Armor")]
public class ArmorItem : Item
{
    public int defensePower;      // Puissance de défense
    public string armorType;      // Type d'armure (plastron, casque, bottes)
}