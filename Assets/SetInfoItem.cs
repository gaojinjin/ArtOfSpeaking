using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetInfoItem : MonoBehaviour
{
    public TextMeshProUGUI strInfoText;
    public void SetInfo(string info) {
        strInfoText.text = info;
    }
}
