using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS
{
    public class ProtagParenter : MonoBehaviour
    {
        private Characters.Protag protag = null;

        private void OnTriggerEnter(Collider other)
        {
            Characters.Protag p = other.gameObject.GetComponent<Characters.Protag>();

            if (p != null)
            {
                other.gameObject.transform.SetParent(this.transform, true);
                protag = p;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Characters.Protag p = other.gameObject.GetComponent<Characters.Protag>();

            if (p != null)
            {
                other.gameObject.transform.SetParent(null, true);
                protag = null;
            }
        }

        private void LateUpdate()
        {
        }
    }
}
