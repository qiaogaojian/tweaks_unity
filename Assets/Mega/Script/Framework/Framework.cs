﻿using System;
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
        }

        private void Start()
        {
            StartCoroutine(InitFramework());
        }

        private IEnumerator InitFramework()
        {
            UI       = ComponentManager.GetComponent<UIManager>();
            Resource = ComponentManager.GetComponent<ResourceManager>();
            Sound    = ComponentManager.GetComponent<SoundManager>();
            Event    = ComponentManager.GetComponent<EventManager>();
            Scene    = ComponentManager.GetComponent<SceneLoadManager>();

            yield return new WaitForEndOfFrame();
            if (loadEvent != null)
            {
                loadEvent.Invoke(LoadEvent.SUCCESS);
            }
        }
    }
}