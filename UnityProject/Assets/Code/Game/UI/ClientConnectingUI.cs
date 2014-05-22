using UnityEngine;
using System.Collections;

public class ClientConnectingUI : MonoBehaviour
{
    private void DrawConnecting() {
        GUI.Box(new Rect(Screen.width * 0.5f - 100f, 50f, 200f, 180f), "CONNECTING ...");
    }

    public void SetConnectingState() {

    }
}
