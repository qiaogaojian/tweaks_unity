using UnityEngine;

namespace Mega
{
    public class View
    {
        private ViewID     viewId;
        private GameObject uiObject;
        private ViewType   viewType;
        private bool       isShow  = false;
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

        public bool IsShow
        {
            get => isShow;
            set
            {
                isShow = value;
                // uiObject.SetActive(isShow);
                if (isShow)
                {
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
                        uiObject.GetComponent<UIBase>().OnResume();
                    }
                    UiObject.transform.SetAsLastSibling();
                    hasInit = true;
                }
                else
                {
                    uiObject.GetComponent<CanvasGroup>().alpha          = 0;
                    uiObject.GetComponent<CanvasGroup>().interactable   = false;
                    uiObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    uiObject.GetComponent<UIBase>().OnHide();
                }
            }
        }

        public View(ViewID id)
        {
            viewId = id;
        }

        ~View()
        {
            Debuger.Log(string.Format("WindowId:{0}已销毁！", ViewId));
        }

        private void Show()
        {
            IsShow = true;
        }

        private void Hide()
        {
            IsShow = false;
        }

        public void Destroy()
        {
        }
    }
}