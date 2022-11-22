using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizBuilder
{
    namespace Models
    {
        [Serializable]
        public class Question
        {
            public string text;

            public List<Option> options = new List<Option>();

            #region Constructors

            public Question(string text = null)
            {
                this.text = text;
            }

            public Question(string text, List<Option> options)
            {
                this.text = text;
                this.options = options;
            }

            #endregion

            public void AddOption(Option option)
            {
                options.Add(option);
            }

            public void RemoveOption(Option option)
            {
                options.Remove(option);
            }

            public override string ToString()
            {
                var s = text + "\n\n";

                for (int i = 0; i < options.Count; i++)
                {
                    s += i + ") " + options[i] + "\n";
                }

                return s;
            }
        }
    }
}