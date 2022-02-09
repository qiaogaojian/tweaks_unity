using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UIAssetBundle : BaseView
{
    private Button btnReturn;
    private Button btnInsantiate;

    private Transform root;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();
        root      = transform.Find("ivBg");

        btnInsantiate = root.Find("btnInsantiate").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
        btnInsantiate.onClick.AddListener(OnClickBtnInsantiate);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
        btnInsantiate.onClick.RemoveListener(OnClickBtnInsantiate);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void OnClickBtnInsantiate()
    {
        InsantiateAsset();
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
        Instantiate(asset, root);
        localAssetBundle.Unload(false);
    }
}