using QuizBuilder.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace QuizBuilder
{
    public class QuizForm : MonoBehaviour
    {
        #region Events

        // Quiz submitted event
        public delegate void OnQuizSubmittedEventHandler(Quiz quiz);
        public event OnQuizSubmittedEventHandler OnQuizSubmitted;

        // Quiz closed without changes event to the questions
        public delegate void OnQuizClosedEventHandler(Quiz quiz);
        public event OnQuizClosedEventHandler OnQuizClosed;

        #endregion

        public Quiz quiz = new Quiz();

        public TextMeshProUGUI quizTitle;
        public Transform questionViewContainer;
        public QuestionView questionViewPrefab;
        public QuestionForm questionForm;
        public QuizTitleForm quizTitleForm;
        public QuizPreviewer quizPreviewer;
        public Button previewQuizButton;
        public Button saveQuizButton;

        private List<Question> originalQuizQuestions = new List<Question>();

        private void Start()
        {
            questionForm.OnSubmitted += HandleQuestionOnSubmitted;
            quizTitleForm.OnSubmitted += HandleOnQuizTitleSubmitted;

            ShowQuizTitleForm();
            CloseQuizPreviewer();
            CloseQuestionForm();
        }

        private void HandleQuestionOnSubmitted(Question question)
        {
            if (quiz.questions.FirstOrDefault(q => q == question) == null)
            {
                quiz.AddQuestion(question);

                StatusBar.Print("Added question");
            }
            
            Render();
        }

        /// <summary>
        /// Handles after user submitted the quiz title
        /// In which we display (via Render method) the details of the quiz
        /// </summary>
        /// <param name="quiz"></param>
        private void HandleOnQuizTitleSubmitted(Quiz quiz)
        {
            CloseQuizTitleForm();

            Render();
        }

        public void LoadQuiz(Quiz quiz)
        {
            this.quiz = quiz;

            // Backup the original questions first, just in case the user closes the form without saving
            originalQuizQuestions = quiz.questions.ToList();

            ShowQuizTitleForm();
        }

        void ShowQuizTitleForm()
        {
            quizTitleForm.gameObject.SetActive(true);

            quizTitleForm.Load(quiz);
        }

        void CloseQuizTitleForm()
        {
            quizTitleForm.gameObject.SetActive(false);
        }

        public void Render()
        {
            // Set the title of the form
            quizTitle.text = quiz.title + " Detail";

            // Hide preview button if no questions
            previewQuizButton.gameObject.SetActive(quiz.questions.Count > 0);

            // Hide save quiz button if no questions
            saveQuizButton.gameObject.SetActive(quiz.questions.Count > 0);

            // Clear the question view container
            foreach (Transform child in questionViewContainer.transform)
            {
                if (child.gameObject.tag == "Header")
                {
                    continue;
                }

                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < quiz.questions.Count; i++)
            {
                InstantiateQuestionView(i + 1, quiz.questions[i]);
            }

            StartCoroutine(RefreshQuestionsLayout());
        }

        void InstantiateQuestionView(int index, Question question)
        {
            QuestionView questionView = Instantiate(questionViewPrefab);
            questionView.LoadQuestion(index, question);
            questionView.transform.SetParent(questionViewContainer);
            questionView.transform.localScale = Vector3.one;

            questionView.OnClick += (questionView) =>
            {
                ShowQuestionForm(questionView.question);
            };

            questionView.OnClickDelete += (questionView) =>
            {
                quiz.RemoveQuestion(questionView.question);
                Render();

                StatusBar.Print("Deleted question");
            };
        }

        IEnumerator RefreshQuestionsLayout()
        {
            VerticalLayoutGroup layout = questionViewContainer.GetComponent<VerticalLayoutGroup>();
            layout.enabled = false;

            yield return new WaitForEndOfFrame();
            layout.enabled = true;
        }

        /// <summary>
        /// Handles when the user click add question button
        /// </summary>
        public void AddQuestion()
        {
            ShowQuestionForm(new Question());
        }

        void ShowQuestionForm(Question question)
        {
            // Show the question form
            questionForm.gameObject.SetActive(true);

            // Load the question form with a new question
            questionForm.Load(question);
        }

        void CloseQuestionForm()
        {
            questionForm.gameObject.SetActive(false);
        }

        /// <summary>
        /// Handles when the user click submit button
        /// </summary>
        public void SubmitQuiz()
        {
            OnQuizSubmitted?.Invoke(quiz);

            // Close it self
            gameObject.SetActive(false);
        }

        public void Close()
        {
            quiz.questions = originalQuizQuestions;

            OnQuizClosed.Invoke(quiz);

            gameObject.SetActive(false);
        }

        public void PreviewQuiz()
        {
            quizPreviewer.gameObject.SetActive(true);

            quizPreviewer.LoadQuiz(quiz);
        }

        public void CloseQuizPreviewer()
        {
            quizPreviewer.gameObject.SetActive(false);
        }
    }
}
