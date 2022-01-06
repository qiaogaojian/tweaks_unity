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


            Framework.Resource.UnLoadRes(CurScene);
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