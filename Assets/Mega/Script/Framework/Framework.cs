using System;
using System.Collections;
using UnityEngine;

namespace Mega
{
    public class Framework : MonoBehaviour
    {
        public static UIManager UI { get; private set; }
        public static ResourceManager Resource { get; private set; }
        public static SoundManager Sound { get; private set; }
        public static EventManager Event { get; private set; }
        public static SceneLoadManager Scene { get; private set; }
        public static PoolManager Pool { get; private set; }
        public static DataManager Data { get; private set; }
        public static FightManager Fight { get; private set; }

        private static bool              hasInit = false;
        private static Action<LoadEvent> loadEvent;

        public static void Init(Action<LoadEvent> onLoad = null)
        {
            if (hasInit) // 防止多次生成
            {
                return;
            }

            hasInit = true;

            loadEvent = onLoad;
            GameObject prefab = Resources.Load<GameObject>("Framework");
            Instantiate(prefab);
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitFramework());
        }

        private IEnumerator InitFramework()
        {
            UI       = transform.Find("UI").GetComponent<UIManager>();
            Resource = transform.Find("Resource").GetComponent<ResourceManager>();
            Sound    = transform.Find("Sound").GetComponent<SoundManager>();
            Event    = transform.Find("Event").GetComponent<EventManager>();
            Scene    = transform.Find("Scene").GetComponent<SceneLoadManager>();
            Pool     = transform.Find("Pool").GetComponent<PoolManager>();
            Data     = transform.Find("DataTable").GetComponent<DataManager>();
            Fight    = transform.Find("Fight").GetComponent<FightManager>();

            yield return new WaitForEndOfFrame();
            if (loadEvent != null)
            {
                loadEvent.Invoke(LoadEvent.SUCCESS);
            }
        }
    }
}