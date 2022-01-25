using System.Collections.Generic;
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

    private List<Transform> pool1Item = new List<Transform>();
    private List<Transform> pool2Item = new List<Transform>();
    private List<Transform> pool3Item = new List<Transform>();

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

    #region 一般对象池 (spawn时不设置父级, 默认会生成在场景中, 跳转场景时会自动销毁)

    private void OnClickbtnSpawn()
    {
        SpawnPool pool = Framework.Pool.GetPool(pool_btnGetPool);
        // 生成在UI节点下时, 也会持久存在
        Transform curEffect = pool.Spawn(effectPrefab.transform, transform.position, transform.rotation, Framework.UI.GetEffectRoot());

        pool1Item.Add(curEffect);
    }

    private void OnClickbtnDespawn()
    {
        if (pool1Item.Count <= 0)
        {
            return;
        }

        SpawnPool pool = Framework.Pool.GetPool(pool_btnGetPool);
        pool.Despawn(pool1Item[Random.Range(0, pool1Item.Count)], 1); // 延迟1秒后回收
    }

    private void OnClickbtnDestroyPool()
    {
        SpawnPool pool = Framework.Pool.GetPool(pool_btnGetPool);
        Framework.Pool.DestroyPool(pool);

        pool1Item.Clear();
    }

    #endregion


    #region 持久对象池 (对象默认生成在Framework/Pool下面,会持续存在)

    private void OnClickbtnSpawnPersist()
    {
        SpawnPool pool      = Framework.Pool.CreatePersistPool(pool_btnCreatePersistPool);
        Transform curEffect = pool.Spawn(effectPrefab.transform, transform.position, transform.rotation, Framework.UI.GetEffectRoot());

        pool2Item.Add(curEffect);
    }

    private void OnClickbtnDespawnPersist()
    {
        if (pool2Item.Count <= 0)
        {
            return;
        }

        SpawnPool pool = Framework.Pool.GetPool(pool_btnCreatePersistPool);
        pool.Despawn(pool2Item[Random.Range(0, pool2Item.Count)], 1); // 延迟1秒后回收
    }

    private void OnClickbtnDestroyPersistPool()
    {
        SpawnPool pool = Framework.Pool.GetPool(pool_btnCreatePersistPool);
        Framework.Pool.DestroyPool(pool);

        pool2Item.Clear();
    }

    #endregion


    #region 预制体对象池 首选 (对象默认生成在Framework/Pool下面,会持续存在,而且会自动回收)

    private void OnClickbtnSpawnPrefab()
    {
        SpawnPool pool      = Framework.Pool.CreatePrefabPool(pool_btnCreatePrefabPool, effectPrefab.transform);
        Transform curEffect = pool.Spawn(effectPrefab.transform, transform.position, transform.rotation, Framework.UI.GetEffectRoot());

        pool3Item.Add(curEffect);
    }

    private void OnClickbtnDeSpawnPrefab()
    {
        if (pool3Item.Count <= 0)
        {
            return;
        }

        SpawnPool pool        = Framework.Pool.GetPool(pool_btnCreatePrefabPool);
        int       removeIndex = Random.Range(0, pool3Item.Count);
        pool.Despawn(pool3Item[removeIndex]); // 立即回收
        pool3Item.RemoveAt(removeIndex);
    }

    private void OnClickbtnDestroyPrefabPool()
    {
        SpawnPool pool = Framework.Pool.GetPool(pool_btnCreatePrefabPool);
        Framework.Pool.DestroyPool(pool);

        pool3Item.Clear();
    }

    #endregion
}