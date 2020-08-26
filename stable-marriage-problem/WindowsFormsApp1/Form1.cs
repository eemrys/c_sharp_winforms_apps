using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1) && (comboBox4.SelectedIndex > -1))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1) && (comboBox4.SelectedIndex > -1))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameLogic.seed = Convert.ToInt32(numericUpDown1.Value);
            GameLogic.mode = comboBox3.SelectedIndex;
            GameLogic.difficulty = comboBox4.SelectedItem.ToString();
            GameLogic.menCount = int.Parse(comboBox1.SelectedItem.ToString());
            GameLogic.womenCount = int.Parse(comboBox2.SelectedItem.ToString());
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1) && (comboBox4.SelectedIndex > -1))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
            if (comboBox3.SelectedIndex == 0)
            {
                label5.Enabled = true;
                numericUpDown1.Enabled = true;
            }
            else
            {
                label5.Enabled = false;
                numericUpDown1.Enabled = false;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            if (comboBox4.SelectedIndex > -1)
            {
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                switch (comboBox4.SelectedIndex)
                {
                    case 0:
                        comboBox1.Items.Add(4);
                        comboBox1.Items.Add(5);
                        comboBox2.Items.Add(4);
                        comboBox2.Items.Add(5);
                        break;
                    case 1:
                        comboBox1.Items.Add(6);
                        comboBox1.Items.Add(7);
                        comboBox2.Items.Add(6);
                        comboBox2.Items.Add(7);
                        break;
                    case 2:
                        comboBox1.Items.Add(8);
                        comboBox1.Items.Add(9);
                        comboBox2.Items.Add(8);
                        comboBox2.Items.Add(9);
                        break;
                }
            }
            else
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }

            if ((comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1) && (comboBox4.SelectedIndex > -1))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label5.Enabled = false;
            numericUpDown1.Enabled = false;
            if (GameLogic.lang == 0)
            {
                label5.Text = "Variant:";
                label3.Text = "Mode:";
                comboBox3.Items.Clear();
                comboBox3.Items.Add("generate data");
                comboBox3.Items.Add("manual input");
                label4.Text = "Difficulty:";
                comboBox4.Items.Clear();
                comboBox4.Items.Add("easy");
                comboBox4.Items.Add("medium");
                comboBox4.Items.Add("hard");
                label1.Text = "Number of men:";
                label2.Text = "Number of women:";
                button1.Text = "Start";
                textBox1.Text = "Solve the problem of finding a stable matching between two sets of elements given an ordering of preferences for each element.";
            }
            textBox1.Update();
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
        }
    }
}
