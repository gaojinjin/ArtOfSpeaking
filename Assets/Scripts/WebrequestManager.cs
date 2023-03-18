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
    private bool isMan=true;
    public List<string> contents;
    public Sprite seaManSpr, greenGirlSp;
    public TextMeshProUGUI butText;
    public GameObject rakingListGo;
    public Button copyBut;
    
    void Start()
    {
        //复制按钮
        copyBut.onClick.AddListener(()=>{
            GUIUtility.systemCopyBuffer = textMeshPro.text;
            ToastText("复制成功");
        });
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        roleChooseBut.onClick.AddListener(() => {
            //��ɫ�л� �ı��޸� 中文 测试
            isMan = !isMan;
            
            roleChooseBut.GetComponent<Image>().sprite = isMan? seaManSpr : greenGirlSp;
            // change text

            butText.text = isMan ? "渣男" : "绿茶";
            GetURL(isMan? strings[0] : strings[1], URLType.Single);
        });
        rankingListBut.onClick.AddListener(() =>
        {
            //���а� ��д������а�  �������ݺ���б�
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
    private void ToastText(string showStr)
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"); ;
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            Toast.CallStatic<AndroidJavaObject>("makeText", context, showStr, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
        }
        ));
    }
}
public enum URLType { 
    None,
    Single,
    RangkList
}