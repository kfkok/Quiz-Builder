using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizBuilder
{
    namespace Models
    {
        [Serializable]
        public class Option
        {
            /// <summary>
            /// The text associated with this option.
            /// </summary>
            public string text;

            /// <summary>
            /// Indicates whether this option is correct
            /// </summary>
            public bool isCorrect;

            #region Constructors

            public Option(string text = null)
            {
                this.text = text;
            }

            public Option(string text, bool isCorrect)
            {
                this.text = text;
                this.isCorrect = isCorrect;
            }

            #endregion

            public override string ToString()
            {
                return text + (isCorrect ? " [Correct Anwser]" : "");
            }

            public bool IsValid
            {
                get
                {
                    return text.Trim().Length > 0;
                }
            }
        }
    }
}