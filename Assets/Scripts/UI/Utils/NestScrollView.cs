using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestScrollView : ScrollRect
{
    //Parent CustomScrollRect object
    private NestScrollView m_Parent;

    public enum Direction
    {
        Horizontal,
        Vertical
    }

    //Sliding direction
    private Direction m_Direction = Direction.Horizontal;

    //Current operation direction
    private Direction m_BeginDragDirection = Direction.Horizontal;

    protected override void Awake()
    {
        base.Awake();
        //Parent object found
        Transform parent = transform.parent;
        if (parent)
        {
            m_Parent = parent.GetComponentInParent<NestScrollView>();
        }

        m_Direction = this.horizontal ? Direction.Horizontal : Direction.Vertical;
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            m_BeginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
            if (m_BeginDragDirection != m_Direction)
            {
                //The current operation direction is not equal to the sliding direction. Pass the event to the parent object
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                return;
            }
        }

        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //The current operation direction is not equal to the sliding direction. Pass the event to the parent object
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.dragHandler);
                return;
            }
        }

        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //The current operation direction is not equal to the sliding direction. Pass the event to the parent object
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.endDragHandler);
                return;
            }
        }

        base.OnEndDrag(eventData);
    }

    public override void OnScroll(PointerEventData data)
    {
        if (m_Parent)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //The current operation direction is not equal to the sliding direction. Pass the event to the parent object
                ExecuteEvents.Execute(m_Parent.gameObject, data, ExecuteEvents.scrollHandler);
                return;
            }
        }

        base.OnScroll(data);
    }
}