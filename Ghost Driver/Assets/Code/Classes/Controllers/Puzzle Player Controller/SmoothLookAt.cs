using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
    [Tooltip ("The target object to look at.")]
    public Transform _Target;
    [Tooltip ("Should the camera smooth to look at the target?")]
    public bool _ShouldSmooth = true;
    [Tooltip ("The amount of smoothing to apply to the tracking.")]
    public float _SmoothFactor = 6.0f;

    private Transform _Transform;

    void Awake ()
    {
        _Transform = GetComponent<Transform> ();
    }

    void FixedUpdate ()
    {
        if (_Target != null)
        {
            if (_ShouldSmooth)
                SmoothLook ();
            else
                Look ();
        }
    }

    private void Look ()
    {
        var targetPosition = new Vector3 (_Target.position.x, _Target.position.y, _Target.position.z);
        Vector3 targetDirection = (targetPosition - _Transform.position).normalized;

        _Transform.rotation = Quaternion.FromToRotation (-Vector3.forward, targetDirection);
    }

    private void SmoothLook ()
    {
        var rotation = Quaternion.LookRotation (_Target.position - _Transform.position);
        _Transform.rotation = Quaternion.Slerp (_Transform.rotation, rotation, Time.deltaTime * _SmoothFactor);
    }
}
