using Newtonsoft.Json;
using UnityEngine;
using System;

[Serializable]
public class ChatDTO
{
    [JsonProperty("ReplyMesssage")]
    public string ReplyMessage { get; set; }
    [JsonProperty("Appearance")]
    public string Appearance { get; set; }
    [JsonProperty("Emotion")]
    public string Emotion { get; set; }
    [JsonProperty("StoryImageDescription")]
    public string StoryImageDescription { get; set; }
    [JsonProperty("Friendliness")]
    public string Friendliness { get; set; }

    public ChatDTO() { }

    public ChatDTO(string replyMessage, string appearance, string emotion, string storyImageDescription, string friendliness)
    {
        ReplyMessage = replyMessage;
        Appearance = appearance;
        Emotion = emotion;
        StoryImageDescription = storyImageDescription;
        Friendliness = friendliness;
    }   
}
