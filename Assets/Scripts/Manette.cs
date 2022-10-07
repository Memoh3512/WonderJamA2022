using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public enum RumbleForce
{
    
    VeryWeak, Weak, Medium, Strong, Max
    
}

public class Manette
{
    private readonly bool isKeyboardandMouse;
    public readonly Gamepad gp;
    private readonly Keyboard kb;
    private readonly Mouse mouse;

    public Manette(Gamepad game)
    {

        isKeyboardandMouse = false;
        gp = game;
        kb = Keyboard.current;
        mouse = Mouse.current;

    }

    public Manette()
    {

        isKeyboardandMouse = true;
        kb = Keyboard.current;
        mouse = Mouse.current;
        gp = new Gamepad();

    }
    
    public async void Rumble(float nbSecs, RumbleForce force = RumbleForce.Medium)
    {
        
        switch (force)
        {
            case RumbleForce.VeryWeak:
                gp.SetMotorSpeeds(0.1f, 0.1f);
                break;
            case RumbleForce.Weak:
                gp.SetMotorSpeeds(0.25f, 0.25f);
                break;
            case RumbleForce.Medium:
                gp.SetMotorSpeeds(0.5f, 0.5f);
                break;
            case RumbleForce.Strong:
                gp.SetMotorSpeeds(0.75f, 0.75f);
                break;
            case RumbleForce.Max:
                gp.SetMotorSpeeds(1f, 1f);
                break;
            
        }
        await Task.Delay((int)(1000 * nbSecs));
        gp.ResetHaptics();

    }
    
    public async void Rumble(float nbSecs, float lMotorForce, float rMotorForce)
    {

        lMotorForce = Mathf.Clamp(lMotorForce, 0f, 1f);
        rMotorForce = Mathf.Clamp(rMotorForce, 0f, 1f);
        
        gp.SetMotorSpeeds(lMotorForce, rMotorForce);
        await Task.Delay((int)(1000 * nbSecs));
        gp.ResetHaptics();

    }

    public ButtonControl aButton => isKeyboardandMouse ? KBControls.GetAButton(kb) : gp?.aButton;

    public ButtonControl bButton => isKeyboardandMouse ? KBControls.GetBButton(kb) : gp?.bButton;
    
    public ButtonControl xButton => isKeyboardandMouse ? KBControls.GetXButton(kb) : gp?.xButton;
    
    public ButtonControl yButton => isKeyboardandMouse ? KBControls.GetYButton(kb) : gp?.yButton;

    public ButtonControl buttonEast => isKeyboardandMouse ? KBControls.GetBButton(kb) : gp?.buttonEast;

    public ButtonControl buttonNorth => isKeyboardandMouse ? KBControls.GetYButton(kb) : gp?.buttonNorth;

    public ButtonControl buttonWest => isKeyboardandMouse ? KBControls.GetXButton(kb) : gp?.buttonWest;

    public ButtonControl buttonSouth => isKeyboardandMouse ? KBControls.GetAButton(kb) : gp?.buttonSouth;

    public ButtonControl leftShoulder => isKeyboardandMouse ? KBControls.GetLeftShoulder(kb) : gp?.leftShoulder;

    public ButtonControl leftTrigger => isKeyboardandMouse ? KBControls.GetLeftTrigger(mouse) : gp?.leftTrigger;

    public ButtonControl rightShoulder => isKeyboardandMouse ? KBControls.GetRightShoulder(kb) : gp?.rightShoulder;

    public ButtonControl rightTrigger => isKeyboardandMouse ? KBControls.GetRightTrigger(mouse) : gp?.rightTrigger;

    public ButtonControl startButton => isKeyboardandMouse ? KBControls.GetStartButton(kb) : gp?.startButton;
    public ButtonControl selectButton => isKeyboardandMouse ? KBControls.GetSelectButton(kb) : gp?.selectButton;
    public ButtonControl joinButton => isKeyboardandMouse ? KBControls.GetAButton(kb) : gp?.startButton;
    public Vector2 leftStick
    {

        get
        {

            Vector2 p = Vector2.zero;
            
            if (isKeyboardandMouse)
            {
                
                if (kb.aKey.isPressed) p.x--;
                if (kb.dKey.isPressed) p.x++;
                if (kb.sKey.isPressed) p.y--;
                if (kb.wKey.isPressed) p.y++;
                Debug.Log(p.ToString());
                
            }
            else
            {

                p = gp.leftStick.ReadValue();

            }

            return p;

        }
        
    }

