using UnityEngine;
using System.Collections;

public class OptionsUI : MonoBehaviour
{
    private MenuUI menuUI;

    void Start() {
        menuUI = GetComponent<MenuUI>();
    }

    private void DrawOptionsMenu() {
        GUI.Box(new Rect(Screen.width * 0.5f - 100f, 50f, 200f, 180f), "TODO: Make options");

        if (GUI.Button(new Rect(Screen.width * 0.5f - 50f, Screen.height * 0.7f, 100f, 30f), "Back"))
            menuUI.SetMainMenuState();
    }

    public void SetOptionsMenuState() {
        DrawUI.instance.drawUI = DrawOptionsMenu;
    }
}
