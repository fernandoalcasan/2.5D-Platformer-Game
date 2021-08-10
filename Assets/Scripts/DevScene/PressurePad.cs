using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevScene
{
    public class PressurePad : MonoBehaviour
    {
        private MeshRenderer _meshRend;

        private void Start()
        {
            _meshRend = GetComponent<MeshRenderer>();
            if (_meshRend is null)
                Debug.LogError("MeshRenderer is NULL");
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("MovableBox"))
            {
                float distance = Vector3.Distance(transform.position, other.transform.position);

                if (distance < 0.5f)
                {
                    Rigidbody rb = other.attachedRigidbody;
                    if (!(rb is null))
                    {
                        rb.isKinematic = true;
                        _meshRend.material.color = Color.red;
                        Destroy(this);
                    }
                }
            }
        }
    }
}