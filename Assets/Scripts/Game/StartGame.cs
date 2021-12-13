using Mega;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        Framework.Init(OnLoadFramework);
    }

    void OnLoadFramework(LoadEvent loadEvent)
    {
        Framework.Scene.Load(SceneType.Hall, () => { Framework.UI.Show(ViewID.UIMenu); });
    }
}