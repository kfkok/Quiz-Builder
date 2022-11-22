using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace QuizBuilder
{
    namespace Models
    {
        [Serializable]
        public class Quiz
        {
            public string title;

            [SerializeField]
            public List<Question> questions = new List<Question>();

            #region Constructors

            public Quiz(string title = null)
            {
                this.title = title;
            }

            public Quiz(string title, List<Question> questions)
            {
                this.title = title;
                this.questions = questions;
            }

            #endregion

            
            /// <summary>
            /// Adds a new question to the existing questions
            /// </summary>
            /// <param name="questionData"></param>
            public void AddQuestion(Question question)
            {
                questions.Add(question);
            }

            public void RemoveQuestion(Question question)
            {
                questions.Remove(question);
            }

            public bool HasValidTitle
            {
                get
                {
                    return title.Trim().Length > 0;
                }
            }

            /// <summary>
            /// Prints the entire quiz structure in the console.
            /// This allows a simple visualization of the quiz without needing a view to display it
            /// </summary>
            public void Print()
            {
                Debug.Log("-------  Quiz Title: " + title + " -------");

                for (int i = 0; i < questions.Count; i++)
                {
                    Debug.Log("Question " + i + ": " + questions[i]);
                }

                Debug.Log("------- End -------");
            }
        }
    }
}


