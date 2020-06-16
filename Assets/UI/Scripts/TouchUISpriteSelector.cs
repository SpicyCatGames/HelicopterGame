using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchUISpriteSelector : MonoBehaviour
{
    //this script will select the sprite for touch joystick at any given time
    [SerializeField] private Sprite Idle = default;
    [SerializeField] private Sprite Up = default;
    [SerializeField] private Sprite Down = default;
    [SerializeField] private Sprite Left = default;
    [SerializeField] private Sprite Right = default;
    [SerializeField] private Sprite UpLeft = default;
    [SerializeField] private Sprite UpRight = default;
    [SerializeField] private Sprite DownLeft = default;
    [SerializeField] private Sprite DownRight = default;

    private Vector2Int _oldInputValues = default;
    private Sprite _currentSprite;
    private SpriteRenderer _renderer = default;

    [SerializeField]private TouchInput _touchInput =  null;

    void Start()
	{
        _currentSprite = Idle;
        _renderer = GetComponent<SpriteRenderer>();
	}

    void Update()
    {
        Vector2Int inputValues = _touchInput.ButtonInputValues;
        //if input has changed, only then do we look to change the sprite.
        if (inputValues != _oldInputValues)
        {
            //checking for all 9 possible combinations 
            if (inputValues.x == -1)
            {
                if (inputValues.y == -1)
                {
                    _currentSprite = DownLeft;
                }
                else if (inputValues.y == 0)
                {
                    _currentSprite = Left;
                }
                else //else if (_touchInput.ButtonInputValues.y == 1)
                {
                    _currentSprite = UpLeft;
                }
            }
            else if (inputValues.x == 0)
            {
                if (_touchInput.ButtonInputValues.y == -1)
                {
                    _currentSprite = Down;
                }
                else if (inputValues.y == 0)
                {
                    _currentSprite = Idle;
                }
                else //else if (_touchInput.ButtonInputValues.y == 1)
                {
                    _currentSprite = Up;
                }
            }
            else //else if (_touchInput.ButtonInputValues.x == 1)
            {
                if (inputValues.y == -1)
                {
                    _currentSprite = DownRight;
                }
                else if (inputValues.y == 0)
                {
                    _currentSprite = Right;
                }
                else //else if (_touchInput.ButtonInputValues.y == 1)
                {
                    _currentSprite = UpRight;
                }
            }

            ApplySpritetoUI();
        }
        _oldInputValues = inputValues;
    }

    private void ApplySpritetoUI()
    {
        //this method will apply the new sprite to the UI
        _renderer.sprite = _currentSprite;
    }
}