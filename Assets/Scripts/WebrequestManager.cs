using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System;
using HTC.UnityPlugin.Utility;

public class WebrequestManager : SingletonBehaviour<WebrequestManager>
{
    public string[] strings;
    public TextMeshProUGUI textMeshPro;
    private string tempUrl;
    public Button rankingListBut, roleChooseBut,nextBut;
    private bool isMan;
    public List<string> contents;
    public GameObject rakingListGo;
    void Start()
    {
        roleChooseBut.onClick.AddListener(() => {
            //角色切换 文本修改
            isMan = !isMan;
            roleChooseBut.GetComponentInChildren<TextMeshProUGUI>().text = isMan? "绿茶" : "渣男";
            GetURL(isMan? strings[0] : strings[1], URLType.Single);
        });
        rankingListBut.onClick.AddListener(() =>
        {
            //排行榜 呼叫创建排行榜  更新内容呼叫列表
            GetURL(strings[2], URLType.RangkList);
            
        });
        nextBut.onClick.AddListener(() => {
            GetURL(isMan ? strings[0] : strings[1], URLType.Single);
        });
        GetURL(isMan ? strings[0] : strings[1], URLType.Single);
    }
    private void GetURL(string url, URLType uRLType) {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        StartCoroutine(RankingList(unityWebRequest, uRLType));
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
            switch (uRLType)
            {
                case URLType.None:
                    break;
                case URLType.Single:
                    string content = node["returnObj"]["content"].Value;
                    textMeshPro.text = content;
                    break;
                case URLType.RangkList:
                    contents = new List<string>();
                    for (int i = 0; i < node["returnObj"].Count; i++)
                    {
                        contents.Add(node["returnObj"][i]["content"].Value);
                    }
                    rakingListGo.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
public enum URLType { 
    None,
    Single,
    RangkList
}