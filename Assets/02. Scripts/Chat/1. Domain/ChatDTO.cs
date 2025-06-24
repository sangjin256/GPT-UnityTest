using UnityEngine;

[System.Serializable]
public class ChatDTO
{
    public string ReplyMessage;
    public string Appearance;
    public string Emotion;
    public string StoryImageDescription;

    public ChatDTO() { }

    public ChatDTO(string replyMessage, string appearance, string emotion, string storyImageDescription)
    {
        ReplyMessage = replyMessage;
        Appearance = appearance;
        Emotion = emotion;
        StoryImageDescription = storyImageDescription;
    }
}
