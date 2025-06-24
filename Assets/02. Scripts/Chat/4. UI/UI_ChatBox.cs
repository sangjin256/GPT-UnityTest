
using TMPro;
using UnityEngine;

public class UI_ChatBox : MonoBehaviour
{
    public TextMeshProUGUI TextUI;

    public void Init(string text)
    {
        TextUI.text = text;
    }
}
