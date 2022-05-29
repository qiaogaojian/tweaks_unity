using Mega;
using UnityEngine;

namespace Game
{
    public class Start : MonoBehaviour
    {
        private void Awake()
        {
            Framework.Init(OnLoadFramework);
        }

        void OnLoadFramework(LoadEvent loadEvent)
        {
            Framework.Scene.Load(SceneType.Hall, () =>
            {
                Framework.UI.Show<UIHall>();
            });
        }
    }
}