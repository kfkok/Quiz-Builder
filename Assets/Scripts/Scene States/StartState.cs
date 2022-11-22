using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneStates
{
    namespace MainScene
    {
        public class StartState : SceneState
        {
            public override IEnumerator Run()
            {
                yield return null;

                Finished();
            }
        }
    }
}


