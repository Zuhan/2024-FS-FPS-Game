using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class playerStats
{
    public static List<weaponStats> weapons = new List<weaponStats>();
    public static int money = 0;
    public static float maxHP = 100;
    public static float hp = maxHP;
    public static List<GameObject> keys = new List<GameObject>();
    public static List<hpPotion> potions = new List<hpPotion>(); 
}
