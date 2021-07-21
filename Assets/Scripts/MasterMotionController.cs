using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace CGber
{
    public class MasterMotionController : NetworkBehaviour
    {
        private Animator m_animator;

        // Start is called before the first frame update
        void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner) { return; }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (m_animator != null)
                {
                    // play Bounce but start at a quarter of the way though
                    //anim.Play("Base Layer.Bounce", 0, 0.25f);
                    m_animator.SetInteger("motion", 1);

                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                if (m_animator != null)
                {
                    m_animator.SetInteger("motion", 8);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (m_animator != null)
                {
                    m_animator.SetInteger("motion", 0);
                }
            }

            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_animator != null)
                {
                    m_animator.SetInteger("motion", 0);
                }
            }
            */
        }
    }
}

