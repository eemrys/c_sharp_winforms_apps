using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private string m_label = "М.";
        private string w_label = "Ж.";
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label4.Text = "";
            dataGridView3.Rows.Clear();
            dataGridView3.Rows.Add();
            dataGridView3.Rows[0].HeaderCell.Value = w_label;
            ColorCells();
            dataGridView3.ClearSelection();
            dataGridView3.ReadOnly = false;
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label4.Text = "";
            ColorCells();
            dataGridView3.ReadOnly = true;
            button3.Enabled = false;
            dataGridView3.Rows.Clear();
            dataGridView3.Rows.Add();
            dataGridView3.Rows[0].HeaderCell.Value = w_label;
            
            dataGridView3.ClearSelection();
            GameLogic.Calculate();
            for (int i = 0; i < dataGridView3.Rows[0].Cells.Count; i++)
            {
                string temp = (GameLogic.pairs[i] + 1).ToString();
                if (temp == "0") { temp = ""; }
                dataGridView3.Rows[0].Cells[i].Value = temp;
            }
            dataGridView3.ClearSelection();
            sizeDGV(dataGridView3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            label4.Text = "";
            dataGridView3.ClearSelection();
            GameLogic.Calculate();
            List<int> answer = new List<int>(GameLogic.pairs.Count);
            answer.AddRange(Enumerable.Repeat(-1, answer.Capacity));
            for (int i = 0; i < dataGridView3.Rows[0].Cells.Count; i++)
            {
                if (dataGridView3.Rows[0].Cells[i].Value != null)
                {
                    answer[i] = int.Parse(dataGridView3.Rows[0].Cells[i].Value.ToString()) - 1;
                }
            }           
            if (Enumerable.SequenceEqual(GameLogic.pairs, answer))
            {
                if (GameLogic.lang == 0)
                {
                    MessageBox.Show("Correct!");
                }
                else
                {
                    MessageBox.Show("Верно!");
                }
            }
            else
            {
                string blocks = GameLogic.FindBlock(answer);
                if (blocks.Length > 0)
                {
                    dataGridView3.Columns[int.Parse(blocks[0].ToString())].HeaderCell.Style.ForeColor = Color.Red;
                    dataGridView3.Columns[int.Parse(blocks[1].ToString())].DefaultCellStyle.ForeColor = Color.Red;
                    label4.Visible = true;
                    label4.ForeColor = Color.Red;
                    if (GameLogic.lang == 0)
                    {
                        label4.Visible = true;
                        label4.Text = "blocking match";
                        label4.ForeColor = Color.Red;
                    }
                    else
                    {
                        label4.Text = "блокирующая пара";
                    }
                }
                if (GameLogic.lang == 0)
                {
                    MessageBox.Show("Wrong.");
                }
                else
                {
                    MessageBox.Show("Неверно.");
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label4.Visible = false;
            label4.Text = "";
            if (GameLogic.lang == 0)
            {
                button5.Text = "Download to desktop";
                label5.Text = "Difficulty: " + GameLogic.difficulty;
                label6.Text = "Variant: " + GameLogic.seed;
                label1.Text = "Men's preferences:";
                label2.Text = "Women's preferences:";
                button4.Text = "Save";
                button1.Text = "Solve";
                label3.Text = "Matching:";
                button3.Text = "Verify";
                button2.Text = "Show solution";
                m_label = "M.";
                w_label = "F.";
            }
            else
            {
                label5.Text = "Уровень сложности: " + GameLogic.difficulty;
                label6.Text = "Вариант: " + GameLogic.seed;
            }
            dataGridView3.EnableHeadersVisualStyles = false;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView3.CellValidating += CellValidating;
            dataGridView3.CellBeginEdit += DataGridView3_CellBeginEdit;
            if (GameLogic.mode == 1)
            {
                button4.Visible = true;
                button4.Enabled = true;
                button1.Enabled = false;
                button2.Enabled = false;
                dataGridView1.ReadOnly = false;
                dataGridView2.ReadOnly = false;
                dataGridView1.CellValidating += CellValidating;
                dataGridView2.CellValidating += CellValidating;
            }
            else
            {
                button4.Enabled = false;
                button4.Visible = false;
                dataGridView1.ReadOnly = true;
                dataGridView2.ReadOnly = true;
            }
            dataGridView3.ReadOnly = true;
            button3.Enabled = false;
            GameLogic.m_pref = new List<List<int>>(GameLogic.menCount);
            GameLogic.w_pref = new List<List<int>>(GameLogic.womenCount);

            if (GameLogic.mode == 0)
            {
                Random random = new Random(GameLogic.seed);
                GameLogic.Randomize(GameLogic.m_pref, GameLogic.womenCount, random);
                GameLogic.Randomize(GameLogic.w_pref, GameLogic.menCount, random);
            }

            for (int i = 0; i < GameLogic.womenCount; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), "№" + (i + 1).ToString());
            }
            for (int i = 0; i < GameLogic.menCount; i++)
            {
                dataGridView2.Columns.Add(i.ToString(), "№" + (i + 1).ToString());
                dataGridView3.Columns.Add(i.ToString(), m_label + (i + 1).ToString());
            }
            dataGridView3.Rows.Add();
            dataGridView3.Rows[0].HeaderCell.Value = w_label;
            dataGridView3.ClearSelection();
            sizeDGV(dataGridView3);
            dataGridView3.Location = new Point((this.Width - dataGridView3.Width) / 2 - 15, 420);

            for (int i = 0; i < GameLogic.menCount; i++)
            {
                if (GameLogic.mode == 0) {
                    string[] row = GameLogic.m_pref[i].Select(x => (x+1).ToString()).ToArray();
                    dataGridView1.Rows.Add(row); 
                }
                else { dataGridView1.Rows.Add(); };
                dataGridView1.Rows[i].HeaderCell.Value = m_label + (i + 1).ToString();
            }           
                          
            for (int i = 0; i < GameLogic.womenCount; i++)
            {
                if (GameLogic.mode == 0)
                {
                    string[] row = GameLogic.w_pref[i].Select(x => (x + 1).ToString()).ToArray();
                    dataGridView2.Rows.Add(row);
                }
                else { dataGridView2.Rows.Add(); };
                dataGridView2.Rows[i].HeaderCell.Value = w_label + (i + 1).ToString();
            }

            dataGridView1.ClearSelection();
            sizeDGV(dataGridView1);
            dataGridView1.Location = new Point(this.Width / 2 - dataGridView1.Width - 10 - 15, 72);
            dataGridView2.ClearSelection();
            sizeDGV(dataGridView2);
            dataGridView2.Location = new Point(this.Width / 2 + 10 - 15, 72);
            button4.Location = new Point((this.Width - button4.Width) / 2 - 15, Math.Max(dataGridView1.Location.Y + dataGridView1.Height, dataGridView2.Location.Y + dataGridView2.Height) + 15);
            label1.Location = new Point(dataGridView1.Location.X + dataGridView1.Width / 2 - label1.Width / 2, 51);
            label2.Location = new Point(dataGridView2.Location.X + dataGridView2.Width / 2 - label2.Width / 2, 51);
            label3.Location = new Point((this.Width - label3.Width) / 2 - 15, 391);
        }

        private void DataGridView3_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            label4.Visible = false;
            ColorCells();
        }

        public static void sizeDGV(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.Width = 45;
            }
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGridViewElementStates states = DataGridViewElementStates.None;
            var totalHeight = dgv.Rows.GetRowsHeight(states) + dgv.ColumnHeadersHeight;
            var totalWidth = dgv.Columns.GetColumnsWidth(states) + dgv.RowHeadersWidth;
            Size size = new Size(totalWidth, totalHeight + 2);
            dgv.ClientSize = size;
            dgv.ScrollBars = ScrollBars.None;

           // dgv.BorderStyle = BorderStyle.None;
           // dgv.BackgroundColor = SystemColors.Control;
            dgv.Update();
            dgv.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    List<int> temp = new List<int>();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value != null)
                        {
                            int val = int.Parse(row.Cells[i].Value.ToString());
                            if (val > GameLogic.womenCount)
                            {
                                throw new Exception();
                            }
                            temp.Add(val - 1);
                        }
                        if ((temp.Count != temp.Distinct().Count()) || (temp.Count == 0))
                        {
                            throw new Exception();
                        }
                    }
                    GameLogic.m_pref.Add(temp);
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    List<int> temp = new List<int>();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value != null)
                        {
                            int val = int.Parse(row.Cells[i].Value.ToString());
                            if (val > GameLogic.menCount)
                            {
                                throw new Exception();
                            }
                            temp.Add(val - 1);
                        }
                        if ((temp.Count != temp.Distinct().Count()) || (temp.Count == 0))
                        {
                            throw new Exception();
                        }                       
                    }
                    GameLogic.w_pref.Add(temp);
                }
                button4.Enabled = false;
                button1.Enabled = true;
                button2.Enabled = true;
            }
            catch (Exception)
            {
                GameLogic.m_pref.Clear();
                GameLogic.w_pref.Clear();
                if (GameLogic.lang == 0)
                {
                    MessageBox.Show("1) Duplicate or non-existing participants' IDs not allowed.\n2) Each participant's preferences must contain at least one ID.");
                }
                else
                {
                    MessageBox.Show("1) Недопустимо наличие повторяющихся номеров и номеров несуществующих участников.\n2) В преференциях каждого участника должен быть хотя бы один номер.");
                }
            }
        }

        private void CellValidating(object sender,
                                           DataGridViewCellValidatingEventArgs e)
        {
            if (!int.TryParse(Convert.ToString(e.FormattedValue), out _))
                {
                if (!(string.IsNullOrEmpty(Convert.ToString(e.FormattedValue))))
                {
                    e.Cancel = true;
                    if (GameLogic.lang == 0)
                    {
                        MessageBox.Show("Only digits allowed.");
                    }
                    else
                    {
                        MessageBox.Show("Допустимы только цифры.");
                    }
                }
            }
        }

        private void ColorCells()
        {
            foreach (DataGridViewColumn col in dataGridView3.Columns)
            {
                col.DefaultCellStyle.ForeColor = Color.Black;
                col.HeaderCell.Style.ForeColor = Color.Black;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WindowScreen(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), GameLogic.difficulty + GameLogic.seed.ToString() + ".jpeg", ImageFormat.Jpeg);
        }

        private void WindowScreen(String filepath, String filename, ImageFormat format)
        {
            Rectangle bounds = this.Bounds;

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                string fullpath = filepath + "\\" + filename;

                bitmap.Save(fullpath, format);
            }
        }
    } 
}
