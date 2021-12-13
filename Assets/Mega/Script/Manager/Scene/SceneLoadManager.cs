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

        IEnumerator ToScene(SceneType type)
        {
            yield return new WaitForEndOfFrame();
            AsyncOperation op = JumpToScene(type);
            op.allowSceneActivation = false;
            yield return new WaitForEndOfFrame();
            op.allowSceneActivation = true;

            Framework.Resource.UnLoadAll();
            if (null != mFinishJump)
            {
                mFinishJump();
                mFinishJump = null;
            }
        }

        private AsyncOperation JumpToScene(SceneType type)
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