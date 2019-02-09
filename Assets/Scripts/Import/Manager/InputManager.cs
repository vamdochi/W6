using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingleTon<InputManager> {

    public enum InputDirection
    {
        ALL = -1,
        LEFT,
        RIGHT,
        DOWN,
        TOP,
        MAX
    }
    
    public enum InputAction
    {
        PLAYER_MOVE = 0,
        PLAYER_ROLL,
        PLAYER_ATTACK,
        PLAYER_BLOCK,
        MAX
    }

    public static InputDirection VecToDirForInput(Vector3 vDir)
    {
        if (vDir.y == 0)
        {
            if( vDir.x < 0)
            {
                return InputDirection.LEFT;
            }
            return InputDirection.RIGHT;
        }
        else if (vDir.y > 0)
        {
            return InputDirection.TOP;
        }

        return InputDirection.DOWN;
    }


    private EnumArray<EnumArray<KeyCode>> _keyInput;

    public bool IsPressKeyOnce( InputAction act, InputDirection dir)
    {
        return Input.GetKeyDown(_keyInput.Get(act).Get(dir));
    }

    public bool IsPressKeyContinuous( InputAction act, InputDirection dir)
    {
        return Input.GetKey(_keyInput.Get(act).Get(dir));
    }

    void Awake () {
        _keyInput = new EnumArray<EnumArray<KeyCode>>();
        _keyInput.Allocate(InputAction.MAX);

        for( int i = 0; i < (int)InputAction.MAX; ++i)
        {
            _keyInput[i] = new EnumArray<KeyCode>();
            _keyInput[i].Allocate(InputDirection.MAX);
        }
        
        RegisterInputKey(InputAction.PLAYER_ATTACK, InputDirection.ALL, KeyCode.Mouse0);

        RegisterInputKey(InputAction.PLAYER_BLOCK,  InputDirection.ALL, KeyCode.Space);

    }
	
    public void RegisterInputKey( InputAction key, InputDirection dir, KeyCode keyCode)
    {
        if( dir == InputDirection.ALL)
        {
            _keyInput.Get(key).Set(InputDirection.TOP, keyCode);
            _keyInput.Get(key).Set(InputDirection.DOWN, keyCode);
            _keyInput.Get(key).Set(InputDirection.LEFT, keyCode);
            _keyInput.Get(key).Set(InputDirection.RIGHT, keyCode);
        }
        else
        {
            _keyInput.Get(key).Set(dir, keyCode);
        }
    }
}
