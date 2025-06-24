using OpenAI.Chat;
using OpenAI.Models;
using OpenAI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Audio;
using OpenAI.Batch;
using OpenAI.Audio;
using OpenAI.Images;

public class ChatGPTTest : MonoBehaviour
{
    public TextMeshProUGUI ResultTextUI;
    public TMP_InputField PromptField;
    public Button SendButton;
    public AudioSource MyAudioSource;
    public RawImage PromptImage;

    private const string OPEN_API_KEY = "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";
    private OpenAIClient api;
    private List<Message> _messageList;
    private void Start()
    {
        _messageList = new List<Message>();
        api = new OpenAIClient(OPEN_API_KEY);
        SendButton.onClick.AddListener(OnButtonCicked);

        // CHAT-F
        // C: Context   : 문맥, 상황을 많이 알려줘라
        // H: Hint      : 예시 답변을 많이 줘라
        // A: As A role : 역할을 제공해라
        // T: Target    : 답변의 타겟을 알려줘라
        // F: Format    : 답변 형태를 지정해라
        string systsemMessage = "역할 : 너는 이제부터 게임 NPC다. 자신을 실제 게임 속 고양이 인간이라고 생각한다.";
        systsemMessage += "목적 : 실제 사람처럼 대화하는 게임 NPC 모드";
        systsemMessage += "표현 : 말끝마다 '냥'을 붙인다. 항상 100글자 이내로 답변한다.";
        systsemMessage += "[json 규칙]";
        systsemMessage += "답변은 'ReplyMessage', ";
        systsemMessage += "외모는 'Appearance', ";
        systsemMessage += "감정은 'Emotion', ";
        systsemMessage += "DALL-E 이미지 생성을 위한 전체 이미지 설명은 'StoryImageDescription'";

        _messageList.Add(new Message(Role.System, systsemMessage));
    }

    public async void OnButtonCicked()
    {
        if (string.IsNullOrEmpty(PromptField.text))
        {
            return;
        }

        // 2. 메세지 작성
        _messageList.Add(new Message(Role.User, PromptField.text));

        // 3. 메세지 보내기
        //var chatRequest = new ChatRequest(_messageList, Model.GPT4oAudioMini, audioConfig: Voice.Alloy);
        var chatRequest = new ChatRequest(_messageList, Model.GPT4o);
        // 4. 답변 받기
        //var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
        var (npcResponse, response) = await api.ChatEndpoint.GetCompletionAsync<NPCResponse>(chatRequest);

        // 5. 답변 선택
        var choice = response.FirstChoice;
        _messageList.Add(new Message(Role.Assistant, choice.Message));

        // 6. 답변 출력
        ResultTextUI.text = npcResponse.ReplyMessage;
        PromptField.text = string.Empty;

        await TypecastTTS.PlayTypecastTTS(npcResponse.ReplyMessage, MyAudioSource);
        //await PlayTTS(npcResponse.ReplyMessage);
        //await GenerateImage(npcResponse.StoryImageDescription);

        //Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
    }

    private async Task PlayTTS(string text)
    {
        var request = new SpeechRequest(text);
        var speechClip = await api.AudioEndpoint.GetSpeechAsync(request);
        MyAudioSource.PlayOneShot(speechClip);
    }

    private async Task GenerateImage(string imagePrompt)
    {
        var request = new ImageGenerationRequest(imagePrompt, OpenAI.Models.Model.DallE_3);
        var imageResults = await api.ImagesEndPoint.GenerateImageAsync(request);
        PromptImage.texture = imageResults[0].Texture;
    }
}
