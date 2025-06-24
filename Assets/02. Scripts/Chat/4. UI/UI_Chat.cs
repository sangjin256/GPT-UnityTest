using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class UI_Chat : MonoBehaviour
{
    public TMP_InputField InputField;
    public ScrollRect ChatScrollRect;

    public GameObject UserChatBoxPrefab;
    public GameObject OtherChatBoxPrefab;

    public Transform ChatBoxParent;
    private RectTransform _chatBoxRectTransform;

    private void Start()
    {
        _chatBoxRectTransform = ChatBoxParent.GetComponent<RectTransform>();
    }

    public async void OnClickSendButton()
    {
        if (string.IsNullOrEmpty(InputField.text))
        {
            return;
        }

        UI_ChatBox UserChatBox = Instantiate(UserChatBoxPrefab, ChatBoxParent).GetComponent<UI_ChatBox>();
        UserChatBox.Init(InputField.text);
        RefreshLayout();

        string sendText = InputField.text;
        InputField.text = string.Empty;

        ChatDTO replyDTO = await ChatManager.Instance.GetReply(sendText);

        UI_ChatBox OtherChatBox = Instantiate(OtherChatBoxPrefab, ChatBoxParent).GetComponent<UI_ChatBox>();
        OtherChatBox.Init($"{replyDTO.ReplyMessage}\n\n우호도 : {replyDTO.Friendliness}");
        RefreshLayout();
    }

    private async void RefreshLayout()
    {
        await Task.Yield();
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatBoxRectTransform);
        await Task.Yield();
        ChatScrollRect.verticalNormalizedPosition = 0f;
    }
}
