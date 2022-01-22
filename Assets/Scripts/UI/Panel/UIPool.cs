using Mega;
using UnityEngine;
using UnityEngine.UI;

public class UIPool : BaseView
{
    private Button btnReturn;
    private Button btnSpawn;
    private Button btnDespawn;
    private Button btnDestroyPool;
    private Button btnSpawnPersist;
    private Button btnDespawnPersist;
    private Button btnDestroyPersistPool;
    private Button btnSpawnPrefab;
    private Button btnDeSpawnPrefab;
    private Button btnDestroyPrefabPool;

    private string pool_btnGetPool           = "btnGet";
    private string pool_btnCreatePersistPool = "btnCreatePersist";
    private string pool_btnCreatePrefabPool  = "btnCreatePrefab";

    private GameObject effectPrefab;

    public override void InitView()
    {
        btnReturn             = transform.Find("btnReturn").GetComponent<Button>();
        btnSpawn              = transform.Find("ivBg/btnSpawn").GetComponent<Button>();
        btnDespawn            = transform.Find("ivBg/btnDespawn").GetComponent<Button>();
        btnDestroyPool        = transform.Find("ivBg/btnDestroyPool").GetComponent<Button>();
        btnSpawnPersist       = transform.Find("ivBg/btnSpawnPersist").GetComponent<Button>();
        btnDespawnPersist     = transform.Find("ivBg/btnDespawnPersist").GetComponent<Button>();
        btnDestroyPersistPool = transform.Find("ivBg/btnDestroyPersistPool").GetComponent<Button>();
        btnSpawnPrefab        = transform.Find("ivBg/btnSpawnPrefab").GetComponent<Button>();
        btnDeSpawnPrefab      = transform.Find("ivBg/btnDeSpawnPrefab").GetComponent<Button>();
        btnDestroyPrefabPool  = transform.Find("ivBg/btnDestroyPrefabPool").GetComponent<Button>();

        effectPrefab = Resources.Load("Effect/Star") as GameObject;
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);

        btnSpawn.onClick.AddListener(OnClickbtnSpawn);
        btnDespawn.onClick.AddListener(OnClickbtnDespawn);
        btnDestroyPool.onClick.AddListener(OnClickbtnDestroyPool);
        btnSpawnPersist.onClick.AddListener(OnClickbtnSpawnPersist);
        btnDespawnPersist.onClick.AddListener(OnClickbtnDespawnPersist);
        btnDestroyPersistPool.onClick.AddListener(OnClickbtnDestroyPersistPool);
        btnSpawnPrefab.onClick.AddListener(OnClickbtnSpawnPrefab);
        btnDeSpawnPrefab.onClick.AddListener(OnClickbtnDeSpawnPrefab);
        btnDestroyPrefabPool.onClick.AddListener(OnClickbtnDestroyPrefabPool);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);

        btnSpawn.onClick.RemoveListener(OnClickbtnSpawn);
        btnDespawn.onClick.RemoveListener(OnClickbtnDespawn);
        btnDestroyPool.onClick.RemoveListener(OnClickbtnDestroyPool);
        btnSpawnPersist.onClick.RemoveListener(OnClickbtnSpawnPersist);
        btnDespawnPersist.onClick.RemoveListener(OnClickbtnDespawnPersist);
        btnDestroyPersistPool.onClick.RemoveListener(OnClickbtnDestroyPersistPool);
        btnSpawnPrefab.onClick.RemoveListener(OnClickbtnSpawnPrefab);
        btnDeSpawnPrefab.onClick.RemoveListener(OnClickbtnDeSpawnPrefab);
        btnDestroyPrefabPool.onClick.RemoveListener(OnClickbtnDestroyPrefabPool);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    private void OnClickbtnSpawn()
    {
        SpawnPool pool      = Framework.Pool.GetPool(pool_btnGetPool);
        Transform curEffect = pool.Spawn(effectPrefab.transform, transform.position, transform.rotation, Framework.UI.GetEffectRoot());
        Framework.Pool.GetPool(pool_btnGetPool).Despawn(curEffect, 1);
    }

    private void OnClickbtnDespawn()
    {
    }

    private void OnClickbtnDestroyPool()
    {
    }

    private void OnClickbtnSpawnPersist()
    {
        SpawnPool pool      = Framework.Pool.GetPool(pool_btnCreatePersistPool);
        Transform curEffect = pool.Spawn(effectPrefab.transform, transform.position, transform.rotation, Framework.UI.GetEffectRoot());
        Framework.Pool.GetPool(pool_btnGetPool).Despawn(curEffect, 1);
    }

    private void OnClickbtnDespawnPersist()
    {
    }

    private void OnClickbtnDestroyPersistPool()
    {
    }

    private void OnClickbtnSpawnPrefab()
    {
        Framework.Pool.CreatePrefabPool(pool_btnCreatePersistPool, effectPrefab.transform);
        Transform curEffect = Framework.Pool.GetPool(pool_btnCreatePersistPool).Spawn(effectPrefab.transform, transform.position, transform.rotation, Framework.UI.GetEffectRoot());
        Framework.Pool.GetPool(pool_btnGetPool).Despawn(curEffect, 1);
    }

    private void OnClickbtnDeSpawnPrefab()
    {
    }

    private void OnClickbtnDestroyPrefabPool()
    {
    }
}