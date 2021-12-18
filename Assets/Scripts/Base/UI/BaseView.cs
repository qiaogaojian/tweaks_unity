using System;
using UnityEngine;

namespace Mega
{
    public class BaseView : MonoBehaviour
    {
        protected BaseViewModel viewModel;

        protected virtual void Awake()
        {
            InitView();
        }

        private void OnEnable()
        {
            OnResume();
            AddEvent();
        }

        private void OnDisable()
        {
            RemoveEvent();
        }

        public virtual void InitView()
        {
        }

        public virtual void RefreshView()
        {
        }

        protected virtual void AddEvent()
        {
        }

        protected virtual void RemoveEvent()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnHide()
        {
        }

        private void OnDestroy()
        {
            viewModel?.Destroy();
        }
    }
}