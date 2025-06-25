using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class UI_Chat : MonoBehaviour
{
    public TMP_InputField InputField;
    public ScrollRect ChatScrollRect;
    public RawImage RawImage;

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

        ChatManager.Instance.PlayTTS(replyDTO.ReplyMessage);

        UI_ChatBox OtherChatBox = Instantiate(OtherChatBoxPrefab, ChatBoxParent).GetComponent<UI_ChatBox>();
        OtherChatBox.Init($"{replyDTO.ReplyMessage}\n\n진척도 : {replyDTO.Progress}");
        RefreshLayout();

        Texture texture = await ChatManager.Instance.GetImage(replyDTO);
        if(texture != null)
        {
            RawImage.texture = texture;
        }
    }

    private async void RefreshLayout()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_chatBoxRectTransform);

        await Task.Yield();
        await Task.Yield();
        await Task.Yield();

        ChatScrollRect.verticalNormalizedPosition = 0f;
    }
}
