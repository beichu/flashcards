// This script controls showing and hiding the answer from the flashcard

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Diagnostics;
using System.Linq;
public class ShowAnswer : MonoBehaviour
{
    // Declare variables
    GameObject question;
    GameObject answer;
    GameObject answerButton;
    static bool answerIsShown; // need to set answerIsShown to static so there is only 1 instance of it.
    static int cardIndex; // this is the index of current card shown on screen.
    Button nextButton;
    Button previousButton;


    // initialize two lists called "questions" and "answers" to store the questions and answers for flashcards.
    static List<string> questions;
    static List<string> answers;


    // method to read csv file and split each line and add them to the two lists. 
    public void InitializeCards()
    {
        string filePath = "Assets/Data/FlashcardData.csv";
        using (StreamReader reader = new StreamReader(filePath))
        {
            questions = new List<string>(); // initializing questions and answers here because Awake() was called multiple times.
            answers = new List<string>();

            string line;

            while (reader.Peek() != -1)
            {
                line = reader.ReadLine();

                var values = line.Split(',');

                questions.Add(values[0]);
                answers.Add(values[1]);
            }
        }

    }



    // Initialize variables
    public void Awake()
    {
        InitializeCards();
        answerIsShown = false;
        question = GameObject.Find("Question");
        answer = GameObject.Find("Answer");
        answerButton = GameObject.Find("AnswerButton");
        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        previousButton = GameObject.Find("PreviousButton").GetComponent<Button>();

    }

    // set initial state for answer and button
    public void Start()
    {
        answer.SetActive(false);
        answerButton.GetComponentInChildren<Text>().text = "Show Answer";
        cardIndex = 0; // starting from 0 because there is no header row now
        question.GetComponent<Text>().text = questions[cardIndex];
        answer.GetComponent<Text>().text = answers[cardIndex];
    }

    // Function that shows the Answer when mouse hover over the card

    // Function that hides the Answer when mouse not hover over the   the card
    public void OnButtonClick()
    {
        answerIsShown = !answerIsShown;
        if (answerIsShown)
        {
            answer.SetActive(true);
            answerButton.GetComponentInChildren<Text>().text = "Hide Answer";

        }
        else
        {
            answer.SetActive(false);
            answerButton.GetComponentInChildren<Text>().text = "Show Answer";
            answerIsShown = false;
        }
    }

    // function to call when NextButton is clicked.
    public void buttonClickNext()
    {
        previousButton.interactable = true;
        cardIndex += 1;
        answerIsShown = false;
        answer.SetActive(false);
        answerButton.GetComponentInChildren<Text>().text = "Show Answer";
        question.GetComponent<Text>().text = questions[cardIndex];
        answer.GetComponent<Text>().text = answers[cardIndex];
        // If the cardIndex reaches the end of the list, make the button non-interactable.
        if (cardIndex >= questions.Count - 1)
        {
            nextButton.interactable = false;
        }
        else
        {
            if (nextButton.interactable == false)
            {
                nextButton.interactable = true;
            }
        }

    }

    // function to call when PreviousButton is clicked.
    public void buttonClickPrevious()
    {
        nextButton.interactable = true;

        cardIndex -= 1;
        answerIsShown = false;
        answer.SetActive(false);
        answerButton.GetComponentInChildren<Text>().text = "Show Answer";
        question.GetComponent<Text>().text = questions[cardIndex];
        answer.GetComponent<Text>().text = answers[cardIndex];
        if (cardIndex <= 0)
        {
            previousButton.interactable = false;


        }
        else
        {
            if (previousButton.interactable == false)
            {
                previousButton.interactable = true;
            }

        }
    }

    public void Shuffle()
    {
        List<string> shuffledQuestions = new List<string>();
        List<string> shuffledAnswers = new List<string>();
        System.Random rnd = new System.Random();
        List<int> idx = Enumerable.Range(0, questions.Count).ToList();

        int n = idx.Count;
        while (n > 1)
        {
            n--;
            int k = rnd.Next(n + 1); // k is a random index number
            int value = idx[k];
            idx[k] = idx[n]; // swap nth value and kth value
            idx[n] = value;
        }
        for (int i = 0; i < idx.Count; i++)
        {

            shuffledQuestions.Add(questions[idx[i]]);
            shuffledAnswers.Add(answers[idx[i]]);
        }
        questions = shuffledQuestions;
        answers = shuffledAnswers;

    }
}
