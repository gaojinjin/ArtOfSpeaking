using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsTip : MonoBehaviour
{
    private bool isShow = false;
    void Start()
    {
        InvokeRepeating("ShowHideAds",1,5);

    }

    
    void ShowHideAds() {
        isShow = !isShow;
        gameObject.SetActive(isShow);
    }
}
