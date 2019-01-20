using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCS.Characters
{
    public abstract class Character<S, I> : MonoBehaviour where S : CharacterState<I>
                                                          where I : CharacterInput, new()
    {

        protected I input = new I();
        protected S state = null;

        public abstract void readInput();

        private void Update()
        {
            readInput();
            state.runAnimation(input);
        }

        private void FixedUpdate()
        {
            state.runLogic(input);
        }

        public void newState<N>() where N : S
        {
            if (state != null)
            {
                state.exit(input);
                Destroy(state);
            }
            state = gameObject.AddComponent<N>();
            state.enter(input);
        }

    }

    public abstract class CharacterInput
    {

    }
}
