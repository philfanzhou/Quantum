using System;
using System.Globalization;
using System.Windows.Forms;
using Core.Expression;

namespace Framework.ExpressionCalc.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ExpressionContext exp = new ExpressionContext();
                int count = Convert.ToInt32(this.textBox2.Text);

                double result = 0;
                for (int i = 0; i < count; i++)
                {
                    result = exp.Evaluate(this.txtExpression.Text);
                }

                MessageBox.Show(result.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ExpressionContext context = new ExpressionContext();
                var expression = new Expression(this.txtExpression.Text);
                IEvaluator eval = context.CreateEvaluator(expression);
                int count = Convert.ToInt32(this.textBox2.Text);
                double result = 0;
                for (int i = 0; i < count; i++)
                {
                    result = eval.Evaluate();
                }

                MessageBox.Show(result.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
