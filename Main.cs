using System;
using BepInEx;
using UnityEngine;
using Steamworks;
using HarmonyLib;
using System.Linq;

[BepInPlugin("com.ecstsy.muckmenu", "Muck Menu", "1.0.0")]

public class Main : BaseUnityPlugin
{

    private bool showMenu = false;
    private bool godMode = false;
    private bool infiniteStamina = false;
    private bool infiniteFood = false;
    private Rect windowRect = new Rect(20, 20, 280, 400);
    Vector2 scrollPos;

    private GUIStyle buttonStyle;
    private GUIStyle windowStyle;
    private GUIStyle scrollViewStyle;
    private GUIStyle verticalScrollbarStyle;
    private GUIStyle verticalThumbStyle;

    private bool stylesInitialized = false;

    public void Start()
    {
        Logger.LogInfo("Muck Menu Loaded!");
    }

    public void Update()
    {
        if (godMode)
        {
            PlayerStatus.Instance.hp = PlayerStatus.Instance.maxHp;
        }

        if (infiniteStamina)
        {
            PlayerStatus.Instance.stamina = PlayerStatus.Instance.maxStamina;
        }

        if (infiniteFood)
        {
            PlayerStatus.Instance.hunger = PlayerStatus.Instance.maxHunger;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            showMenu = !showMenu;
        }
    }

    void OnGUI()
    {
        if (!showMenu) return;

        if (!stylesInitialized)
        {
            InitStyles();
            stylesInitialized = true;
        }

        windowRect = GUI.Window(0, windowRect, DrawWindow, "Muck Menu", windowStyle);
    }

    void DrawWindow(int id)
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);
        GUILayout.BeginVertical();

        if (GUILayout.Button("Add 1,000 coins", buttonStyle))
        {
            InventoryItem item = ItemManager.Instance.allScriptableItems[3];
            item.amount = 1000;
            InventoryUI.Instance.AddItemToInventory(item);
            Logger.LogInfo($"[Muck Menu] Added 1000 coins to inventory.");
        }

        if (GUILayout.Button("Drop 1,000 Coins", buttonStyle))
        {
            ClientSend.DropItem(3, 1000);
        }

        if (GUILayout.Button("Full Stamina Stamina", buttonStyle))
        {
            PlayerStatus.Instance.stamina = PlayerStatus.Instance.maxStamina;
        }

        GUI.color = infiniteStamina ? Color.green : Color.white;
        if (GUILayout.Button($"Infinite Stamina: {(infiniteStamina ? "ON" : "OFF")}", buttonStyle))
        {
            infiniteStamina = !infiniteStamina;
            Logger.LogInfo($"[Muck Menu] Infinite Stamina {(infiniteStamina ? "Enabled" : "Disabled")}.");
        }
        GUI.color = Color.white;

        GUI.color = infiniteFood ? Color.green : Color.white;
        if (GUILayout.Button($"Infinite Food: {(infiniteFood ? "ON" : "OFF")}", buttonStyle))
        {
            infiniteFood = !infiniteFood;
            Logger.LogInfo($"[Muck Menu] Infinite Food {(infiniteFood ? "Enabled" : "Disabled")}.");
        }
        GUI.color = Color.white;

        if (GUILayout.Button("Unlock ALL Achievements", buttonStyle))
        {
            SteamUserStats.Achievements.Do(a => a.Trigger(true));
            SteamUserStats.StoreStats();
        }

        GUI.color = godMode ? Color.green : Color.white;
        if (GUILayout.Button($"God Mode: {(godMode ? "ON" : "OFF")}", buttonStyle))
        {
            godMode = !godMode;
            Logger.LogInfo($"[Muck Menu] God Mode {(godMode ? "Enabled" : "Disabled")}.");
        }
        GUI.color = Color.white;

        if (GUILayout.Button("Full Health", buttonStyle))
        {
            PlayerStatus.Instance.hp = PlayerStatus.Instance.maxHp;
            Logger.LogInfo("[Muck Menu] Set player health to max.");
        }

        if (GUILayout.Button("Revive All", buttonStyle))
        {
            GameManager.players.Values.Where(m => m != null && m.dead && !m.disconnected).Do(m => ClientSend.RevivePlayer(m.id, -1, false));
        }

        if (GUILayout.Button("Close Menu", buttonStyle))
        {
            showMenu = false;
        }

        GUILayout.EndVertical(); 
        GUILayout.EndScrollView();
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++) pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private void InitStyles()
    {

        Color baseColor = new Color(0.05f, 0.05f, 0.05f); 
        Color borderColor = new Color(0.2f, 0.2f, 0.2f);  

        GUI.backgroundColor = Color.black;
        GUI.contentColor = Color.white;

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.fontSize = 14;
        buttonStyle.normal.background = MakeTex(2, 2, new Color(0.12f, 0.12f, 0.12f)); 
        buttonStyle.hover.background = MakeTex(2, 2, new Color(0.18f, 0.18f, 0.18f));
        buttonStyle.active.background = MakeTex(2, 2, new Color(0.25f, 0.25f, 0.25f));

        buttonStyle.border = new RectOffset(2, 2, 2, 2);
        buttonStyle.margin = new RectOffset(4, 4, 4, 4);
        buttonStyle.padding = new RectOffset(6, 6, 4, 4);

        windowStyle = new GUIStyle(GUI.skin.window);
        windowStyle.fontSize = 16;
        windowStyle.normal.background = MakeTex(2, 2, baseColor);
        windowStyle.padding = new RectOffset(10, 10, 20, 10);
        windowStyle.border = new RectOffset(4, 4, 4, 4);

        scrollViewStyle = new GUIStyle(GUI.skin.scrollView);
        scrollViewStyle.normal.background = MakeTex(2, 2, baseColor);

        verticalScrollbarStyle = new GUIStyle(GUI.skin.verticalScrollbar);
        verticalScrollbarStyle.fixedWidth = 8;
        verticalScrollbarStyle.normal.background = MakeTex(2, 2, borderColor);

        verticalThumbStyle = new GUIStyle(GUI.skin.verticalScrollbarThumb);
        verticalThumbStyle.fixedWidth = 8;
        verticalThumbStyle.normal.background = MakeTex(2, 2, new Color(0.35f, 0.35f, 0.35f));
        verticalThumbStyle.hover.background = MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f));
    }

}
