using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode {
       LookAt,
       LookAtInverted,
       CameraForward,
       CameraForwardInverted
    }


    [SerializeField] Mode mode;
    Vector3 dirFromCamera;
    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                dirFromCamera = this.transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position+dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
            default:
                dirFromCamera = this.transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
        }
    }
}
