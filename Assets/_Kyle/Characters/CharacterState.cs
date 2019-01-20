using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TCS.Characters
{
    public abstract class CharacterState<I> : MonoBehaviour where I : CharacterInput
    {

        public abstract void enter(I input);

        public abstract void runAnimation(I input);

        public abstract bool runLogic(I input);

        public abstract void exit(I input);
    }
}
