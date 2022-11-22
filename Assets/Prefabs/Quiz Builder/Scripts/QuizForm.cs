using QuizBuilder.Models;
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
            quizPreviewer.OnQuizPreviewComplete += HandleOnQuizPreviewComplete;

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

            CloseQuestionForm();
            
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

        private void HandleOnQuizPreviewComplete(QuizPreviewer quizPreviewer)
        {
            CloseQuizPreviewer();
        }

        public void LoadQuiz(Quiz quiz)
        {
            this.quiz = quiz;

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
                QuestionView questionView = Instantiate(questionViewPrefab);
                questionView.LoadQuestion(i + 1, quiz.questions[i]);
                questionView.transform.SetParent(questionViewContainer);
                questionView.transform.localScale = Vector3.one;
                questionView.OnClick += (questionView) => 
                {
                    ShowQuestionForm(questionView.question);
                };
                questionView.OnClickDelete += (questionView) =>
                {
                    Question question = quiz.questions.SingleOrDefault(q => q == questionView.question);

                    if (question != null)
                    {
                        quiz.RemoveQuestion(question);
                        Render();
                    }

                    StatusBar.Print("Deleted question");
                };
            }

            StartCoroutine(RefreshQuestionsLayout());
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
        }

        public void Close()
        {
            quiz.questions = originalQuizQuestions;

            this.gameObject.SetActive(false);
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
