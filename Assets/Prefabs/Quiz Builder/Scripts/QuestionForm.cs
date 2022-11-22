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

        public List<OptionForm> optionForms = new List<OptionForm>();

        public void Load(Question question)
        {
            // Load the question
            this.question = question;
            questionTextField.text = question.text;

            LoadOptions();
        }

        void LoadOptions()
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
                    OptionForm optionForm = Instantiate(optionFormPrefab);
                    optionForm.Load(i + 1, question.options[i]);
                    optionForm.transform.SetParent(optionFormsContainer);
                    optionForms.Add(optionForm);
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
                    if (i == 0)
                    {
                        option.isCorrect = true;
                    }

                    question.AddOption(option);
                   
                    // Instantiate the option form and load the created options 
                    OptionForm optionForm = Instantiate(optionFormPrefab);
                    optionForm.Load(i + 1, option);
                    optionForm.transform.SetParent(optionFormsContainer);
                    optionForms.Add(optionForm);
                }
            }
        }

        public void Submit()
        {
            if (IsValid)
            {
                StatusBar.Print("Submitted question");
                OnSubmitted?.Invoke(question);
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

                foreach (OptionForm optionForm in optionForms)
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

