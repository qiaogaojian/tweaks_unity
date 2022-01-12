using Mega;
using UnityEngine.UI;

public class UIDebuger : BaseView
{
    private Button btnReturn;
    private Button btnLog;
    private Button btnLogWarning;
    private Button btnLogError;

    public override void InitView()
    {
        btnReturn     = transform.Find("btnReturn").GetComponent<Button>();
        btnLog        = transform.Find("btnLog").GetComponent<Button>();
        btnLogWarning = transform.Find("btnLogWarning").GetComponent<Button>();
        btnLogError   = transform.Find("btnLogError").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
        btnLog.onClick.AddListener(OnClickBtnLog);
        btnLogWarning.onClick.AddListener(OnClickBtnLogWarning);
        btnLogError.onClick.AddListener(OnClickBtnLogError);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
        btnLog.onClick.AddListener(OnClickBtnLog);
        btnLogWarning.onClick.AddListener(OnClickBtnLogWarning);
        btnLogError.onClick.AddListener(OnClickBtnLogError);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }

    void OnClickBtnLog()
    {
        Debuger.Log("Log");
        Debuger.Log("white");
        Debuger.Log("yellow", Debuger.ColorType.yellow);
        Debuger.Log("red", Debuger.ColorType.red);
        Debuger.Log("green", Debuger.ColorType.green);
        Debuger.Log("cyan", Debuger.ColorType.cyan);
        Debuger.Log("magenta", Debuger.ColorType.magenta);
    }

    void OnClickBtnLogWarning()
    {
        Debuger.LogWarning("LogWarning");
        Debuger.LogWarning("white");
        Debuger.LogWarning("yellow", Debuger.ColorType.yellow);
        Debuger.LogWarning("red", Debuger.ColorType.red);
        Debuger.LogWarning("green", Debuger.ColorType.green);
        Debuger.LogWarning("cyan", Debuger.ColorType.cyan);
        Debuger.LogWarning("magenta", Debuger.ColorType.magenta);
    }

    void OnClickBtnLogError()
    {
        Debuger.LogError("LogError");
        Debuger.LogError("white");
        Debuger.LogError("yellow", Debuger.ColorType.yellow);
        Debuger.LogError("red", Debuger.ColorType.red);
        Debuger.LogError("green", Debuger.ColorType.green);
        Debuger.LogError("cyan", Debuger.ColorType.cyan);
        Debuger.LogError("magenta", Debuger.ColorType.magenta);
    }
}