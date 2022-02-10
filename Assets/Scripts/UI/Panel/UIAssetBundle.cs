using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UIAssetBundle : BaseView
{
    private Button btnReturn;
    private Button btnInsantiate;
    private Button btnInsantiateAsync;

    private Transform root;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();
        root      = transform.Find("ivBg");

        btnInsantiate      = root.Find("btnInsantiate").GetComponent<Button>();
        btnInsantiateAsync = root.Find("btnInsantiateAsync").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
        btnInsantiate.onClick.AddListener(OnClickBtnInsantiate);
        btnInsantiateAsync.onClick.AddListener(OnClickBtnInsantiateAsync);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
        btnInsantiate.onClick.RemoveListener(OnClickBtnInsantiate);
        btnInsantiateAsync.onClick.RemoveListener(OnClickBtnInsantiateAsync);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void OnClickBtnInsantiate()
    {
        InsantiateAsset();
    }

    private void OnClickBtnInsantiateAsync()
    {
        StartCoroutine(InsantiateAssetAsync());
    }

    private string assetName  = "BundledSpriteObject";
    private string bundleName = "testbundle";

    private void InsantiateAsset()
    {
        AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));

        if (localAssetBundle == null)
        {
            Debuger.LogError("Failed to load AssetBundle!");
            return;
        }

        GameObject asset = localAssetBundle.LoadAsset<GameObject>(assetName);
        Instantiate(asset, root).transform.DOMoveX(-1,0.1f);
        localAssetBundle.Unload(false);
    }

    IEnumerator InsantiateAssetAsync()
    {
        AssetBundleCreateRequest asyncBundleRequest = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, bundleName));
        yield return asyncBundleRequest;
        AssetBundle localAssetBundle = asyncBundleRequest.assetBundle;

        if (localAssetBundle == null)
        {
            Debuger.LogError("Failed to load AssetBundle!");
            yield break;
        }

        AssetBundleRequest assetRequest = localAssetBundle.LoadAssetAsync<GameObject>(assetName);
        yield return assetRequest;

        GameObject prefab = assetRequest.asset as GameObject;
        Instantiate(prefab, root).transform.DOMoveX(1,0.1f);
        localAssetBundle.Unload(false);
    }
}