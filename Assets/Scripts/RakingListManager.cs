using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理排行榜
/// </summary>
public class RakingListManager : MonoBehaviour
{
    public Transform parTran, itemTran;
    public Button closeBut;
    private void Start()
    {
        closeBut.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });
    }
    private void OnEnable()
    {
        //激活的时候刷新内容
        foreach (Transform item in parTran)
        {
            Destroy(item.gameObject);
        }
        //获取文本并且汇入物体
        for (int i = 0; i < WebrequestManager.Instance.contents.Count; i++)
        {
            SetInfoItem go = Instantiate(itemTran, parTran).GetComponent<SetInfoItem>();
            go.SetInfo(WebrequestManager.Instance.contents[i]);
        }
    }
}
