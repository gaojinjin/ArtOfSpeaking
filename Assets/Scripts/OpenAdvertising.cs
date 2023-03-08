using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class OpenAdvertising : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // 对于不受支持的平台，此值将保持为 null
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = false;
    private bool oneTime=false;
    private string _gameId;

    void Awake()
    {
        InitializeAds();
        // 获取当前平台的 Ad Unit ID（广告单元 ID）：
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // 在准备好展示广告之前禁用该按钮：
        _showAdButton.interactable = false;

        
        
    }
    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    // 将内容加载到广告单元中：
    public void LoadAd()
    {
        // 重要！仅在初始化之后再加载内容（在此示例中，初始化在另一个脚本中处理）。
        //Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // 如果广告加载成功，请向按钮添加监听器并启用它：
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // 配置该按钮在单击时调用 ShowAd() 方法：
            
                _showAdButton.onClick.AddListener(ShowAd);

            // 允许用户点击按钮：
            _showAdButton.interactable = true;
        }
    }

    // 实现当用户点击按钮时执行的方法：
    public void ShowAd()
    {
    if (!oneTime)
    {
        // 禁用该按钮：
        _showAdButton.interactable = false;
        // 然后展示广告：
        Advertisement.Show(_adUnitId, this);
        oneTime = true;
            }
    }

    // 实现 Show Listener 的 OnUnityAdsShowComplete 回调方法来判断用户是否获得奖励：
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // 给予奖励。

            // 加载另一个广告：
            Advertisement.Load(_adUnitId, this);
        }
    }

    // 实现 Load 和 Show Listener 错误回调：
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // 使用错误详细信息来确定是否要尝试加载另一个广告。
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // 使用错误详细信息来确定是否要尝试加载另一个广告。
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // 清理按钮的监听器：
        _showAdButton.onClick.RemoveAllListeners();
    }
    public void OnInitializationComplete()
    {
        //加载广告
        LoadAd();
        //播放加载广告的视频
        //ShowAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}