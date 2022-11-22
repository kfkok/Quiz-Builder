using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuizBuilder
{
    public class QuizTitleForm : MonoBehaviour
    {
        #region

        public delegate void OnQuizNameSubmittedEventHandler(Quiz quiz);
        public event OnQuizNameSubmittedEventHandler OnSubmitted;

        #endregion

        public Quiz quiz; 
        public TMP_InputField quizTitleTextField; 
        public Button SaveButton;

        public void Load(Quiz quiz)
        {
            this.quiz = quiz;

            quizTitleTextField.text = quiz.title;

            EnableSaveButtonOnValidTitle();
        }

        public void SetQuizTitle(string quizTitle)
        {
            quiz.title = quizTitle;

            EnableSaveButtonOnValidTitle();
        }

        void EnableSaveButtonOnValidTitle()
        {
            SaveButton.interactable = quiz.HasValidTitle;
        }

        public void SubmitQuizTitle()
        {
            OnSubmitted?.Invoke(quiz);
        }
    }
}
