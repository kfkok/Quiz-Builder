using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QuizBuilder
{
    public class QuizView : MonoBehaviour, IPointerClickHandler
    {
        #region Events

        public delegate void OnClickEventHandler(QuizView quizView);
        public event OnClickEventHandler OnClick;

        public delegate void OnClickDeleteEventHandler(QuizView quizView);
        public event OnClickDeleteEventHandler OnClickDelete;

        #endregion

        public Quiz quiz;
        public int index = 0;

        public TextMeshProUGUI quizIndex;
        public TextMeshProUGUI quizTitle;
        public TextMeshProUGUI questionCount;

        public void Load(int index, Quiz quiz)
        {
            this.index = index;
            this.quiz = quiz;

            Render();
        }

        public void Render()
        {
            quizIndex.text = index.ToString();
            quizTitle.text = quiz.title;
            questionCount.text = quiz.questions.Count.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }

        public void Delete()
        {
            OnClickDelete?.Invoke(this);
        }
    }
}


