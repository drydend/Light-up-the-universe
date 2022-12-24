﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class PlayerInput : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private bool _invertControl;

    private Player _player;
    private Camera _camera;
    private bool _isActive = true;

    public event Action OnPlayerTap;

    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    public void DisableInput()
    {
        _isActive = false;
    }

    public void EnableInput()
    {
        _isActive = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isActive)
        {
            return;
        }

        OnPlayerTap?.Invoke();

        var clickPosition = _camera.ScreenToWorldPoint(eventData.position);
        var jumpDirection = (Vector2)(_camera.transform.position - clickPosition);
        jumpDirection.Normalize();

        if (_invertControl)
        {
            var x = jumpDirection.x * -1;
            jumpDirection.x = jumpDirection.y;
            jumpDirection.y = x;
        }

        _player.Jump(jumpDirection);
    }


    private void Awake()
    {
        _camera = Camera.main;
    }
}
