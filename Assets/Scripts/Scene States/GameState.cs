using QuizBuilder;
using QuizBuilder.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneStates
{
    namespace MainScene
    {
        public class GameState : SceneState
        {
            public QuizBuilder.QuizBuilder quizBuilder;

            public override IEnumerator Run()
            {
                StatusBar.Print("Quiz Builder by Kok Keng Fai, developed for eon reality job application as Unity Developer");

                yield return null;
            }
        }
    }
}
