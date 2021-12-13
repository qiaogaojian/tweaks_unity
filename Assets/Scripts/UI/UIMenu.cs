using Mega;
using UnityEngine.UI;

/**
 * 功能菜单
 *
 * TestDebuger (测试 Debuger工具,打包成dll文件以方便定位Log位置,源文件在Asset同级C#目录下)
 * TestSound (测试音效播放管理工具)
 * TestSpine (测试Spine的所有功能)
 * TestFight (测试通用战斗框架)
 * TestSceneText (测试非UI场景中文字的实现)
 * TestTextEffect (测试文字打字机效果和富文本)
 *
 * Todo:
 * TestAssetManager (热更新和资源管理)
 * TestShader (图形渲染)
 * TestDataStructure (数据结构)
 * TestAlgorithm (算法)
 * TestOptimize (性能优化)
 * TestDesignPattern (设计模式)
 * TestUGUI (游戏界面)
 * TestEventManager (事件系统)
 * TestJsonNet
 * TestPathFinding
 * TestNGUI
 * TestUIToolkit
 * TestFairyGUI
 * TestHttp
 * TestSocket
 * TestDataManager
 * TestRuntimeUpdate
 * TestTilemap
 */
public class UIMenu : UIBase
{
    private Button btnTestDebuger;
    private Button btnTestSound;
    private Button btnTestSpine;
    private Button btnTestFight;
    private Button btnTestSceneText;
    private Button btnTestTextEffect;

    public override void InitView()
    {
        btnTestDebuger    = transform.Find("bg/svBtn/Viewport/Content/btnTestDebuger").GetComponent<Button>();
        btnTestSound      = transform.Find("bg/svBtn/Viewport/Content/btnTestSound").GetComponent<Button>();
        btnTestSpine      = transform.Find("bg/svBtn/Viewport/Content/btnTestSpine").GetComponent<Button>();
        btnTestFight      = transform.Find("bg/svBtn/Viewport/Content/btnTestFight").GetComponent<Button>();
        btnTestSceneText  = transform.Find("bg/svBtn/Viewport/Content/btnTestSceneText").GetComponent<Button>();
        btnTestTextEffect = transform.Find("bg/svBtn/Viewport/Content/btnTestTextEffect").GetComponent<Button>();
    }

    #region 事件

    protected override void AddEvent()
    {
        btnTestDebuger.onClick.AddListener(OnClickBtnTestDebuger);
        btnTestSound.onClick.AddListener(OnClickBtnTestSound);
        btnTestSpine.onClick.AddListener(OnClickBtnTestSpine);
        btnTestFight.onClick.AddListener(OnClickBtnTestFight);
        btnTestSceneText.onClick.AddListener(OnClickBtnTestSceneText);
        btnTestTextEffect.onClick.AddListener(OnClickBtnTestTextEffect);
    }

    protected override void RemoveEvent()
    {
        btnTestDebuger.onClick.RemoveListener(OnClickBtnTestDebuger);
        btnTestSound.onClick.RemoveListener(OnClickBtnTestSound);
        btnTestSpine.onClick.RemoveListener(OnClickBtnTestSpine);
        btnTestFight.onClick.RemoveListener(OnClickBtnTestFight);
        btnTestSceneText.onClick.RemoveListener(OnClickBtnTestSceneText);
        btnTestTextEffect.onClick.RemoveListener(OnClickBtnTestTextEffect);
    }

    private void OnClickBtnTestDebuger()
    {
        Framework.UI.Show(ViewID.UITestDebuger);
    }

    private void OnClickBtnTestSound()
    {
        Framework.UI.Show(ViewID.UITestSound);
        // Framework.Scene.Load(SceneType.Sound);
    }

    private void OnClickBtnTestSpine()
    {
        Framework.UI.Show(ViewID.UITestSpine);
    }

    private void OnClickBtnTestFight()
    {
        // Framework.Fight.Init();
        // Framework.UI.Show(ViewID.UITestFight);
    }

    private void OnClickBtnTestSceneText()
    {
        Framework.Scene.Load(SceneType.Game, () => { GamePlayManager.Instance.StartGame(GamePlayType.LootGrid); });
        Framework.UI.Hide(ViewID.UIMenu);
        Framework.UI.Show(ViewID.UIFight);
    }

    private void OnClickBtnTestTextEffect()
    {
        Framework.UI.Show(ViewID.UITestTextEffect);
    }

    #endregion
}