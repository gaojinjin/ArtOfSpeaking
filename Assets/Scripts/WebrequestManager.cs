using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System;

public class WebrequestManager : MonoBehaviour
{
    public string[] strings;
    public TextMeshProUGUI textMeshPro;
    private string tempUrl;
    public Button rankingListBut, roleChooseBut;
    private bool isMan;
    void Start()
    {
        roleChooseBut.onClick.AddListener(() => {
            //角色切换 文本修改
            isMan = !isMan;
            GetComponentInChildren<TextMeshProUGUI>().text = isMan?"渣男":"绿茶";
            GetURL(isMan? strings[0] : strings[1]);
        });
        rankingListBut.onClick.AddListener(() =>
        {
            //排行榜 呼叫创建排行榜

        });
    }
    private void GetURL(string url) {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        StartCoroutine(RankingList(unityWebRequest,URLType.Single));
    }

    /// <summary>
    /// 解析字符串
    /// </summary>
    /// <param name="unityWebRequest">解析连接</param>
    /// <returns>获取Json文件</returns>
    private IEnumerator GetUrl(UnityWebRequest unityWebRequest)
    {
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            //Debug.Log(unityWebRequest.downloadHandler.text);
            //解析字符串
            string jsonStr = unityWebRequest.downloadHandler.text;
            JSONNode node = JSON.Parse(jsonStr);
            string content = node["returnObj"]["content"].Value;
            textMeshPro.text = content;
        }
    }
    private IEnumerator RankingList(UnityWebRequest unityWebRequest, URLType uRLType)
    {
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            string jsonStr = unityWebRequest.downloadHandler.text;
            JSONNode node = JSON.Parse(jsonStr);
            List<string> contents = new List<string>();
            for (int i = 0; i < node["returnObj"].Count; i++)
            {
                contents.Add(node["returnObj"][i]["content"].Value);
            }
            foreach (var item in contents)
            {
                Debug.Log(item);
            }
        }
    }
}
public enum URLType { 
    None,
    Single,
    RangkList
}