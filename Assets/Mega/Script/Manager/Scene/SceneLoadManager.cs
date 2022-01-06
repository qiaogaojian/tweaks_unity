using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mega
{
    public class SceneLoadManager : GameComponent
    {
        private SceneType _SceneType  = SceneType.Start;
        private Action    mFinishJump = null;
        public SceneType CurScene => _SceneType;


        public void Load(SceneType type, Action onLoad = null)
        {
            // switch (type)
            // {
            //     case SceneType.Game:
            //         UIManager.Instance.Show(WindowId.UILoading).GetScript<UILoading>().Init(100, LoadingBg.Dungeon, 30);
            //         break;
            //     default:
            //         UIManager.Instance.Show(WindowId.UILoading).GetScript<UILoading>().Init(100, LoadingBg.Default, 30);
            //         break;
            // }

            if (_SceneType == type)
            {
                return;
            }

            mFinishJump = onLoad;
            StartCoroutine(ToScene(type));
        }


        public void StartLoading(SceneType type, float minDuration = 3f, Action onLoad = null)
        {
            if (_SceneType == type)
            {
                return;
            }

            StartCoroutine(ProcessScene(type, minDuration, onLoad));
        }

        private IEnumerator ProcessScene(SceneType type, float minDuration, Action onLoad = null)
        {
            var         go      = Framework.UI.Show(ViewID.UILoading, ViewType.Top).UiObject;
            LoadingView loading = go.GetComponent<LoadingView>();

            yield return new WaitForEndOfFrame();

            var op = Framework.Scene.JumpToScene(SceneType.Fight);

            //allowSceneActivation 为 false op.progress 最大值为0.89999999
            op.allowSceneActivation = false;

            var minTimer = minDuration;

            while (!op.isDone)
            {
                yield return new WaitForEndOfFrame();

                minTimer -= Time.deltaTime;

                if (minTimer <= 0
                    && op.progress >= 0.899f
                    && op.allowSceneActivation != true)
                {
                    loading.Progress        = 0.9f;
                    op.allowSceneActivation = true;
                }
                else if (op.progress < 0.9
                         && loading.Progress < op.progress)
                {
                    loading.Progress += 0.01f;
                }
                else
                {
                    loading.Progress = op.progress;
                }
            }

            onLoad?.Invoke();

            yield return new WaitForSeconds(0.2f);

            Framework.UI.Destroy(ViewID.UILoading);

            yield return null;
        }

        private IEnumerator ToScene(SceneType type)
        {
            yield return new WaitForEndOfFrame();
            AsyncOperation op = JumpToScene(type);
            // op.allowSceneActivation = false;
            // yield return new WaitForEndOfFrame();
            op.allowSceneActivation = true;


            Framework.Resource.UnLoadRes(CurScene);
            if (null != mFinishJump)
            {
                mFinishJump();
                mFinishJump = null;
            }

            yield return null;
        }


        public AsyncOperation JumpToScene(SceneType type)
        {
            try
            {
                _SceneType = type;
                return SceneManager.LoadSceneAsync(type.ToString());
            }
            catch (Exception e)
            {
                Debuger.LogError(string.Format("没有该场景:{0} Error:{1}", type, e.ToString()));
                return null;
            }
        }
    }
}