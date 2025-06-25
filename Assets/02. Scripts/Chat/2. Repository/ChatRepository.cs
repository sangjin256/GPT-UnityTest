using OpenAI;
using UnityEngine;
using OpenAI.Chat;
using OpenAI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Audio;
using OpenAI.Batch;
using OpenAI.Audio;
using OpenAI.Images;
using System;
public class ChatRepository
{
    private OpenAIClient _api;

    public ChatRepository()
    {
        _api = new OpenAIClient(OPENAI.KEY);
    }

    public async Task<ChatDTO> GetReply(List<Message> messageList)
    {
        var chatRequest = new ChatRequest(messageList, Model.GPT4o);
        var (npcChatDTO, response) = await _api.ChatEndpoint.GetCompletionAsync<ChatDTO>(chatRequest);
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
