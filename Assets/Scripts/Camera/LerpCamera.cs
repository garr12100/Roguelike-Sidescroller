using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCamera : MonoBehaviour
{
    public float lerpTime = .25f;
    public Camera _camera;
    public Transform target;
    public bool defaultToPlayer = true;

    private void Awake()
    {
        if (_camera == null)
            _camera = GetComponent<Camera>();
        if (defaultToPlayer)
        {
            target = GameObject.FindGameObjectWithTag(UtilityStrings.Tags.Player).transform;
        }
    }

    private void FixedUpdate()
    {
        if (_camera == null || target == null)
            return;
        Vector3 newPos = Vector3.Lerp(_camera.transform.position, target.transform.position, lerpTime);
        _camera.transform.position = new Vector3(newPos.x, newPos.y, _camera.transform.position.z);

    }
}
