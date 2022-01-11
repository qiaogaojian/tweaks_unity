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
            // Returns the component of Type type in the GameObject or any of its parents
            m_Parent = parent.GetComponentInParent<NestScrollView>();
        }

        m_Direction = this.horizontal ? Direction.Horizontal : Direction.Vertical;
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            m_BeginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
            if (m_BeginDragDirection != m_Direction ||
                this.verticalNormalizedPosition <= 0.05f && eventData.delta.y > 0 ||
                this.verticalNormalizedPosition >= 0.95f && eventData.delta.y < 0)
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

            if (this.verticalNormalizedPosition < 0.05f && eventData.delta.y > 0 ||
                this.verticalNormalizedPosition > 0.95f && eventData.delta.y < 0)
            {
                eventData.pressPosition = eventData.position;

                //The current operation direction is not equal to the sliding direction. Pass the event to the parent object
                Debuger.Log($"eventData {eventData.delta}");
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
            if (m_BeginDragDirection != m_Direction ||
                this.verticalNormalizedPosition <= 0.05f && eventData.delta.y > 0 ||
                this.verticalNormalizedPosition >= 0.95f && eventData.delta.y < 0)
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