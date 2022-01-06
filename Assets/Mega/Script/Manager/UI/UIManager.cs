using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mega
{
    public class UIManager : GameComponent
    {
        private GameObject uiRoot;
        private GameObject uiCamera;
        private GameObject eventSystem;
        private GameObject normalRoot;
        private GameObject dialogRoot;
        private GameObject hintRoot;
        private GameObject toppestRoot;
        private GameObject effectRoot;
        private GameObject webglRoot;
        private GameObject webglLeftFB;
        private GameObject webglRightFB;
        private GameObject webglTopFB;
        private GameObject webglBottomFB;

        private Dictionary<ViewID, View> views     = new Dictionary<ViewID, View>();
        private UIStack                  viewStack = new UIStack();

        public static int DesignWidth  = 1920;
        public static int DesignHeight = 1080;

        void Start()
        {
            Init();
        }

        private void Init()
        {
            uiRoot      = new GameObject("UIRoot");
            uiCamera    = new GameObject("UICamera");
            eventSystem = new GameObject("EventSystem");
            normalRoot  = new GameObject("NormalRoot");
            dialogRoot  = new GameObject("PopupRoot");
            hintRoot    = new GameObject("HintRoot");
            toppestRoot = new GameObject("ToppestRoot");
            effectRoot  = new GameObject("EffectRoot");

#if UNITY_WEBGL
            webglRoot     = new GameObject("WebglRoot");
            webglLeftFB   = new GameObject("webglLeftFB");
            webglRightFB  = new GameObject("webglRightFB");
            webglTopFB    = new GameObject("webglTopFB");
            webglBottomFB = new GameObject("webglBottomFB");
#endif

            
            uiRoot.transform.SetParent(transform);
            uiCamera.transform.SetParent(uiRoot.transform);
            eventSystem.transform.SetParent(uiRoot.transform);
            normalRoot.transform.SetParent(uiRoot.transform);
            dialogRoot.transform.SetParent(uiRoot.transform);
            hintRoot.transform.SetParent(uiRoot.transform);
            toppestRoot.transform.SetParent(uiRoot.transform);
            effectRoot.transform.SetParent(uiRoot.transform);
#if UNITY_WEBGL
            webglRoot.transform.SetParent(uiRoot.transform);
            webglLeftFB.transform.SetParent(webglRoot.transform);
            webglRightFB.transform.SetParent(webglRoot.transform);
            webglTopFB.transform.SetParent(webglRoot.transform);
            webglBottomFB.transform.SetParent(webglRoot.transform);
#endif

            uiRoot.layer      = LayerMask.NameToLayer("UI");
            uiCamera.layer    = LayerMask.NameToLayer("UI");
            normalRoot.layer  = LayerMask.NameToLayer("UI");
            dialogRoot.layer  = LayerMask.NameToLayer("UI");
            hintRoot.layer    = LayerMask.NameToLayer("UI");
            toppestRoot.layer = LayerMask.NameToLayer("UI");
            effectRoot.layer  = LayerMask.NameToLayer("UI");

#if UNITY_WEBGL
            webglRoot.layer     = LayerMask.NameToLayer("UI");
            webglLeftFB.layer   = LayerMask.NameToLayer("UI");
            webglRightFB.layer  = LayerMask.NameToLayer("UI");
            webglTopFB.layer    = LayerMask.NameToLayer("UI");
            webglBottomFB.layer = LayerMask.NameToLayer("UI");
#endif

            //初始化画布组件
            Camera camera = uiCamera.AddComponent<Camera>();
            Canvas canvas = uiRoot.AddComponent<Canvas>();
            canvas.renderMode              = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera             = camera;
            canvas.pixelPerfect            = true;
            canvas.planeDistance           = 100;
            canvas.sortingOrder            = -1;
            uiRoot.transform.localPosition = Vector3.zero;
            CanvasScaler canvasScaler = uiRoot.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

#if UNITY_WEBGL

            canvasScaler.referenceResolution = new Vector2(DesignHeight, DesignWidth);
#else
  // UI适配
            if (Display.main.systemWidth > Display.main.systemHeight)
            {
                canvasScaler.referenceResolution = new Vector2(DesignWidth, DesignHeight);
            }
            else
            {
                canvasScaler.referenceResolution = new Vector2(DesignHeight, DesignWidth);
            }
#endif

            canvasScaler.screenMatchMode        = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.referencePixelsPerUnit = 100;
            uiRoot.AddComponent<GraphicRaycaster>();

            //初始化相机组件
            camera.clearFlags       = CameraClearFlags.Depth;
            camera.cullingMask      = 1 << 5; //表示UI
            camera.orthographic     = true;
            camera.orthographicSize = 5;
            camera.nearClipPlane    = -10;
            camera.farClipPlane     = 50;
            camera.depth            = 10;
            uiCamera.AddComponent<AudioListener>();
            
            //正常层级
            canvas = normalRoot.AddComponent<Canvas>(); 
            RectTransform rectTransform = canvas.transform as RectTransform;
#if UNITY_WEBGL

#else
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
#endif
         
            rectTransform.pivot     = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            canvas.overrideSorting  = true;
            canvas.pixelPerfect     = true;
            canvas.sortingOrder     = (int) ViewType.Normal;
#if UNITY_WEBGL
            rectTransform.sizeDelta = new Vector2( DesignHeight, DesignWidth);
#endif
            normalRoot.AddComponent<GraphicRaycaster>();

            //弹窗口层级
            canvas                  = dialogRoot.AddComponent<Canvas>();
            rectTransform           = canvas.transform as RectTransform;
#if UNITY_WEBGL

#else
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
#endif
            rectTransform.pivot     = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            canvas.overrideSorting  = true;
            canvas.pixelPerfect     = true;
            canvas.sortingOrder     = (int) ViewType.Dialog;
#if UNITY_WEBGL
            rectTransform.sizeDelta = new Vector2( DesignHeight, DesignWidth);
#endif
            dialogRoot.AddComponent<GraphicRaycaster>();

            //提示层级
            canvas                  = hintRoot.AddComponent<Canvas>();
            rectTransform           = canvas.transform as RectTransform;
#if UNITY_WEBGL

#else
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
#endif
            rectTransform.pivot     = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            canvas.overrideSorting  = true;
            canvas.pixelPerfect     = true;
            canvas.sortingOrder     = (int) ViewType.Hint;
#if UNITY_WEBGL
            rectTransform.sizeDelta = new Vector2( DesignHeight, DesignWidth);
#endif
            hintRoot.AddComponent<GraphicRaycaster>();

            //最高层级
            canvas                  = toppestRoot.AddComponent<Canvas>();
            rectTransform           = canvas.transform as RectTransform;
#if UNITY_WEBGL

#else
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
#endif
            rectTransform.pivot     = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            canvas.overrideSorting  = true;
            canvas.pixelPerfect     = true;
            canvas.sortingOrder     = (int) ViewType.Top;
#if UNITY_WEBGL
            rectTransform.sizeDelta = new Vector2( DesignHeight, DesignWidth);
#endif
            toppestRoot.AddComponent<GraphicRaycaster>();

            //2D特效层级
            canvas                  = effectRoot.AddComponent<Canvas>();
            rectTransform           = canvas.transform as RectTransform;
#if UNITY_WEBGL

#else
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
#endif
            rectTransform.pivot     = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            canvas.overrideSorting  = true;
            canvas.pixelPerfect     = true;
            canvas.sortingOrder     = (int) ViewType.Effect;
#if UNITY_WEBGL
            rectTransform.sizeDelta = new Vector2( DesignHeight, DesignWidth);
#endif
            effectRoot.AddComponent<GraphicRaycaster>();

#if UNITY_WEBGL


       
            
            // Webgl适配层
            canvas                  = webglRoot.AddComponent<Canvas>();
            rectTransform           = canvas.transform as RectTransform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot     = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            canvas.overrideSorting  = true;
            canvas.pixelPerfect     = true;
            canvas.sortingOrder     = (int) ViewType.Effect;

            // Left
            Image img = webglLeftFB.AddComponent<Image>();
            img.color               = Color.cyan;
            rectTransform           = img.transform as RectTransform;
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.pivot     = new Vector2(0, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;

            // Right
            img                     = webglRightFB.AddComponent<Image>();
            img.color               = Color.cyan;
            rectTransform           = img.transform as RectTransform;
            rectTransform.anchorMin = new Vector2(1, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
            rectTransform.pivot     = new Vector2(1, 0.5f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;

            // Top
            img                     = webglTopFB.AddComponent<Image>();
            img.color               = Color.cyan;
            rectTransform           = img.transform as RectTransform;
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot     = new Vector2(0.5f, 1f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;

            // Bottom
            img                     = webglBottomFB.AddComponent<Image>();
            img.color               = Color.cyan;
            rectTransform           = img.transform as RectTransform;
            rectTransform.anchorMin = new Vector2(0.5f, 0f);
            rectTransform.anchorMax = new Vector2(0.5f, 0f);
            rectTransform.pivot     = new Vector2(0.5f, 0f);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;

            webglRoot.AddComponent<GraphicRaycaster>();
            webglRoot.AddComponent<WebMargin>();
#endif

            // EventSystem
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        public View Show(ViewID id, ViewType type = ViewType.Normal)
        {
            viewStack.Push(id);
            if (isLoaded(id)) 
            {
                views[id].Show();
                return views[id];
            }

            return Load(id, type);
        }

        public void Hide(ViewID id)
        {
            if (isLoaded(id))
            {
                views[id].Hide();
                viewStack.Remove(id);
            }
        }

        public void HideCurrent()
        {
            if (isLoaded(viewStack.Peek()))
            {
                ViewID id = viewStack.Peek();

                views[id].Hide();
                viewStack.Pop();
            }
        }

        public void Destroy(ViewID id)
        {
            if (isLoaded(id))
            {
                views[id].Destroy();
                views.Remove(id);
                viewStack.Remove(id);
            }
        }

        public void DestroyCurrent()
        {
            if (isLoaded(viewStack.Peek()))
            {
                ViewID id = viewStack.Peek();

                views[id].Destroy();
                views.Remove(id);
                viewStack.Pop();
            }
        }

        public bool HaveComponent<T>(ViewID id) where T : Component
        {
            if (isLoaded(id))
            {
                return views[id].UiObject.GetComponent<T>() != null;
            }

            return false;
        }

        public T GetComponent<T>(ViewID id) where T : Component
        {
            if (isLoaded(id))
            {
                return views[id].UiObject.GetComponent<T>();
            }

            return null;
        }

        public bool isLoaded(ViewID id)
        {
            if (views.ContainsKey(id))
            {
                return true;
            }

            // Debuger.LogError(string.Format(" View {0} is not loaded.", id.ToString()));
            return false;
        }

        public bool isShowing(ViewID id)
        {
            if (isLoaded(id))
            {
                return views[id].UIBase.IsShow;
            }

            return false;
        }

        private View Load(ViewID id, ViewType type = ViewType.Normal)
        {
            string path = string.Format("Prefabs/UI/{0}", id.ToString());
            if (uiRoot == null || uiCamera == null)
            {
                Init();
            }

            View view = new View(id);
            view.UiObject = CreateUI(Framework.Resource.GetUIPrefab(path), getViewRoot(type));
            view.UiObject.AddComponent<CanvasGroup>(); // https://blog.csdn.net/LLLLL__/article/details/103385952
            view.ViewType = type;
            view.UIBase   = view.UiObject.GetComponent<UIBase>();
            view.Show();
            views.Add(id, view);
            
            return view;
        }

        private GameObject CreateUI(GameObject prefab, GameObject parent)
        {
            GameObject go = Instantiate(prefab, parent.transform, true) as GameObject;
            go.transform.localPosition                 = Vector3.zero;
            go.transform.localScale                    = Vector3.one;
            go.transform.rotation                      = Quaternion.identity;
            go.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            go.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            return go;
        }

        private GameObject getViewRoot(ViewType type)
        {
            switch (type)
            {
                case ViewType.Normal:
                    return normalRoot;
                case ViewType.Dialog:
                    return dialogRoot;
                case ViewType.Hint:
                    return hintRoot;
                case ViewType.Top:
                    return toppestRoot;
                case ViewType.Effect:
                    return effectRoot;
                default:
                    return normalRoot;
            }
        }
    }
}