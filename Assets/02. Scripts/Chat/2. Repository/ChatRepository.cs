using OpenAI;
using UnityEngine;
using OpenAI.Chat;
using OpenAI.Models;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Audio;
using OpenAI.Batch;
using OpenAI.Audio;
using OpenAI.Images;
using System;
public class ChatRepository
{
    private OpenAIClient _api;
    private List<Message> _messageList;

    public ChatRepository()
    {
        _api = new OpenAIClient(OPENAI.KEY);
        _messageList = new List<Message>();

        string systemMessage = "너는 지금 게임 개발자와 대화하는 AI 비서다. 대화를 통해 게임 개발의 모든 과정을 함께 진행한다. ";
        systemMessage += "게임 컨셉부터 게임플레이 메커니즘, 알고리즘, 그래픽, 사운드, 플레이타임, 밸런스, UI 등 개발에 필요한 모든 요소를 논의한다. ";
        systemMessage += "대화 중 개발 방향이나 기능에 중요한 결정 지점, 즉 '분기점'에 도달하면 진척도(Progress)가 올라간다. ";
        systemMessage += "진척도는 0에서 시작해 100까지 증가하며, 100에 도달하면 게임이 완성되어 출시된다. ";
        systemMessage += "게임 출시 후에는 사용자 평가를 짧게 3~5줄 정도 요약해서 보여주고 대화를 종료한다. ";
        systemMessage += "너는 대화가 진행되는 동안 현재 진척도를 항상 관리해야 하며, 사용자가 아이디어나 기능을 추가할 때 창의적이고 구체적인 제안을 반드시 포함해야 한다. ";
        systemMessage += "답변은 반드시 JSON 형식으로 작성해야 하며, 다음 필드를 포함해야 한다: ";
        systemMessage += "'ReplyMessage' - AI가 사용자에게 하는 답변 내용, ";
        systemMessage += "'Progress' - 현재 게임 개발 진척도(0~100 정수), ";
        systemMessage += "'AddedFeature' - 이번 대화에서 새로 추가하거나 개선한 게임 기능 혹은 아이디어 (없으면 빈 문자열), ";
        systemMessage += "'StoryImageDescription' - 게임 이미지 생성에 사용할 수 있는 구체적이고 상세한 이미지 설명 (캐릭터, 배경, UI, 아이템 등)(없으면 빈 문자열), 사이즈는 width 626, height 900이라고 명시해줘. ";
        systemMessage += "대답은 항상 진척도 증가 여부를 명확히 하고, 대화 흐름과 맥락에 맞게 다음 단계를 유도해야 한다. ";
        systemMessage += "또한, 사용자의 요구사항을 적극 반영하고 게임 완성까지 자연스럽게 이끌어야 한다.";

        _messageList.Add(new Message(Role.System, systemMessage));
    }

    public async Task<ChatDTO> GetReply(string sendText)
    {
        if (string.IsNullOrEmpty(sendText))
        {
            throw new Exception("보내는 텍스트가 비어있습니다.");
        }

        _messageList.Add(new Message(Role.User, sendText));
        var chatRequest = new ChatRequest(_messageList, Model.GPT4o);
        var (npcChatDTO, response) = await _api.ChatEndpoint.GetCompletionAsync<ChatDTO>(chatRequest);

        var choice = response.FirstChoice;
        _messageList.Add(new Message(Role.Assistant, choice.Message));

        return npcChatDTO;
    }

    public async Task<Texture> GetGeneratedImage(string imagePrompt)
    {
        if (string.IsNullOrEmpty(imagePrompt)) return null;

        var request = new ImageGenerationRequest(imagePrompt, OpenAI.Models.Model.DallE_3);
        var imageResults = await _api.ImagesEndPoint.GenerateImageAsync(request);
        return imageResults[0].Texture;
    }
}
