using UnityEngine;
using System;
using NUnit.Framework.Constraints;

public class Chat
{
    public string ReplyMessage { get; private set; }
    public string Progress { get; private set; }
    public string AddedFeature { get; private set; }
    public string StoryImageDescription { get; private set; }

    public Chat(string replyMessage, string progress, string addedFeature, string storyImageDescription)
    {
        if (string.IsNullOrEmpty(replyMessage))
        {
            throw new Exception("응답 메세지는 비어있을 수 없습니다.");
        }
        if (string.IsNullOrEmpty(progress))
        {
            throw new Exception("진척도는 비어있을 수 없습니다.");
        }

        ReplyMessage = replyMessage;
        Progress = progress;
        AddedFeature = addedFeature;
        StoryImageDescription = storyImageDescription;
    }

    public ChatDTO ToDTO()
    {
        return new ChatDTO(ReplyMessage, Progress, AddedFeature, StoryImageDescription);
    }
}
