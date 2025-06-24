using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class TypecastTTS : MonoBehaviour
{
    private const string API_KEY = "__pltFnBf8fX8RMaMNyvGTD75L3yhBV8Mz8hUQ5kD4NRX";
    private const string VOICE_ID = "63a3d9da4b235ddd6541a795";

    public static async Task PlayTypecastTTS(string text, AudioSource audiosource)
    {
        string audioUrl = await GetTypecastAudioUrlAsync(text);
        if (!string.IsNullOrEmpty(audioUrl))
        {
            AudioClip clip = await DownloadAudioClipAsync(audioUrl);
            if (clip != null)
            {
                audiosource.clip = clip;
                audiosource.Play();
            }
        }
    }

    private static async Task<string> GetTypecastAudioUrlAsync(string text)
    {
        string url = "https://typecast.ai/api/speak";
        var requestData = new
        {
            text = text,
            lang = "auto",
            model_version = "latest",
            emotion_tone_preset = "neutral", // 예: "neutral", "happy", "sad" 등
            actor_id = VOICE_ID, // 실제 24자리 actor_id 문자열
            xapi_hd = true,
            xapi_audio_format = "mp3",
            max_seconds = 20,
            volume = 100,
            speed_x = 1f,
            tempo = 1f,
            pitch = 0
        };

        string json = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {API_KEY}");

            var asyncOp = request.SendWebRequest();
            while (!asyncOp.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Typecast가 반환하는 JSON 예시: { "audioUrl": "https://..." }
                string responseJson = request.downloadHandler.text;
                var response = JsonUtility.FromJson<TypecastResponse>(responseJson);
                return response.audioUrl;
            }
            else
            {
                Debug.LogError($"API 요청 실패: {request.responseCode} - {request.error}");
                Debug.LogError(request.downloadHandler.text);
                return null;
            }
        }
    }

    private static async Task<AudioClip> DownloadAudioClipAsync(string audioUrl)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.MPEG))
        {
            var asyncOp = request.SendWebRequest();
            while (!asyncOp.isDone) await Task.Yield();

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
    private class TypecastResponse
    {
        public string audioUrl;
    }
}
