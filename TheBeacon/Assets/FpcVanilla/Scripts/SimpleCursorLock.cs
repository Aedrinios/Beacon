using UnityEngine;

[RequireComponent(typeof(FpInput))]
public class SimpleCursorLock : MonoBehaviour
{
    [SerializeField] private bool lockOnStart = true;

    private FpInput input;

    private void OnEnable()
    {
        RegisterInputEvents();

        if (lockOnStart)
            LockCursor();
    }

    private void OnDisable()
    {
        UnregisterInputEvents();
    }

    #region Input Events
    void RegisterInputEvents()
    {
        input = GetComponent<FpInput>();
        input.OnAnyMouseButtonDown += LockCursor;
        input.OnEscButtonDown += UnlockCursor;
    }

    void UnregisterInputEvents()
    {
        input.OnAnyMouseButtonDown -= LockCursor;
        input.OnEscButtonDown -= UnlockCursor;
    }
    #endregion

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
