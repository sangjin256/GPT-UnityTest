using UnityEngine;
using System;
using NUnit.Framework.Constraints;

public class Chat
{
    public string ReplyMessage { get; private set; }
    public string Appearance { get; private set; }
    public string Emotion { get; private set; }
    public string StoryImageDescription { get; private set; }
    public string Friendliness { get; private set; }

    public Chat(string replyMessage, string appearance, string emotion, string storyImageDescription, string friendliness)
    {
        if (string.IsNullOrEmpty(replyMessage))
        {
            throw new Exception("응답 메세지는 비어있을 수 없습니다.");
        }
        if (string.IsNullOrEmpty(appearance))
        {
            throw new Exception("외모 설명은 비어있을 수 없습니다.");
        }
        if (string.IsNullOrEmpty(emotion))
        {
            throw new Exception("감정 설명은 비어있을 수 없습니다.");
        }
        if (string.IsNullOrEmpty(friendliness))
        {
            throw new Exception("우호도는 비어있을 수 없습니다.");
        }

        ReplyMessage = replyMessage;
        Appearance = appearance;
        Emotion = emotion;
        StoryImageDescription = storyImageDescription;
        Friendliness = friendliness;
    }

    public ChatDTO ToDTO()
    {
        return new ChatDTO(ReplyMessage, Appearance, Emotion, StoryImageDescription, Friendliness);
    }
}
