using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuizBuilder
{
    public class OptionForm : MonoBehaviour
    {
        public Option option; 
        public Toggle toggle;
        public TMP_InputField inputField;
        public TextMeshProUGUI optionIndexText;

        private void Start()
        {
            // Find the outer nearest toggle group and set it as this toggle's group
            toggle.group = GetComponentInParent<ToggleGroup>();
        }

        public void Load(int index, Option option)
        {
            this.option = option;
            optionIndexText.text = "Option " + index.ToString();

            toggle.isOn = option.isCorrect;
            inputField.text = option.text;
        }

        public bool Checked
        {
            get { return toggle.isOn; }
        }

        public void SetOptionIsCorrect(bool isCorrect)
        {
            option.isCorrect = isCorrect;
        }

        /// <summary>
        /// Real time updates the option text when typing on text field  
        /// </summary>
        /// <param name="text"></param>
        public void SetOptionText(string text)
        {
            option.text = text;
        }

        public bool IsValid
        {
            get
            {
                return option.IsValid;
            }
        }
    }
}