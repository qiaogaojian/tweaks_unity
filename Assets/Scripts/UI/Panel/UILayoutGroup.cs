using Mega;
using UnityEngine.UI;

public class UILayoutGroup : BaseView
{
    private Button btnReturn;

    public override void InitView()
    {
        btnReturn = transform.Find("btnTransform").GetComponent<Button>();
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
        Framework.UI.Hide(ViewID.UILayoutGroup);
    }
}