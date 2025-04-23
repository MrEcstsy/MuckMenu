using System;
using BepInEx;
using UnityEngine;
using Steamworks;
using HarmonyLib;
using System.Linq;

namespace MuckMenu
{
    [BepInPlugin("com.ecstsy.muckmenu", "Muck Menu", "1.0.0")]

    public class Main : BaseUnityPlugin
    {

        private bool showMenu;
        private bool godMode;
        private bool infiniteStamina;
        private bool infiniteFood;
        private bool megaJump;

        private Rect windowRect = new Rect(20, 20, 280, 400);
        private Vector2 scrollPos;
        private bool stylesInitialized = false;

        private GUIStyle buttonStyle;
        private GUIStyle windowStyle;
        private GUIStyle scrollViewStyle;
        private GUIStyle verticalScrollbarStyle;
        private GUIStyle verticalThumbStyle;

        public void Start()
        {
            Logger.LogInfo("Muck Menu Loaded!");
        }

        public void Update()
        {
            HandleKeybinds();
            ApplyToggles();
        }

        void OnGUI()
        {
            if (!showMenu)
                return;

            if (!stylesInitialized)
            {
                InitStyles();
            }

            windowRect = GUI.Window(0, windowRect, DrawWindow, "Muck Menu", windowStyle);
        }

        private void HandleKeybinds()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                showMenu = !showMenu;
            }
        }

        private void ApplyToggles()
        {
            if (godMode)
                PlayerUtils.SetFullHealth();

            if (infiniteStamina)
                PlayerUtils.SetFullStamina();

            if (infiniteFood)
                PlayerUtils.SetFullHunger();
        }

        private void DrawWindow(int id)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar);

            GUILayout.BeginVertical();

            Utils.ActionButton("Add 1,000 Coins", buttonStyle, () => PlayerUtils.AddCoins(1000));
            Utils.ActionButton("Drop 1,000 Coins", buttonStyle, () => PlayerUtils.DropCoins(1000));
            Utils.ActionButton("Full Stamina", buttonStyle, () => PlayerUtils.SetFullStamina());

            infiniteStamina = Utils.ToggleButton("Infinite Stamina", infiniteStamina, buttonStyle,
                state => Logger.LogInfo($"Infinite Stamina {(state ? "Enabled" : "Disabled")}."));

            infiniteFood = Utils.ToggleButton("Infinite Food", infiniteFood, buttonStyle,
                state => Logger.LogInfo($"Infinite Food {(state ? "Enabled" : "Disabled")}."));

            godMode = Utils.ToggleButton("God Mode", godMode, buttonStyle,
                state => Logger.LogInfo($"God Mode {(state ? "Enabled" : "Disabled")}."));

            Utils.ActionButton("Unlock ALL Achievements", buttonStyle, PlayerUtils.UnlockAllAchievements);
            Utils.ActionButton("Revive All", buttonStyle, PlayerUtils.ReviveAllPlayers);
            Utils.ActionButton("Full Health", buttonStyle, PlayerUtils.SetFullHealth);
            Utils.ActionButton("Close Menu", buttonStyle, () => showMenu = false);

            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void InitStyles()
        {
            stylesInitialized = true;
            Color baseColor = new Color(0.05f, 0.05f, 0.05f);
            Color borderColor = new Color(0.2f, 0.2f, 0.2f);

            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;

            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontSize = 14;
            buttonStyle.normal.background = Utils.MakeTex(2, 2, new Color(0.12f, 0.12f, 0.12f));
            buttonStyle.hover.background = Utils.MakeTex(2, 2, new Color(0.18f, 0.18f, 0.18f));
            buttonStyle.active.background = Utils.MakeTex(2, 2, new Color(0.25f, 0.25f, 0.25f));

            buttonStyle.border = new RectOffset(2, 2, 2, 2);
            buttonStyle.margin = new RectOffset(4, 4, 4, 4);
            buttonStyle.padding = new RectOffset(6, 6, 4, 4);

            windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.fontSize = 16;
            windowStyle.normal.background = Utils.MakeTex(2, 2, baseColor);
            windowStyle.padding = new RectOffset(10, 10, 20, 10);
            windowStyle.border = new RectOffset(4, 4, 4, 4);

            scrollViewStyle = new GUIStyle(GUI.skin.scrollView);
            scrollViewStyle.normal.background = Utils.MakeTex(2, 2, baseColor);

            verticalScrollbarStyle = new GUIStyle(GUI.skin.verticalScrollbar);
            verticalScrollbarStyle.fixedWidth = 8;
            verticalScrollbarStyle.normal.background = Utils.MakeTex(2, 2, borderColor);

            verticalThumbStyle = new GUIStyle(GUI.skin.verticalScrollbarThumb);
            verticalThumbStyle.fixedWidth = 8;
            verticalThumbStyle.normal.background = Utils.MakeTex(2, 2, new Color(0.35f, 0.35f, 0.35f));
            verticalThumbStyle.hover.background = Utils.MakeTex(2, 2, new Color(0.5f, 0.5f, 0.5f));
        }
    }
}
