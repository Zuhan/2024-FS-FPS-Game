using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class playerStats
{
    public static List<weaponStats> weapons = new List<weaponStats>();
    public static int money = 0;
    public static float maxHP = 40;
    public static float hp = maxHP;
    public static List<GameObject> keys = new List<GameObject>();
    public static List<hpPotion> potions = new List<hpPotion>();
    public static bool slingUI = false;
    public static bool fireUI = false;
    public static bool thUI = false;
    public static bool expStaffUI = false;
    public static bool potionUI = false;
    public static int currentWeapon = 0;
}
