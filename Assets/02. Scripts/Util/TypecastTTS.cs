using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class TypecastTTS
{
    private const string VOICE_ID = "63a3d9da4b235ddd6541a795";

    public static async Task<AudioClip> GetTTSClip(string text)
    {
        string audioUrl = await RequestTypecastAudioUrl(text);
        if (!string.IsNullOrEmpty(audioUrl))
        {
            AudioClip clip = await DownloadAudioClipAsync(audioUrl);
            if (clip != null)
            {
                return clip;
            }
        }

        return null;
    }

    private static async Task<string> RequestTypecastAudioUrl(string text)
    {
        var urlPost = "https://typecast.ai/api/speak";
        var req = new SpeakPostRequest
        {
            text = text,
            tts_mode = "actor",
            actor_id = VOICE_ID,
            lang = "auto",
            model_version = "latest",
            xapi_hd = true,
            xapi_audio_format = "mp3",
            max_seconds = 20,
            volume = 100,
            speed_x = 1f,
            tempo = 1f,
            pitch = 0
        };
        string body = JsonUtility.ToJson(req);

        using (var request = new UnityWebRequest(urlPost, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {TYPECAST.API_KEY}");
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body));
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("POST 오류: " + request.downloadHandler.text);
                return null;
            }

            var res = JsonUtility.FromJson<SpeakPostResponse>(request.downloadHandler.text);
            string statusUrl = res.result.speak_v2_url;

            // (2) 상태 폴링
            while (true)
            {
                using (var statusReq = UnityWebRequest.Get(statusUrl))
                {
                    statusReq.SetRequestHeader("Authorization", $"Bearer {TYPECAST.API_KEY}");
                    await statusReq.SendWebRequest();

                    if (statusReq.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("GET 상태 오류: " + statusReq.downloadHandler.text);
                        return null;
                    }

                    var statusRes = JsonUtility.FromJson<SpeakStatusResponse>(statusReq.downloadHandler.text);
                    if (statusRes.result.status == "done")
                    {
                        return statusRes.result.audio_download_url;
                    }

                    await Task.Delay(500); // 0.5초마다 체크
                }
            }
        }
    }

    private static async Task<AudioClip> DownloadAudioClipAsync(string audioUrl)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.MPEG))
        {
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerAudioClip.GetContent(request);
            }
            else
            {
                Debug.LogError($"오디오 다운로드 실패: {request.error}");
                return null;
            }
        }
    }

    [System.Serializable]
    private class SpeakPostRequest
    {
        public string text;
        public string tts_mode;
        public string actor_id;
        public string lang;
        public string model_version;
        public bool xapi_hd;
        public string xapi_audio_format;
        public int max_seconds;
        public int volume;
        public float speed_x;
        public float tempo;
        public int pitch;
    }

    [Serializable]
    private class SpeakPostResponse
    {
        public Result result;
        [Serializable]
        public class Result
        {
            public string speak_v2_url;
        }
    }

    [Serializable]
    private class SpeakStatusResponse
    {
        public Result result;
        [Serializable]
        public class Result
        {
            [JsonProperty("status")]
            public string status;
            [JsonProperty("audio_download_url")]
            public string audio_download_url;
        }
    }


}
