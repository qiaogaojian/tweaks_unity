using Mega;
using UnityEngine;

namespace Mega
{
    public class GameComponent : MonoBehaviour
    {
        protected virtual void Awake()
        {
            ComponentManager.RegisterComponent(this);
        }
    }
}