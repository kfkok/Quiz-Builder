using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuizBuilder
{
    public class OptionPreviewView : MonoBehaviour
    {
        public Option option;
        public Toggle toggle;
        public TextMeshProUGUI optionText;

        private void Start()
        {
            // Find the outer nearest toggle group and set it as this toggle's group
            toggle.group = GetComponentInParent<ToggleGroup>();
        }

        public void Load(Option option)
        {
            this.option = option;

            Render();
        }

        public void Render()
        {
            optionText.text = option.text;
        }

        public bool IsCorrect
        {
            get
            {
                return toggle.isOn && option.isCorrect;
            }
        }
    }
}

