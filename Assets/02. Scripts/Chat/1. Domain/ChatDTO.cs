using Newtonsoft.Json;
using UnityEngine;
using System;

[Serializable]
public class ChatDTO
{
    [JsonProperty("ReplyMesssage")]
    public string ReplyMessage { get; set; }
    [JsonProperty("Progress")]
    public string Progress { get; set; }
    [JsonProperty("AddedFeature")]
    public string AddedFeature { get; set; }
    [JsonProperty("StoryImageDescription")]
    public string StoryImageDescription { get; set; }

    public ChatDTO() { }

    public ChatDTO(string replyMessage, string progress, string addedFeature, string storyImageDescription)
    {
        ReplyMessage = replyMessage;
        Progress = progress;
        AddedFeature = addedFeature;
        StoryImageDescription = storyImageDescription;
    }
}
