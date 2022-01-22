using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UIPool : BaseView
{
    private Button btnReturn;
    private Button btnGetPool;
    private Button btnCreatePersistPool;
    private Button btnCreatePrefabPool;

    private string pool_btnGetPool           = "pool_btnGetPool";
    private string pool_btnCreatePersistPool = "pool_btnCreatePersistPool";
    private string pool_btnCreatePrefabPool  = "pool_btnCreatePrefabPool";

    private GameObject effectPrefab;

    public override void InitView()
    {
        btnReturn            = transform.Find("btnReturn").GetComponent<Button>();
        btnGetPool           = transform.Find("ivBg/btnGetPool").GetComponent<Button>();
        btnCreatePersistPool = transform.Find("ivBg/btnCreatePersistPool").GetComponent<Button>();
        btnCreatePrefabPool  = transform.Find("ivBg/btnCreatePrefabPool").GetComponent<Button>();

        effectPrefab = Resources.Load("Effect/Star") as GameObject;
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
        btnGetPool.onClick.AddListener(OnClickObtnGetPool);
        btnCreatePersistPool.onClick.AddListener(OnClickObtnCreatePersistPool);
        btnCreatePrefabPool.onClick.AddListener(OnClickObtnCreatePrefabPool);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
        btnGetPool.onClick.RemoveListener(OnClickObtnGetPool);
        btnCreatePersistPool.onClick.RemoveListener(OnClickObtnCreatePersistPool);
        btnCreatePrefabPool.onClick.RemoveListener(OnClickObtnCreatePrefabPool);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void OnClickObtnGetPool()
    {
        Framework.Pool.GetPool(pool_btnGetPool).Spawn(effectPrefab.transform);
    }

    private void OnClickObtnCreatePersistPool()
    {
        Framework.Pool.GetPool(pool_btnGetPool).Despawn(effectPrefab.transform, 3);
    }

    private void OnClickObtnCreatePrefabPool()
    {
    }
}