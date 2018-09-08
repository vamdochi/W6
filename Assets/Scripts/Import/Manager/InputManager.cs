using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingleTon<InputManager> {

    public enum InputDirection
    {
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

    void Start () {
        _keyInput = new EnumArray<EnumArray<KeyCode>>();
        _keyInput.Allocate(InputAction.MAX);

        for( int i = 0; i < (int)InputAction.MAX; ++i)
        {
            _keyInput[i] = new EnumArray<KeyCode>();
            _keyInput[i].Allocate(InputDirection.MAX);
        }

        RegisterInputKey(InputAction.PLAYER_ATTACK, InputDirection.TOP, KeyCode.W);
        RegisterInputKey(InputAction.PLAYER_ATTACK, InputDirection.LEFT, KeyCode.A);
        RegisterInputKey(InputAction.PLAYER_ATTACK, InputDirection.DOWN, KeyCode.S);
        RegisterInputKey(InputAction.PLAYER_ATTACK, InputDirection.RIGHT, KeyCode.D);

    }
	
    public void RegisterInputKey( InputAction key, InputDirection dir, KeyCode keyCode)
    {
        _keyInput.Get(key).Set(dir, keyCode );
    }
}
