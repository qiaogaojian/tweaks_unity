using UnityEngine;

namespace Mega
{
    public class View
    {
        private ViewID     viewId;
        private GameObject uiObject;
        private ViewType   viewType;
        private BaseView   baseView;
        private bool       hasInit = false;

        public ViewID ViewId
        {
            get => viewId;
        }

        public GameObject UiObject
        {
            get => uiObject;
            set => uiObject = value;
        }


        public ViewType ViewType
        {
            get => viewType;
            set => viewType = value;
        }

        public BaseView BaseView
        {
            get => baseView;
            set => baseView = value;
        }


        public View(ViewID id)
        {
            viewId = id;
        }

        ~View()
        {
            Debuger.Log(string.Format("WindowId:{0}已销毁！", ViewId));
        }

        public void Show()
        {
            baseView.IsShow = true;

            uiObject.GetComponent<CanvasGroup>().alpha        = 1;
            uiObject.GetComponent<CanvasGroup>().interactable = true;
            if (viewType == ViewType.Normal)
            {
                uiObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                uiObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }

            if (hasInit)
            {
                baseView.OnResume();
            }

            UiObject.transform.SetAsLastSibling();
            uiObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
            hasInit                                              = true;
        }

        public void Hide()
        {
            baseView.Hide();
        }

        public void Destroy()
        {
            baseView.Destroy();
        }
    }
}