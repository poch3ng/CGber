using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;


public class StudentMotionController : NetworkBehaviour
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (m_animator != null)
            {
                m_animator.SetInteger("motion", 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_animator != null)
            {
                m_animator.SetInteger("motion", 2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_animator != null)
            {
                m_animator.SetInteger("motion", 0);
            }
        }
        
    }
}
