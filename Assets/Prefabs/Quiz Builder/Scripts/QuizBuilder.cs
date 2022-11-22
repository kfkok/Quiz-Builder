using QuizBuilder.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

namespace QuizBuilder
{
    public class QuizBuilder : MonoBehaviour
    {
        // The horizontal layout container where all the quiz views are contained
        public Transform quizViewsContainer;
        
        // The prefab of the created quiz that will be instantiated and be placed inside the quizViewContainer
        public QuizView quizViewPrefab;
        
        // The form that contains the details of a particular quiz, a.k.a Quiz Details
        public QuizForm quizForm;

        public Button saveQuizzesButton;

        [SerializeField]
        private List<Quiz> quizzes = new List<Quiz>();

        // Start is called before the first frame update
        void Start()
        {
            quizzes = new List<Quiz>();

            quizForm.OnQuizSubmitted += HandleOnQuizSubmitted;

            HideQuizForm();

            Render();
        }

        public void ShowQuizMenu()
        {
            HideQuizForm();

            Render();
        }

        public void CreateQuiz()
        {
            ShowQuizForm(new Quiz());
        }

        void ShowQuizForm(Quiz quiz)
        {
            quizForm.gameObject.SetActive(true);

            quizForm.LoadQuiz(quiz);
        }

        void HideQuizForm()
        {
            quizForm.gameObject.SetActive(false);
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            string savePath = Application.persistentDataPath + "/Quiz Builder.dat";
            FileStream file = File.Create(savePath);
            bf.Serialize(file, quizzes);
            file.Close();

            StatusBar.Print("Quizzes saved to " + savePath);
        }

        public void Load()
        {
            string savePath = Application.persistentDataPath + "/Quiz Builder.dat";

            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                quizzes = (List<Quiz>) bf.Deserialize(file);
                file.Close();

                StatusBar.Print("Quizzes loaded from " + savePath);
            }
            else
            {
                StatusBar.Print("There is no save data!");
            }

            Render();
        }

        public void Render()
        {
            // Clear all the quiz views in the container
            foreach (Transform child in quizViewsContainer.transform)
            {
                // Do not clear the header
                if (child.gameObject.tag == "Header")
                {
                    continue;
                }

                GameObject.Destroy(child.gameObject);
            }

            // Enable or disable the save quizzes button depending whether there are any quizzes created
            saveQuizzesButton.interactable = quizzes.Count > 0;

            // Populate the quiz views container with quiz views
            for (int i = 0; i < quizzes.Count; i++)
            {
                Quiz quiz = quizzes[i];

                QuizView quizView = Instantiate(quizViewPrefab);
                quizView.Load(i + 1, quiz);

                // When user click the quiz, load the quiz details form
                quizView.OnClick += (QuizView view) => {
                    ShowQuizForm(view.quiz);
                };

                // When user click the quiz delete button, remove the quiz from the list 
                quizView.OnClickDelete += (QuizView) => {
                    Quiz q = quizzes.SingleOrDefault(q => q == quiz);

                    if (q != null)
                    {
                        quizzes.Remove(quiz);
                        Render();
                    }
                };

                quizView.transform.SetParent(quizViewsContainer);
            }
        }

        public void HandleOnQuizSubmitted(Quiz quiz)
        {
            Quiz q = quizzes.SingleOrDefault(q => q == quiz);

            if (q == null)
            {
                quizzes.Add(quiz);
            }

            ShowQuizMenu();
        }
    }
}