    public Vector2 rightStick
    {
        
        get
        {
            
            Vector2 p = Vector2.zero;
            
            if (isKeyboardandMouse)
            {
                
                if (kb.leftArrowKey.isPressed) p.x--;
                if (kb.rightArrowKey.isPressed) p.x++;
                if (kb.downArrowKey.isPressed) p.y--;
                if (kb.upArrowKey.isPressed) p.y++;

            }
            else
            {

                p = gp.rightStick.ReadValue();

            }

            return p;
            
        }
        
    }
    
    public Vector2 dpad
    {

        get
        {

            if (isKeyboardandMouse)
            {

                float x = 0, y = 0;
                if (kb.leftArrowKey.isPressed) x = -1;
                if (kb.rightArrowKey.isPressed) x = 1;
                if (kb.downArrowKey.isPressed) y = -1;
                if (kb.upArrowKey.isPressed) y = 1;
                return new Vector2(x,y);

            }
            else
            {

                return gp.dpad.ReadValue();

            }

        }
        
    }
    
    #region LeftJoystickButtons

    public ButtonControl lJoyLeft => isKeyboardandMouse ? kb.aKey : gp?.leftStick.left;
    public ButtonControl lJoyRight => isKeyboardandMouse ? kb.dKey : gp?.leftStick.right;
    public ButtonControl lJoyUp => isKeyboardandMouse ? kb.wKey : gp?.leftStick.up;
    public ButtonControl lJoyDown => isKeyboardandMouse ? kb.sKey : gp?.leftStick.down;
    
    #endregion
    
    #region RightJoystickButtons

    public ButtonControl rJoyLeft => isKeyboardandMouse ? kb.leftArrowKey : gp?.rightStick.left;
    public ButtonControl rJoyRight => isKeyboardandMouse ? kb.rightArrowKey : gp?.rightStick.right;
    public ButtonControl rJoyUp => isKeyboardandMouse ? kb.upArrowKey : gp?.rightStick.up;
    public ButtonControl rJoyDown => isKeyboardandMouse ? kb.downArrowKey : gp?.rightStick.down;
    
    #endregion
    
    #region DPadButtons

    public ButtonControl dpLeft => isKeyboardandMouse ? kb.numpad4Key : gp?.dpad.left;
    public ButtonControl dpRight => isKeyboardandMouse ? kb.numpad6Key : gp?.dpad.right;
    public ButtonControl dpUp => isKeyboardandMouse ? kb.numpad8Key : gp?.dpad.up;
    public ButtonControl dpDown => isKeyboardandMouse ? kb.numpad2Key : gp?.dpad.down;
    
    #endregion
    public Vector2 mousePosition => mouse.position.ReadValue();

    public Vector2 mouseDelta => mouse.delta.ReadValue();

    public ButtonControl leftClick => mouse.leftButton;

    public ButtonControl rightClick => mouse.rightButton;
    
}

public static class KBControls
{

    public static ButtonControl GetAButton(Keyboard kb) => kb.spaceKey;

    public static ButtonControl GetBButton(Keyboard kb) => kb.fKey;

    public static ButtonControl GetXButton(Keyboard kb) => kb.eKey;

    public static ButtonControl GetYButton(Keyboard kb) => kb.qKey;

    public static ButtonControl GetStartButton(Keyboard kb) => kb.enterKey;

    public static ButtonControl GetSelectButton(Keyboard kb) => kb.backspaceKey;

    public static ButtonControl GetLeftTrigger(Mouse ms) => ms.leftButton;

    public static ButtonControl GetRightTrigger(Mouse ms) => ms.rightButton;

    public static ButtonControl GetLeftShoulder(Keyboard kb) => kb.zKey;

    public static ButtonControl GetRightShoulder(Keyboard kb) => kb.xKey;
}
