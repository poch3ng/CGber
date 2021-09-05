using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

namespace CGber
{
    public class AvatarController : NetworkBehaviour
    {

        private float scaleValue = 0.0f;
        private float scaleChangeValue;
        private Vector3 scaleChange;


        private Vector3 mOffset;
        private float mZCoord;

        void OnMouseDown()
        {
            // if (!IsOwner) { return; }

            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

            // Store offset = gameobject world pos - mouse world pos
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        }


        private Vector3 GetMouseAsWorldPoint()
        {
            // Pixel coordinates of mouse (x,y)
            Vector3 mousePoint = Input.mousePosition;

            // z coordinate of game object on screen
            mousePoint.z = mZCoord;

            // Convert it to world points
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        void OnMouseDrag()
        {
            if (!IsOwner) { return; }
                
            transform.position = GetMouseAsWorldPoint() + mOffset;
        }

        public void OnMouseOver()
        {
            if (!IsOwner) { return; }

            scaleValue += Input.mouseScrollDelta.y * 0.1f;

            scaleChangeValue = 1.0f + scaleValue;

            scaleChange = new Vector3(scaleChangeValue, scaleChangeValue, 1.0f);

            transform.localScale = scaleChange;

        }

    }

}

