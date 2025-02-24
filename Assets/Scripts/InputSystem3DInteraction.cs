using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem3DInteraction : MonoBehaviour
{
    public Camera mainCamera;
    public InputAction clickAction;
    Vector2 mouseEnterPos;
    Vector2 mouseExitPos;
    bool isInteractable;
    public SliceDir dirToPass;
    public string id;
    public LevelManager levelManager;

    private void OnEnable()
    {
        clickAction.Enable();
        clickAction.performed += onClickPerformed;
        clickAction.canceled += onClickCanceled;
    }

    private void OnDisable()
    {
        clickAction.performed -= onClickPerformed;
        clickAction.canceled -= onClickCanceled;
        clickAction.Disable();
    }
    private void onClickCanceled(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = Vector2.zero;

        if (Touchscreen.current != null)
        {
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            touchPosition = Mouse.current.position.ReadValue();
        }

        if (isInteractable)
        {
            mouseExitPos = touchPosition;
            ReadDirection(mouseEnterPos, mouseExitPos);
        }
    }
    private void onClickPerformed(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = Vector2.zero;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            touchPosition = Mouse.current.position.ReadValue();
        }

        mouseEnterPos = touchPosition;
        Ray ray = mainCamera.ScreenPointToRay(mouseEnterPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
            {
                id = hit.collider.gameObject.GetComponent<SliceRef>().id;
                isInteractable = true;
            }
        }
    }

    private void ReadDirection(Vector2 mouseEnterPos, Vector2 mouseExitPos)
    {
        //calcolo dei due v2 che capisce la dir
        Vector2 delta = mouseExitPos - mouseEnterPos;
        Vector2 direction = Vector2.zero;

        //if (Mathf.Abs(delta.x) > 4f)
        //{
        //    direction.x = Mathf.Clamp(delta.x, -1, 1);
        //}
        //if (Mathf.Abs(delta.y) > 4f)
        //{
        //    direction.y = Mathf.Clamp(delta.y, -1, 1);
        //}
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if(delta.x > 0)
            {
                dirToPass = SliceDir.RIGHT;
            }
            else
            {
                dirToPass = SliceDir.LEFT;
            }
        }
        else
        {
            if (delta.y > 0)
            {
                dirToPass = SliceDir.UP;
            }
            else
            {
                dirToPass = SliceDir.DOWN;
            }
        }
        levelManager.SliceMovement(dirToPass, id);

        //Vector2 directionRounded = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        //if (direction != Vector2.zero && directionRounded != new Vector2(1, 1))
        //{
        //    if (direction == new Vector2(1, 0))
        //    {
        //        dirToPass = SliceDir.RIGHT;
        //    }
        //    else if (direction == new Vector2(-1, 0))
        //    {
        //        dirToPass = SliceDir.LEFT;
        //    }
        //    else if (direction == new Vector2(0, -1))
        //    {
        //        dirToPass = SliceDir.DOWN;
        //    }
        //    else if (direction == new Vector2(0, 1))
        //    {
        //        dirToPass = SliceDir.UP;
        //    }
        //    //Debug.Log(levelManager);
        //    //Debug.Log(dirToPass+" "+ id);

        //}

    }
}

