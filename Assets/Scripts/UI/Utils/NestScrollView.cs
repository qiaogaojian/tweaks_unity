using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestScrollView : ScrollRect
{
    private NestScrollView m_Parent;
    private bool           firstReachEnd = true;

    public enum Direction
    {
        Horizontal,
        Vertical
    }

    private Direction m_Direction          = Direction.Horizontal;
    private Direction m_BeginDragDirection = Direction.Horizontal;

    protected override void Awake()
    {
        base.Awake();
        Transform parent = transform.parent;
        if (parent)
        {
            m_Parent = parent.GetComponentInParent<NestScrollView>(); // 从下往上所有的父级中找到一个即返回
        }

        m_Direction = this.horizontal ? Direction.Horizontal : Direction.Vertical;
        if (m_Parent)
        {
            m_Parent.m_Direction = this.horizontal ? Direction.Horizontal : Direction.Vertical;
        }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            m_BeginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
            if (m_BeginDragDirection != m_Direction)
            {
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                return;
            }

            if (this.verticalNormalizedPosition <= 0.05f && eventData.delta.y > 0 ||
                this.verticalNormalizedPosition >= 0.95f && eventData.delta.y < 0)
            {
                if (m_Parent.m_Direction == m_Direction) // 子ScrollView和父ScrollView同一个方向时
                {
                    ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                    return;
                }
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
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.dragHandler);
                return;
            }

            if (this.verticalNormalizedPosition < 0.05f && eventData.delta.y > 0 ||
                this.verticalNormalizedPosition > 0.95f && eventData.delta.y < 0)
            {
                if (m_Parent.m_Direction == m_Direction)
                {
                    if (firstReachEnd)
                    {
                        ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
                        firstReachEnd = false;
                    }

                    ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.dragHandler);
                    return;
                }
            }

            firstReachEnd = true;
        }

        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_Parent)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.endDragHandler);
                return;
            }

            if (this.verticalNormalizedPosition <= 0.05f && eventData.delta.y > 0 ||
                this.verticalNormalizedPosition >= 0.95f && eventData.delta.y < 0)
            {
                if (m_Parent.m_Direction == m_Direction)
                {
                    ExecuteEvents.Execute(m_Parent.gameObject, eventData, ExecuteEvents.endDragHandler);
                    return;
                }
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
                ExecuteEvents.Execute(m_Parent.gameObject, data, ExecuteEvents.scrollHandler);
                return;
            }
        }

        base.OnScroll(data);
    }
}