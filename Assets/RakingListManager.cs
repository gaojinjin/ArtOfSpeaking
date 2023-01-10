using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������а�
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
        //�����ʱ��ˢ������
        foreach (Transform item in parTran)
        {
            Destroy(item.gameObject);
        }
        //��ȡ�ı����һ�������
        for (int i = 0; i < WebrequestManager.Instance.contents.Count; i++)
        {
            SetInfoItem go = Instantiate(itemTran, parTran).GetComponent<SetInfoItem>();
            go.SetInfo(WebrequestManager.Instance.contents[i]);
        }
    }
}
