﻿using System.Collections;
using UnityEngine;
using Zenject;

public class MovingObstacle : MonoBehaviour
{   
    [SerializeField]
    private Transform _startPosition;
    [SerializeField]
    private Transform _endPosition;
    [SerializeField]
    private AnimationCurve _speedCurve;
    [SerializeField]
    private float _moveTime;

    private Transform _currentEndPosition;
    private Transform _currentStartPosition;

    [Inject]
    public void Construct(ILevelStartTrigger levelStartTrigger)
    {
        levelStartTrigger.OnLevelStart += StartMoving;
    }

    private void StartMoving()
    {
        ResetPositions();
        StartCoroutine(MovingRoutine());
    }

    private void ResetPositions()
    {
        _currentStartPosition = _startPosition;
        _currentEndPosition = _endPosition;
    }

    private IEnumerator MovingRoutine()
    {
        while (true)
        {
            yield return MoveToDesiredPositionRoutine();
            SwapPositions();
        }
    }

    private IEnumerator MoveToDesiredPositionRoutine()
    {
        float timeElapsed = 0;

        while(timeElapsed <= 1)
        {
            var value = _speedCurve.Evaluate(timeElapsed);
            var newPosition = Vector2.Lerp(_currentStartPosition.position, _currentEndPosition.position, value);
            transform.position = newPosition;

            timeElapsed += Time.deltaTime / _moveTime;
            yield return null;
        }
    }

    private void SwapPositions()
    {
        var temp = _currentStartPosition;
        _currentStartPosition = _currentEndPosition;
        _currentEndPosition = temp;
    }
}