using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MouseLook
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;

    private Quaternion _CharacterTargetRot;
    private Quaternion _CameraTargetRot;
    private bool _CursorIsLocked = true;
    private Camera _Camera = null;

    public void Init (Transform character, Transform camera)
    {
        _CharacterTargetRot = character.localRotation;
        _CameraTargetRot = camera.localRotation;
        _Camera = camera.GetComponent<Camera> ();
    }

    public void LookRotation (Transform character, Transform camera)
    {
        float yRot = Input.GetAxis ("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis ("Mouse Y") * YSensitivity;

        _CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
        _CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

        if (clampVerticalRotation)
            _CameraTargetRot = ClampRotationAroundXAxis (_CameraTargetRot);

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp (character.localRotation, _CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp (camera.localRotation, _CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = _CharacterTargetRot;
            camera.localRotation = _CameraTargetRot;
        }

        UpdateCursorLock ();
    }

    public void SetCursorLock (bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock ()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate ();
    }

    private void InternalLockUpdate ()
    {
        if (Input.GetKeyUp (KeyCode.Escape))
        {
            _CursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp (0))
        {
            _CursorIsLocked = true;
        }

        if (_CursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!_CursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis (Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
