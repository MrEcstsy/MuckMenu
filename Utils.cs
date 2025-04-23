using UnityEngine;
using System;

namespace MuckMenu
{
    public static class Utils
    {
        public static Texture2D MakeTex(int width, int height, Color col)
        {
            var pix    = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public static bool ToggleButton(string label, bool current, GUIStyle style, Action<bool> onToggle = null)
        {
            GUI.color = current ? Color.green : Color.white;
            if (GUILayout.Button($"{label}: {(current ? "ON" : "OFF")}", style))
            {
                current = !current;
                onToggle?.Invoke(current);
            }
            GUI.color = Color.white;
            return current;
        }

        public static void ActionButton(string label, GUIStyle style, Action onClick, params GUILayoutOption[] opts)
        {
            if (GUILayout.Button(label, style, opts))
                onClick();
        }

    }
}
