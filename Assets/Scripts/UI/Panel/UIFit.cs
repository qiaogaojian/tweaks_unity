using Mega;
using UnityEngine.UI;

public class UIFit : BaseView
{
    private Button btnReturn;

    public override void InitView()
    {
        btnReturn = transform.Find("btnReturn").GetComponent<Button>();
    }

    protected override void AddEvent()
    {
        btnReturn.onClick.AddListener(OnClickBtnReturn);
    }

    protected override void RemoveEvent()
    {
        btnReturn.onClick.RemoveListener(OnClickBtnReturn);
    }

    private void OnClickBtnReturn()
    {
        Framework.UI.HideCurrent();
    }
}