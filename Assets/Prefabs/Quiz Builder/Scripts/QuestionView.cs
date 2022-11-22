using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QuizBuilder
{
    public class QuestionView : MonoBehaviour, IPointerClickHandler
    {
        #region Events

        // Click event
        public delegate void OnClickEventHandler(QuestionView questionView);
        public event OnClickEventHandler OnClick;

        // Delete event
        public delegate void OnClickDeleteEventHandler(QuestionView questionView);
        public event OnClickDeleteEventHandler OnClickDelete;

        #endregion

        public Question question;
        public int index;

        public TextMeshProUGUI questionIndex;
        public TextMeshProUGUI questionContent;

        public void LoadQuestion(int index, Question question)
        {
            this.index = index;

            Refresh(question);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }

        public void Refresh(Question question)
        {
            this.question = question;

            Render();
        }

        public void Render()
        {
            questionIndex.text = index.ToString();
            questionContent.text = question.text + "\n";

            for (int i = 0; i < question.options.Count; i++)
            {
                Option option = question.options[i];

                questionContent.text += "(" + i + ") " + (option.isCorrect ? "<color=green>": "") + option.text + (option.isCorrect ? "</color>" : "") + "\n";
            }
        }

        public void Delete()
        {
            OnClickDelete?.Invoke(this);
        }
    }
}