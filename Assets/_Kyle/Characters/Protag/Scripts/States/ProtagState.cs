using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public abstract class ProtagState : CharacterState<ProtagInput>
    {
        protected Protag protag
        {
            get
            {
                if (p == null)
                    p = GetComponent<Protag>();
                return p;
            }
        }

        private Protag p;
    }
}
