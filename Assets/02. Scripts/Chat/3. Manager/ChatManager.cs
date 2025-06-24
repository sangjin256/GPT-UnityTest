using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ChatManager : BehaviourSingleton<ChatManager>
{
    private ChatRepository _repository;
    private List<ChatDTO> _chatList;

    private void Start()
    {
        _repository = new ChatRepository();
        _chatList = new List<ChatDTO>();
    }

    public async Task<ChatDTO> GetReply(string sendText)
    {
        ChatDTO dto = await _repository.GetReply(sendText);
        _chatList.Add(dto);

        return dto;
    }
}
