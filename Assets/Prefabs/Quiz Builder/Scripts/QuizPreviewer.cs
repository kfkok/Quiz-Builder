using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuizBuilder
{
    public class QuizPreviewer : MonoBehaviour
    {
        #region

        public delegate void OnQuizPreviewCompleteEventHandler(QuizPreviewer quizPreviewer);
        public event OnQuizPreviewCompleteEventHandler OnQuizPreviewComplete;

        #endregion

        public Quiz quiz;

        public OptionPreviewView optionPreviewViewPrefab;

        public TextMeshProUGUI questionText;

        public Transform optionPreviewViewContainer;

        public TextMeshProUGUI questionIndexText;

        public TextMeshProUGUI scoreText;

        private Question currentQuestion;

        private int currentQuestionIndex = 0;

        private int questionCount = 0;

        public int score = 0;

        public GameObject scoreBoard;

        public TextMeshProUGUI finalScoreText;

        public void LoadQuiz(Quiz quiz)
        {
            this.quiz = quiz;

            Initialize();

            HideScoreBoard();

            Render();
        }

        void Initialize()
        {
            questionCount = quiz.questions.Count;

            currentQuestionIndex = 0;

            score = 0;

            currentQuestion = quiz.questions[currentQuestionIndex];
        }

        public void Render()
        {
            questionText.text = currentQuestion.text;

            questionIndexText.text = "Question " + (currentQuestionIndex + 1) + " / " + questionCount;

            scoreText.text = "Score " + score + " / " + questionCount;

            foreach (Transform child in optionPreviewViewContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            for (int i = 0; i < currentQuestion.options.Count; i++)
            {
                OptionPreviewView optionPreviewView = Instantiate(optionPreviewViewPrefab);
                optionPreviewView.Load(currentQuestion.options[i]);
                optionPreviewView.transform.SetParent(optionPreviewViewContainer);
                optionPreviewView.transform.localScale = Vector3.one;
            }

            StartCoroutine(RefreshOptionsLayout());
        }

        IEnumerator RefreshOptionsLayout()
        {
            VerticalLayoutGroup layout = optionPreviewViewContainer.GetComponent<VerticalLayoutGroup>();
            layout.enabled = false;

            yield return new WaitForEndOfFrame();
            layout.enabled = true;
        }

        public void NextQuestion()
        {
            if (CheckAnswerIsCorrect())
            {
                StatusBar.Print("Answer is correct");
                score++;
            }
            else
            {
                StatusBar.Print("Answer is wrong");
            }

            currentQuestionIndex++;

            // End
            if (currentQuestionIndex == questionCount)
            {
                ShowScore();
            }

            // Next question
            else
            {
                currentQuestion = quiz.questions[currentQuestionIndex];

                Render();
            }
        }

        bool CheckAnswerIsCorrect()
        {
            foreach (OptionPreviewView optionPreviewView in optionPreviewViewContainer.GetComponentsInChildren<OptionPreviewView>())
            {
                if (optionPreviewView.IsCorrect)
                {
                    return true;
                }
            }

            return false;
        }

        void ShowScore()
        {
            scoreBoard.gameObject.SetActive(true);

            finalScoreText.text = score.ToString() + " / " + questionCount.ToString();
        }

        void HideScoreBoard()
        {
            scoreBoard.gameObject.SetActive(false);
        }

        public void Complete()
        {
            OnQuizPreviewComplete?.Invoke(this);

            StatusBar.Print("Preview completed");

            gameObject.SetActive(false);
        }
    }
}

