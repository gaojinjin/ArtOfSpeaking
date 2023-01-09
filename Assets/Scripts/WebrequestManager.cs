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
            //��ɫ�л� �ı��޸�
            isMan = !isMan;
            GetComponentInChildren<TextMeshProUGUI>().text = isMan?"����":"�̲�";
            GetURL(isMan? strings[0] : strings[1]);
        });
        rankingListBut.onClick.AddListener(() =>
        {
            //���а� ���д������а�

        });
    }
    private void GetURL(string url) {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        StartCoroutine(RankingList(unityWebRequest,URLType.Single));
    }

    /// <summary>
    /// �����ַ���
    /// </summary>
    /// <param name="unityWebRequest">��������</param>
    /// <returns>��ȡJson�ļ�</returns>
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
            //�����ַ���
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