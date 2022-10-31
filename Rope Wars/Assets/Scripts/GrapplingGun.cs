using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private SpringJoint joint;

    private float maxDistance = 10f; 

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            StartGrapple();
        else if(Input.GetMouseButtonUp(0))
            StopGrapple();
        if(Input.GetKey(KeyCode.Space) && joint != null)
        {
            joint.maxDistance--;
        }

        if(Input.GetKey(KeyCode.LeftShift) && joint != null)
        {
            joint.maxDistance++;
        }
    }

    private void LateUpdate()
    {
        DrawRope();
        
    }

    private void StartGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            Debug.Log(hit.transform.position);
            Debug.Log(hit.point);
            Vector3 difTarget = hit.transform.position;
            Vector3 difHit = hit.point;

            if(Mathf.Abs(difTarget.x - difHit.x) > 0.5f)
            {
                if(difHit.x - difTarget.x < 0)
                    difHit.x -= difHit.x - (difTarget.x - 0.5f);
                if(difHit.x - difTarget.x > 0)
                    difHit.x -= difHit.x - (difTarget.x + 0.5f);
            }

            if(Mathf.Abs(difTarget.y - difHit.y) > 0.5f)
            {
                if(difHit.y - difTarget.y < 0)
                    difHit.y -= difHit.y - (difTarget.y - 0.5f);
                if(difHit.x - difTarget.y > 0)
                    difHit.y -= difHit.y - (difTarget.y + 0.5f);
            }

            if(Mathf.Abs(difTarget.z - difHit.z) > 0.5f)
            {
                if(difHit.z - difTarget.z < 0)
                    difHit.z -= difHit.z - (difTarget.z - 0.5f);
                if(difHit.z - difTarget.z > 0)
                    difHit.z -= difHit.z - (difTarget.z + 0.5f);
            }
            Debug.Log(difHit);
            hit.point = difHit;

            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.6f;
            joint.minDistance = distanceFromPoint * 0.2f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    private void DrawRope()
    {
        if(!joint) return;
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }
}
