using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Stationary,
    Patrol
}

public class EnemyMover : Mover
{
    public Vector3 directionToMove = new Vector3(0f, 0f, Board.spacing);
    public MovementType movementType = MovementType.Stationary;
    public float standTime = 1f;
    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void MoveOneTurn()
    {
        switch (movementType)
        {
            case MovementType.Patrol:
                Patrol();
                break;

            case MovementType.Stationary:
                Stand();
                break;
        }
    }

    void Stand()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine()
    {
        yield return new WaitForSeconds(standTime);
        base.finishMovementEvent.Invoke();
    }

    void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        Vector3 startPos = new Vector3(m_currentNode.Coordinate.x, 0f, m_currentNode.Coordinate.y);
        Vector3 newDestination = startPos + transform.TransformVector(directionToMove);
        Vector3 nextDestination = startPos + transform.TransformVector(directionToMove*2);
        Move(newDestination, 0f);
        while (isMoving)
        {
            yield return null;
        }
        base.finishMovementEvent.Invoke();
    }
}
