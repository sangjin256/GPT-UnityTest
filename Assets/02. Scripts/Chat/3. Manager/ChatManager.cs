using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ChatManager : BehaviourSingleton<ChatManager>
{
    private ChatRepository _repository;
    private List<ChatDTO> _chatList;

    [SerializeField] private AudioSource _myAudioSource;

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

    public async Task PlayTTS(string text)
    {
        AudioClip clip = await TypecastTTS.GetTTSClip(text);

        if (_myAudioSource.isPlaying) _myAudioSource.Stop();

        _myAudioSource.clip = clip;
        _myAudioSource.Play();
    }

    public async Task<Texture> GetImage(ChatDTO chat)
    {
        return await _repository.GetGeneratedImage(chat.StoryImageDescription);
    }
}
