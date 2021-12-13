using UnityEngine;

public class UIBase : MonoBehaviour
{
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
}