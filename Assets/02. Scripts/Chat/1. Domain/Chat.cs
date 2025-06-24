using UnityEngine;
using System;

public class Chat
{
    public string ReplyMessage { get; private set; }
    public string Appearance { get; private set; }
    public string Emotion { get; private set; }
    public string StoryImageDescription { get; private set; }

    public Chat(string replyMessage, string appearance, string emotion, string storyImageDescription)
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

        ReplyMessage = replyMessage;
        Appearance = appearance;
        Emotion = emotion;
        StoryImageDescription = storyImageDescription;
    }
}
