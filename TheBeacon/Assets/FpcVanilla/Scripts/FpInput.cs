using System;
using UnityEngine;

public class FpInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public bool RunDown { get; private set; }
    public bool RunUp { get; private set; }
    public bool Jump { get; private set; }
    public bool EscapeDown { get; private set; }
    public bool LeftMouseButtonDown { get; private set; }
    public bool AnyMouseButtonDown { get; private set; }

    #region Events
    public event Action OnRunButtonDown;
    public event Action OnRunButtonUp;
    public event Action OnJumpButtonDown;
    public event Action OnEscButtonDown;
    public event Action OnAnyMouseButtonDown;
    public event Action OnLeftMouseButtonDown;
    #endregion

    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        RunDown = Input.GetButtonDown("Fire3"); //"Fire3" mapped to SHIFT key
        RunUp = Input.GetButtonUp("Fire3");
        Jump = Input.GetButtonDown("Jump");
        EscapeDown = Input.GetButtonDown("Cancel");
        LeftMouseButtonDown = Input.GetMouseButtonDown(0);
        AnyMouseButtonDown = LeftMouseButtonDown || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);

        if (Jump)
            OnJumpButtonDown?.Invoke();

        if (RunDown)
            OnRunButtonDown?.Invoke();

        if (RunUp)
            OnRunButtonUp?.Invoke();

        if (EscapeDown)
            OnEscButtonDown?.Invoke();

        if (LeftMouseButtonDown)
            OnLeftMouseButtonDown?.Invoke();

        if (AnyMouseButtonDown)
            OnAnyMouseButtonDown?.Invoke();
    }
}
