using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static float spacing = 2f;
    public static readonly Vector2[] directions =
        {
        new Vector2(spacing , 0f),
        new Vector2(-spacing , 0f),
        new Vector2(0f,spacing),
        new Vector2(0f,-spacing)
    };

    List<Node> m_allNodes = new List<Node>();
    public List<Node> AllNodes
    {
        get {
            return m_allNodes;
        }
    }

    Node m_playerNode;
    public Node PlayerNode
    {
        get
        {
            return m_playerNode;
        }
    }

    Node m_goalNode;
    public Node GoalNode
    {
        get
        {
            return m_goalNode;
        }
    }

    public GameObject goalPrefab;
    public float drawGoalTime = 2f;
    public float drawGoalDelay = 2f;
    public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeInOutExpo;

    PlayerMover m_playerMover;

    void Awake()
    {
        m_playerMover = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        GetNodeList();
        m_goalNode = FindGoalNode();
    }

    public void GetNodeList()
    {
        Node[] nList = GameObject.FindObjectsOfType<Node>();
        m_allNodes = new List<Node>(nList);
    }

    public Node FindNodeAt(Vector3 pos)
    {
        Vector2 boardCord = Utility.Vector2Round(new Vector2(pos.x, pos.z));
        return m_allNodes.Find(n => n.Coordinate == boardCord);
    }

    public Node FindPlayerNode()
    {
        if (m_playerMover != null && !m_playerMover.isMoving)
        {
            return FindNodeAt(m_playerMover.transform.position);
        }
        return null;
    }

    public void UpdatePlayerNode()
    {
        m_playerNode = FindPlayerNode();
    }

    public Node FindGoalNode()
    {
        return m_allNodes.Find(n => n.isLevelGoal);
    }

    public void DrawGoal()
    {
        if (goalPrefab != null && m_goalNode != null)
        {
            GameObject goalInstance = Instantiate(goalPrefab, m_goalNode.transform.position, Quaternion.identity);
            iTween.ScaleFrom(goalInstance, iTween.Hash(
                "scale", Vector3.zero,
                "time", drawGoalTime,
                "delay", drawGoalDelay,
                "easetype", drawGoalEaseType));
        }
    }

    public void InitBoard()
    {
        if (m_playerNode != null)
        {
            m_playerNode.InitNode();
        }
    }
}
