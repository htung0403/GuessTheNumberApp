using GuessTheNumberApp.FormComponent;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Microsoft.VisualBasic.Interaction;


namespace GuessTheNumberApp
{
    public partial class Form1 : Form
    {
        string number = "";
        int currentScore;
        private GuessingGame game;
        private Button[] Answered = new Button[10];
        List<int> guessedNumbers = new List<int>();
        bool isTooHigh;

        public Form1()
        {
            InitializeComponent();
            game = new GuessingGame();
            currentScore = game.Score;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            numericTxtNumber.ReadOnly = true;
            playAgainBtn.Visible = false;
            saveScoreBtn.Visible = false;
            turnlbl.Text = game.GetRemainingTurns().ToString();
            scorelbl.Text = game.Score.ToString();
            LoadHighScore();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            number= string.Empty;
            numericTxtNumber.Text = "";
        }
        #region numpad
        private void pic1_MouseClick(object sender, MouseEventArgs e)
        {
            
            number += 1;
            numericTxtNumber.Text = number;
        }

        private void pic2_MouseClick(object sender, MouseEventArgs e)
        {
            number += 2;
            numericTxtNumber.Text = number;
        }

        private void pic3_MouseClick(object sender, MouseEventArgs e)
        {
            number += 3;
            numericTxtNumber.Text = number;
        }

        private void pic4_MouseClick(object sender, MouseEventArgs e)
        {
            number += 4;
            numericTxtNumber.Text = number;
        }

        private void pic5_MouseClick(object sender, MouseEventArgs e)
        {
            number += 5;
            numericTxtNumber.Text = number;
        }

        private void pic6_MouseClick(object sender, MouseEventArgs e)
        {
            number += 6;
            numericTxtNumber.Text = number;
        }

        private void pic7_MouseClick(object sender, MouseEventArgs e)
        {
            number += 7;
            numericTxtNumber.Text = number;
        }

        private void pic8_MouseClick(object sender, MouseEventArgs e)
        {
            number += 8;
            numericTxtNumber.Text = number;
        }

        private void pic9_MouseClick(object sender, MouseEventArgs e)
        {   
            number += 9;
            numericTxtNumber.Text = number;
        }

        private void pic0_MouseClick(object sender, MouseEventArgs e)
        {
            number += 0;
            numericTxtNumber.Text = number;
        }
        #endregion
        private void btnGuess_Click(object sender, EventArgs e)
        {
            int.TryParse(numericTxtNumber.Text, out int guess);

            if (!game.CheckValidAnswer(guess))
            {
                MessageBox.Show("Please enter numbers between 1 and 100!");
                return;
            }

            if (guessedNumbers.Contains(guess))
            {
                number = "";
                numericTxtNumber.Text = "";
                MessageBox.Show("You have already guessed this number. Try another one!");
                return;
            }

            guessedNumbers.Add(guess);

            if (game.CheckTheAnswer(guess, out isTooHigh))
            {
                MessageBox.Show("Congrats");
                playAgainBtn.Visible = true;
                btnGuess.Visible = false;
                saveScoreBtn.Visible = true;
                playAgainBtn.Text = "CONTINUE ?";
                currentScore = game.Score;
                scorelbl.Text = game.Score.ToString();
            }
            else
            {
                switch (game.GetRemainingTurns())
                {
                    case 0:
                        MessageBox.Show($"The correct answer was {game.NumNeedToGuess}!", "You're out of turns");
                        playAgainBtn.Visible = true;
                        btnGuess.Visible = false;
                        saveScoreBtn.Visible = true;
                        assignTheAnswer(guess, false);
                        playAgainBtn.Text = "PLAY AGAIN?";
                        break;
                    default:
                        if (isTooHigh)
                        {
                            number = "";
                            numericTxtNumber.Text = "";
                            MessageBox.Show($"{guess} is too high, guess again!");
                            assignTheAnswer(guess, false);
                        }
                        else
                        {
                            number = "";
                            numericTxtNumber.Text = "";
                            MessageBox.Show($"{guess} is too low, guess again!");
                            assignTheAnswer(guess, false);
                        }
                        turnlbl.Text = game.GetRemainingTurns().ToString();
                        numericTxtNumber.Text = "";
                        break;
                }
            }
        }

        private int assignedButtonIndex = -1;
        private void resetAnswerButtons()
        {
            assignedButtonIndex = -1;
            for (int i = 0; i < Answered.Length; i++)
            {
                Answered[i].Text = "";
            }
        }
        void assignTheAnswer(int answer, bool resetButtons)
        {
            Answered = new[] { answer1, answer2, answer3, answer4, answer5,
                       answer6, answer7, answer8, answer9, answer10 };

            scorelbl.Text = game.Score.ToString();

            if (resetButtons)
            {
                resetAnswerButtons();
                return;
            }

            for (int i = 0; i < Answered.Length; i++)
            {
                if (assignedButtonIndex >= i)
                {
                    continue;
                }

                if (game.CheckValidAnswer(answer))
                {
                    if (!isTooHigh)
                    {
                        Answered[i].Text = ">" + answer.ToString();

                        assignedButtonIndex = i;

                        break;
                    }
                    else
                    {
                        Answered[i].Text = "<" + answer.ToString();

                        assignedButtonIndex = i;

                        break;
                    }
                }
            }
        }
        private void ResetGameState(int lastScore)
        {
            number = "";
            guessedNumbers.Clear();
            game = new GuessingGame();
            game.AddScore(lastScore);
        }
        private void ResetUI()
        {
            number = "";
            numericTxtNumber.Text = "";
            assignTheAnswer(-1, true);
            playAgainBtn.Visible = false;
            btnGuess.Visible = true;
            guessedNumbers.Clear();
            turnlbl.Text = game.GetRemainingTurns().ToString();
            scorelbl.Text = game.Score.ToString();
        }
        private void playAgainBtn_Click(object sender, EventArgs e)
        {
            if (playAgainBtn.Text.Equals("CONTINUE ?"))
            {
                currentScore = game.Score;
                ResetGameState(currentScore);
                saveScoreBtn.Visible = false;
            } else
            {
                ResetGameState(0);
            }

            ResetUI();
        }
       
        private void giveupbtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"The correct answer was {game.NumNeedToGuess}!");
            playAgainBtn.Visible = true;
            btnGuess.Visible = false;
        }

        private void LoadHighScore()
        {
            string highScoreFilePath = Path.Combine(Application.StartupPath, "highScore.txt");
            if (File.Exists(highScoreFilePath))
            {
                string highScoreText = File.ReadAllText(highScoreFilePath);
                highscorelbl.Text = highScoreText;
            }
            else
            {
                highscorelbl.Text = "0";
            }
        }

        private void SaveScore()
        {
            string highScoreFilePath = Path.Combine(Application.StartupPath, "highScore.txt");

            if (!File.Exists(highScoreFilePath))
            {
                File.Create(highScoreFilePath);
            }

            if (currentScore > int.Parse(File.ReadAllText("highScore.txt")))
            {
                File.WriteAllText("highScore.txt", currentScore.ToString());
            }
        }
        private void saveScoreBtn_Click(object sender, EventArgs e)
        {
            SaveScore();

            ResetGameState(0);
            ResetUI();
            LoadHighScore();
            saveScoreBtn.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveScore();
        }
    }
}
