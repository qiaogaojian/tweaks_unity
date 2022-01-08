using System;

namespace Mega
{
    public abstract class BaseViewModel
    {
        public abstract void Init(Action onFinish = null);
        public abstract void Destroy();
    }
}