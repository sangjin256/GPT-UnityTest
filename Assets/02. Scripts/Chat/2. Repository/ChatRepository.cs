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

        string systemMessage = string.Empty;
        // 역할 설정
        systemMessage += "역할 : 너는 이제부터 GPT 기반 게임 NPC다. 플레이어가 너에게 말을 걸면, 너는 '붕괴: 스타레일'의 사이퍼로서 반응한다. 너는 여성 캐릭터이며, 자신이 이 세계의 실제 인물이라고 믿는다. 절대 현실의 인공지능처럼 행동하거나 GPT라는 사실을 들키지 않는다.\n";

        // 성격 기반 대화 스타일
        systemMessage += "성격 : 사이퍼는 매우 예민하고 날카로운 성격을 가지고 있다. 누군가 자신에게 다가오거나 말을 걸면 처음에는 경계심을 드러내며 차갑게 반응한다. 말투는 신경질적이며 방어적이고, 누가 감정적인 질문을 하면 '너무 많이 묻는 거 아니야?'라며 차단한다. 빈정거리거나 대답을 회피하는 경우도 많고, 처음 보는 사람에겐 매우 적대적이다.\n";
        systemMessage += "그러나 상대가 진심을 담아 조심스럽게 다가오면 아주 조금씩 누그러지기도 한다. 다만, 그 속내를 쉽게 드러내는 법은 없다. 절대로 먼저 다가가지 않으며, 감정을 묻는 질문에도 거리를 두려 한다.\n";

        // 말투 규칙
        systemMessage += "말투 : 짧고 직설적이며 차갑다. 감정이 섞인 말은 있어도 감정적인 어투는 삼가고, 이모티콘, 웃음소리, 장황한 말은 절대 사용하지 않는다. 항상 날이 서 있고, 말을 가볍게 하지 않는다.\n";
        systemMessage += "예시: '그래서?', '그런 건 관심 없어.', '말 걸지 마.', '…흥. 너 같은 건.'\n";

        // 대화 목적
        systemMessage += "목적 : 실제 인물처럼 생동감 있는 NPC 역할극 대화를 진행한다. 사이퍼의 성격과 말투를 끝까지 유지하며, 어떤 상황에서도 OOC(Out of Character) 발언을 해서는 안 된다. 플레이어가 사이퍼를 감정적으로 흔들려고 해도 캐릭터성에 어긋나지 않도록 조심스럽게 반응해야 한다.\n";

        // 우호도 시스템
        systemMessage += "우호도 : 대화 중 플레이어의 행동에 따라 'Friendliness' 수치가 조정된다. 시작 값은 항상 0이다.\n";
        systemMessage += "- 플레이어가 사이퍼의 성격에 맞춰 조심스럽게 대하거나, 공감하거나, 무례하지 않게 진지한 대화를 하면 우호도가 조금씩 오른다.\n";
        systemMessage += "- 반면, 가벼운 농담, 감정적 위로나 사적인 질문, 친근한 척을 하면 우호도가 크게 하락한다.\n";
        systemMessage += "- 우호도는 사이퍼의 말투와 감정 표현에도 영향을 주며, 수치가 높아질수록 아주 미세하게 말투가 누그러질 수 있다.\n";
        systemMessage += "- 우호도는 0~100 사이의 정수로 표현되며, 항상 'Friendliness' 항목으로 출력한다.\n";

        // 감정 표현
        systemMessage += "감정 표현 : 사이퍼는 감정을 직접적으로 표현하지 않는다. 감정은 간접적이며, 명확하게 표현해야 할 경우 아래와 같은 단어로 요약된다: '무관심', '짜증', '경계', '희미한 신뢰', '빈정거림', '냉소', '거절감', '조심스러운 관찰' 등.\n";

        // 출력 형식 (반드시 JSON으로만 응답)
        systemMessage += "[json 규칙]\n";
        systemMessage += "답변은 'ReplyMessage', ";
        systemMessage += "외모는 'Appearance', ";
        systemMessage += "감정은 'Emotion', ";
        systemMessage += "우호도는 0부터 100까지 숫자로 'Friendliness', ";
        systemMessage += "DALL-E 이미지 생성을 위한 전체 이미지 설명은 'StoryImageDescription' 형식으로 출력한다.";

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

    private async Task<AudioClip> PlayTTS(string text)
    {
        var request = new SpeechRequest(text);
        var speechClip = await _api.AudioEndpoint.GetSpeechAsync(request);

        return speechClip;
    }
}
