using Mega;
using UnityEngine.UI;

public class TestDebuger : BaseView
{
    private Button btnTestLog;
    private Button btnTestLogWarning;
    private Button btnTestLogError;
    private Button btnReturn;

    public override void InitView()
    {
        btnTestLog        = transform.Find("btnTestLog").GetComponent<Button>();
        btnTestLogWarning = transform.Find("btnTestLogWarning").GetComponent<Button>();
        btnTestLogError   = transform.Find("btnTestLogError").GetComponent<Button>();
        btnReturn         = transform.Find("btnReturn").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnTestLog.onClick.AddListener(OnClickTestLog);
        btnTestLogWarning.onClick.AddListener(OnClickTestLogWarning);
        btnTestLogError.onClick.AddListener(OnClickTestLogError);
        btnReturn.onClick.AddListener(OnClickBtnReturn);
    }

    protected override void RemoveEvent()
    {
        btnTestLog.onClick.RemoveListener(OnClickTestLog);
        btnTestLogWarning.onClick.RemoveListener(OnClickTestLogWarning);
        btnTestLogError.onClick.RemoveListener(OnClickTestLogError);
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
    }

    private void OnClickTestLog()
    {
        Debuger.Log("Log");
        Debuger.Log("white", Debuger.ColorType.white);
        Debuger.Log("yellow", Debuger.ColorType.yellow);
        Debuger.Log("red", Debuger.ColorType.red);
        Debuger.Log("green", Debuger.ColorType.green);
        Debuger.Log("cyan", Debuger.ColorType.cyan);
        Debuger.Log("magenta", Debuger.ColorType.magenta);
    }

    private void OnClickTestLogWarning()
    {
        Debuger.LogWarning("LogWarning");
        Debuger.LogWarning("white", Debuger.ColorType.white);
        Debuger.LogWarning("yellow", Debuger.ColorType.yellow);
        Debuger.LogWarning("red", Debuger.ColorType.red);
        Debuger.LogWarning("green", Debuger.ColorType.green);
        Debuger.LogWarning("cyan", Debuger.ColorType.cyan);
        Debuger.LogWarning("magenta", Debuger.ColorType.magenta);
    }

    private void OnClickTestLogError()
    {
        Debuger.LogError("LogError");
        Debuger.LogError("white", Debuger.ColorType.white);
        Debuger.LogError("yellow", Debuger.ColorType.yellow);
        Debuger.LogError("red", Debuger.ColorType.red);
        Debuger.LogError("green", Debuger.ColorType.green);
        Debuger.LogError("cyan", Debuger.ColorType.cyan);
        Debuger.LogError("magenta", Debuger.ColorType.magenta);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.Hide(ViewID.UITestDebuger);
    }
}