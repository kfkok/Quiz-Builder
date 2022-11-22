using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace QuizBuilder
{
    public class QuestionForm : MonoBehaviour
    {
        #region Events

        // Question submitted event
        public delegate void OnSubmittedEventHandler(Question question);
        public event OnSubmittedEventHandler OnSubmitted;

        #endregion

        public Question question;
        public TMP_InputField questionTextField;

        // Options 
        [Space]
        public Transform optionFormsContainer;
        public OptionForm optionFormPrefab;

        public void Load(Question question)
        {
            // Load the question
            this.question = question;

            Render();
        }

        void Render()
        {
            questionTextField.text = question.text;

            RenderOptions();
        }

        void RenderOptions()
        {
            foreach (Transform child in optionFormsContainer.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            // If options exists, load the options
            if (question.options != null && question.options.Count > 0)
            {
                for (int i = 0; i < question.options.Count; i++)
                {
                    InstantiateOptionForm(i + 1, question.options[i]);
                }
            } 

            // Else, prepare default three options
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    // Create a new option and add it to the question
                    Option option = new Option();

                    // Set the first option as is correct
                    option.isCorrect = (i == 0);

                    question.AddOption(option);

                    InstantiateOptionForm(i + 1, option);
                }
            }
        }

        void InstantiateOptionForm(int index, Option option)
        {
            OptionForm optionForm = Instantiate(optionFormPrefab);
            optionForm.Load(index, option);
            optionForm.transform.SetParent(optionFormsContainer);
            optionForm.transform.localScale = Vector3.one;
        }

        public void Submit()
        {
            if (IsValid)
            {
                StatusBar.Print("Submitted question");

                OnSubmitted?.Invoke(question);

                gameObject.SetActive(false);
            }
            else
            {
                StatusBar.Print("Question is not valid");
            }
        }

        public bool IsValid
        {
            get
            {
                int invalidOptionFormCount = 0;
                int correctOptionCount = 0;

                foreach (OptionForm optionForm in optionFormsContainer.GetComponentsInChildren<OptionForm>())
                {
                    if (!optionForm.IsValid)
                    {
                        invalidOptionFormCount++;
                    }

                    if (optionForm.Checked)
                    {
                        correctOptionCount++;
                    }
                }

                bool hasQuestionText = questionTextField.text.Trim().Length > 0;

                return (invalidOptionFormCount == 0) && (correctOptionCount > 0) && hasQuestionText;
            }
        }

        public void SetQuestionText(string text)
        {
            question.text = text;
        }
    }
}

