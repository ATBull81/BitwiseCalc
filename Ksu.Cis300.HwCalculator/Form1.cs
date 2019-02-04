/*Created By: Alec Turnbull
 *
 *
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.HwCalculator
{
    /// <summary>
    /// A GUI for a bitwise calculator
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Constructs the GUI
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            
        }

        private bool _display = false;
        private uint _binary;
        private Stack _expressions = new Stack();

        /// <summary>
        /// An event handler for all of the digit buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxD_Click(object sender, EventArgs e)
        {
            //HELP display issue after using an AND OR or XOR
            Button button = sender as Button;
            string buttonText = ((Button)sender).Text;
            if (_display == true)
            {
                //uxEquationBox.Clear();
                uxEquationBox.Text = buttonText;
                _display = false;
            }
            else if (uxEquationBox.Text == "0")
            {
                uxEquationBox.Clear();
                uxEquationBox.Text += buttonText;
                _display = false;
                //_expressions.Push(buttonText);
            }
            else if (uxEquationBox.Text.Length < 8)
            {
                uxEquationBox.Text += buttonText;
                //_expressions.Push(buttonText);
            }
        }
        
        /// <summary>
        /// Creates a precedence of all the operations.
        /// </summary>
        /// <param name="x">Name of operation</param>
        /// <returns>the number of their ranking of precedence</returns>
        private int uxPrecedence(string x)
        {
            int p;
            switch (x)
            {
                case "NOT":
                    p = 5;
                    break;

                case "AND":
                    p = 4;
                    break;

                case "XOR":
                    p = 3;
                    break;

                case "OR":
                    p = 2;
                    break;

                case ")":
                    p = 2;
                    break;

                case "(":
                    p = 1;
                    break;
                default:
                    throw new ArgumentException();

            }

            return p;
        }


        /// <summary>
        /// Receives the button pressed, and the two operans being used in the equation and either does the AND, OR, or XOR operation on them.
        /// </summary>
        /// <param name="x">string that is either AND, OR, or XOR</param>
        /// <param name="operan1">First half of equation</param>
        /// <param name="operan2">Second half of equation</param>
        /// <returns>solution</returns>
        private uint uxBinaryOperation(string x, uint operan1, uint operan2)
        {
            uint _result = 0;

            switch(x)
            {
                case "AND":
                    _result = operan1 & operan2;
                    break;

                case "XOR":
                    _result = operan1 ^ operan2;
                    break;

                case "OR":
                    _result = operan1 | operan2;
                    break;

            }
            return _result;
        } 

        /// <summary>
        /// Simplifies the expression
        /// </summary>
        /// <param name="x"> minimum precedence sent in</param>
        /// <param name="y"> operan to add to the current expression</param>
        /// <returns></returns>
        private uint uxSimplify(int x, uint y)
        {
           // _expressions.Pop();
            string b = (string) _expressions.Pop();
            string equation = (string)_expressions.Pop();
            uint _newnumbers = Convert.ToUInt32(equation, 16);
            // _binary = uxBinaryOperation(b, _numbers, y);
            uint number2;
            
                /*while ((string)_expressions.Peek() == "(")
                {
                    _expressions.Pop();
                b = (string)_expressions.Pop();
                number2 = Convert.ToUInt32(b, 16);
                _binary = uxBinaryOperation(equation, number2, y);
                b = (string)_expressions.Peek();

                }*/
           

                    number2 = Convert.ToUInt32(equation,16);
                    _binary = uxBinaryOperation(b, number2, y);
            return _binary;
        }

           
        

        /// <summary>
        /// returns the value of the portion of the current expression 
        /// beginning with the last open parenthesis.
        /// </summary>
        /// <param name="x">the value to be added to the current expression</param>
        /// <returns></returns>
        private uint uxCloseParent(uint x)
        {
            //fix
            //_expressions.Pop();
            
            return uxSimplify(uxPrecedence(")"), x);//(uint) _expressions.Pop();
            
        }
        
        /// <summary>
        /// A method that reacts to a user pressing the NOT button.
        /// Converts the string and solves the equation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxNot_Click(object sender, EventArgs e)
        {
            string equation = uxEquationBox.Text;
            uint solving = Convert.ToUInt32(equation,16);
            solving = 0xFFFFFFFF ^ solving;
            uxEquationBox.Clear();
            uxEquationBox.Text = solving.ToString("X");
            _display = true;

        }

        /// <summary>
        /// Pushes a ( onto the stack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxLeftParent_Click(object sender, EventArgs e)
        {
            _expressions.Push(uxEquationBox.Text);
            _expressions.Push("(");
            
        }

        /// <summary>
        /// Retrieves the value from the display and adds it plus a ")" to the expression
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxRightParent_Click(object sender, EventArgs e)
        {
            //HELP
            string value = uxEquationBox.Text;
            _expressions.Push(value);
            _expressions.Push(")");

    
            uxEquationBox.Text = uxCloseParent(Convert.ToUInt32(value, 16)).ToString("X");
        
            _display = true;

        }

        /// <summary>
        /// Handles the Equal button click response.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxEquals_Click(object sender, EventArgs e)
        {
            string expressed = uxEquationBox.Text;
            
            uxEquationBox.Text = uxCloseParent(Convert.ToUInt32(expressed, 16)).ToString("X");
            //uxEquationBox.Text = expressed.ToString();
            _display = true;
        }
        private void uxClear_Click(object sender, EventArgs e)
        {
            if (_display == true)
            {
                uxEquationBox.Clear();
                _expressions.Clear();
                uxEquationBox.Text = "0";
                _display = false;
            }
            else
            {
                uxEquationBox.Text = "0";
                _display = true;
            }
        }

        /// <summary>
        /// Handles the AND, OR, and XOR button responses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxAnd_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string buttonText = ((Button)sender).Text;
            _expressions.Push(uxEquationBox.Text);
            _expressions.Push(buttonText);
            uxPrecedence(buttonText);
            _display = true;
        }
    }
}
