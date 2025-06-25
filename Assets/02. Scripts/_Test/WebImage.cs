﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class WebImage : MonoBehaviour
{
    public RawImage MyImage;
    void Start()
    {
        StartCoroutine(GetTexture());
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://cdn.hankooki.com/news/photo/202409/192597_266586_1726985307.jpg");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            MyImage.texture = myTexture;
        }
    }
}
