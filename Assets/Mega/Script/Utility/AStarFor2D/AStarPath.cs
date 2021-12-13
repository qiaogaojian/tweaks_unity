using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mega;

public class AStarPath
{
    private AStarUtils       m_AstarUtils;
    private AStarNode        m_BeginNode;
    private AStarNode        m_EndNode;
    private int              m_Width;
    private int              m_Height;
    private IList<AStarNode> m_PathList;

    /// <summary>
    /// 记录可通过状态
    /// </summary>
    static private List<bool> m_NodePassList = new List<bool>();

    private bool m_InitFinished = false;

    static private List<AStarUnit> m_ReadyUtilList = new List<AStarUnit>();

    /// <summary>
    /// Init 先初始化再使用寻路功能
    /// nodes true表示可通过  false表示障碍
    /// width 表示列数  height表示行数
    /// </summary>
    public void Init(List<bool> nodes, int width, int height)
    {
        int  diffCount = width * height - m_Width * m_Height;
        bool smallWH   = (width <= m_Width && height <= m_Height);
        if (!smallWH)
        {
            //更大的地图
            m_AstarUtils = new AStarUtils(width, height);
            for (int i = 0; i < diffCount; i++)
            {
                m_ReadyUtilList.Add(new AStarUnit());
            }

            m_NodePassList.Clear();

            //从节点列表中取节点 初始化寻路状态
            int index = 0;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    bool      pass      = nodes[row * width + col];
                    AStarUnit aStarUnit = m_ReadyUtilList[index];
                    aStarUnit.isPassable = pass;
                    m_AstarUtils.GetNode(col, row).AddUnit(aStarUnit);
                    //加入状态列表
                    m_NodePassList.Add(pass);

                    index++;
                }
            }

            //更新宽高
            m_Width  = width;
            m_Height = height;
        }
        else
        {
            UpdateNodeList(nodes, width, height);
        }

        //初始化完毕
        m_InitFinished = true;
    }

    /// <summary>
    /// 修改指定点是否可通行
    /// </summary>
    /// <param name="pass"> 是否可通行 </param>
    /// <param name="col"> 坐标x </param>
    /// <param name="row"> 坐标y </param>
    public void UpdateNode(bool pass, int col, int row)
    {
        if (m_InitFinished)
        {
            m_AstarUtils.GetNode(col, row).UpdatePassStatus(pass);
            //更新状态列表
            m_NodePassList[row * m_Width + col] = pass;
        }
    }

    /// <summary>
    /// 修改制定list 点是否可通行
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void UpdateNodeList(List<bool> nodes, int width, int height)
    {
        int curCount = width * height;
        if (m_InitFinished && curCount <= m_Width * m_Height)
        {
            Dictionary<string, AStarNode> nodeMap = m_AstarUtils.nodes;

            int index = 0;
            for (int row = 0; row < m_Height; row++)
            {
                int baseIndex = row * m_Width;
                for (int col = 0; col < m_Width; col++)
                {
                    int oldMapIndex = baseIndex + col;

                    if (row < height && col < width)
                    {
                        //地图区域内 更新通行状态
                        bool nodePass = nodes[index];
                        index++;
                        if (m_NodePassList[oldMapIndex] != nodePass)
                        {
                            m_NodePassList[oldMapIndex] = nodePass;
                            nodeMap[col + ":" + row].UpdatePassStatusEx(nodePass);
                        }
                    }
                    else if (m_NodePassList[oldMapIndex])
                    {
                        //地图区域外不可通行
                        m_NodePassList[oldMapIndex] = false;
                        nodeMap[col + ":" + row].UpdatePassStatusEx(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 寻路
    /// </summary>
    public IList<AStarNode> FindPath(int beginX, int beginY, int endX, int endY)
    {
        //Debuger.Log("beginX"+beginX+"beginY"+beginY+"endX"+endX+"endY"+endY,Debuger.ColorType.magenta);

        if (beginX < 0 || beginX >= m_Width || endX < 0 || endX >= m_Width ||
            beginY < 0 || beginY >= m_Height || endY < 0 || endY >= m_Height)
        {
            Debuger.Log("超出地图范围.");
            return null;
        }

        m_BeginNode = m_AstarUtils.GetNode(beginX, beginY);
        m_EndNode   = m_AstarUtils.GetNode(endX, endY);
        m_PathList  = m_AstarUtils.FindPath(m_BeginNode, m_EndNode);
        return m_PathList;
    }
}