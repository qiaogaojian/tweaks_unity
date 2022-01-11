using System;
using Mega;
using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    private bool isShow = false;

    public bool IsShow
    {
        get => isShow;
        set { isShow = value; }
    }

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

    public abstract void InitView();

    protected abstract void AddEvent();

    protected abstract void RemoveEvent();

    public virtual void OnResume()
    {
    }

    public virtual void OnHide()
    {
    }

    public virtual void Hide()
    {
        HideImp();
    }

    public virtual void Destroy()
    {
        DestroyImp();
    }

    public void HideImp()
    {
        this.IsShow                                     = false;
        this.GetComponent<CanvasGroup>().alpha          = 0;
        this.GetComponent<CanvasGroup>().interactable   = false;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        OnHide();
    }

    public void DestroyImp()
    {
        this.IsShow = false;
        GameObject.Destroy(gameObject);
        OnHide();
    }
}