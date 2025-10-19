using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;



namespace WindowsFormsApp1
{



    public partial class Form1 : Form
    {
        private bool isFirstClick = true;
        private bool isSecondClick = true;
        private bool isThridClick = true;
        private bool isFourthClick = true;
        private bool isFiveClick = true;


        private MySqlConnection connection;
        private string connections = "server=192.168.0.89; port=3306; username= _dpr2214;password= _dpr2214;database= _dpr2214_durka";

        public Form1()
        {
            InitializeComponent();
            this.AutoScroll = false;

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable durka = new DataTable();

            try
            {
                MySqlConnection connection = new MySqlConnection(connections);
                connection.Open();// Проверка

                string checkQuery = "SELECT COUNT(*) FROM bolnie WHERE NomerPalatbl = @NomerPalatbl";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);


                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите номер палаты.");
                    return;
                }
                checkCommand.Parameters.AddWithValue("@NomerPalatbl", comboBox1.SelectedItem.ToString());

                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Номер палаты занят. Пожалуйста, выберите другой номер.");
                    return;
                
                }
            string query = "INSERT INTO bolnie (Familiya, Imya, Vozrast, Otchestvo, NomerPalatbl) VALUES (@Familiya, @Imya, @Vozrast, @Otchestvo, @NomerPalatbl)";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Familiya", textBox1.Text);
                command.Parameters.AddWithValue("@Imya", textBox2.Text);
                command.Parameters.AddWithValue("@Vozrast", Convert.ToInt32(textBox4.Text));
                command.Parameters.AddWithValue("@Otchestvo", textBox3.Text);
                command.Parameters.AddWithValue("@NomerPalatbl", comboBox1.SelectedItem.ToString()); 
                command.ExecuteNonQuery();
                // Создаем новую Panel с номером палаты
                System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
                newButton.Text = "" + comboBox1.SelectedItem.ToString();
                newButton.Name = "button" + comboBox1.SelectedItem.ToString();
                newButton.BackColor = Color.Purple;
                newButton.Size = new Size(37, 40);
                


                // Устанавливаем корректные размеры и расположение для новой панели


                // Получаем имя из базы данных
                string name = string.Empty;
                string nameQuery = "SELECT Imya FROM bolnie WHERE NomerPalatbl = @NomerPalatbl";
                MySqlCommand nameCommand = new MySqlCommand(nameQuery, connection);
                nameCommand.Parameters.AddWithValue("@NomerPalatbl", comboBox1.SelectedItem.ToString());
                name = nameCommand.ExecuteScalar().ToString();

                Label label = new Label();
                label.Text = name;
                label.Dock = DockStyle.Fill;
                label.AutoSize = false;
                label.AutoEllipsis = false; 
                
                label.Size = new Size(37, 40);
               


                // Ищем панель с таким же номером и перемещаем новую панель к найденной
                Panel targetPanel = Controls.Find("panel" + comboBox1.SelectedItem.ToString(), true).FirstOrDefault() as Panel;

                if (targetPanel != null)
                {
                    targetPanel.Parent.Controls.Add(newButton);
                    newButton.Location = new Point(targetPanel.Left, targetPanel.Top - newButton.Height - -40); 
                    newButton.BringToFront();

                }

                MessageBox.Show("Данные успешно добавлены в базу данных.");
             }
            catch (Exception ex)
            {
               
                MessageBox.Show("\tПроверьте правильность введенных данных.  \n \n \nОшибка при добавлении данных в базу данных: \n" + ex.Message);
                
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            buttonnssadd();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = GetDataBl();
        }


        private DataTable GetDataBl()
        { 
        DataTable Bolnie = new DataTable();


            MySqlConnection connection = new MySqlConnection(connections);

            try
            {
                connection.Open();

                string query = "Select idBolnie,Familiya,Imya,Vozrast,Otchestvo from bolnie";
                MySqlCommand command = new MySqlCommand(query, connection)
                  ;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(Bolnie);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении данных из MySQL: " + ex.Message);

            }
            finally
            {
                    connection.Close();
            
            }
            return Bolnie;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            button2.Visible = false;


        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (isFirstClick)
            {
                textBox1.Text = string.Empty;
                isFirstClick = false;
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (isSecondClick)
            {
                textBox2.Text = string.Empty;
                isSecondClick = false;
            }
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            if (isThridClick)
            {
                textBox3.Text = string.Empty;
                isThridClick = false;
            }
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            if (isFourthClick)
            {
                textBox4.Text = string.Empty;
                isFourthClick = false;
            }
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connections);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT NomerPalatbl FROM bolnie", connection);
            MySqlDataReader reader = cmd.ExecuteReader();





            HashSet<string> existingNumbers = new HashSet<string>();
            while (reader.Read())
            {
                existingNumbers.Add(reader["NomerPalatbl"].ToString());
            }

            comboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox1.DrawItem += (s, d) =>
            {
                if (d.Index >= 0)
                {
                    string item = comboBox1.Items[d.Index].ToString();
                    if (existingNumbers.Contains(item))
                    {
                        d.Graphics.FillRectangle(Brushes.Red, d.Bounds);
                        d.Graphics.DrawString(item, comboBox1.Font, Brushes.Black, d.Bounds);
                    }
                    else
                    {
                        d.Graphics.FillRectangle(SystemBrushes.Window, d.Bounds);
                        d.Graphics.DrawString(item, comboBox1.Font, Brushes.Black, d.Bounds);
                    }
                }
            };
            foreach (Control control in this.Controls)
            {
                if (control is Panel)
                {
                    Panel panel = control as Panel;
                    panel.DragEnter += panel_DragEnter;
                    panel.DragDrop += panel_DragDrop;
                }
            }
            connection.Close();

           


        }


        private void buttonnssadd()
        {
            // Remove all existing buttons except for buttons 1, 2, 3, and 4
            foreach (Control control in this.Controls)
            {
                if (control is System.Windows.Forms.Button && control.Name.StartsWith("button") && !control.Name.Equals("button1") && !control.Name.Equals("button2") && !control.Name.Equals("button3") && !control.Name.Equals("button4"))
                {
                    this.Controls.Remove(control);
                }
            }

            // Re-add the occupied buttons
            MySqlConnection connection = new MySqlConnection(connections);
            string query = "SELECT NomerPalatbl, Imya FROM bolnie";
            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            List<System.Windows.Forms.Button> allButtons = new List<System.Windows.Forms.Button>();
            while (reader.Read())
            {
                string nomerPalatbl = reader["NomerPalatbl"].ToString();
                string imya = reader["Imya"].ToString();
                System.Windows.Forms.Button occupiedButton = new System.Windows.Forms.Button();
                occupiedButton.Name = "button" + nomerPalatbl;
                occupiedButton.BackColor = Color.Red;
                occupiedButton.Size = new Size(37, 40);
                Label nameLabel = new Label();
                nameLabel.Text = imya;
                nameLabel.Dock = DockStyle.Top;
                occupiedButton.Controls.Add(nameLabel);
                Label numberLabel = new Label();
                numberLabel.Text = nomerPalatbl;
                numberLabel.Dock = DockStyle.Fill;
                nameLabel.TextAlign = ContentAlignment.MiddleCenter;
                occupiedButton.Controls.Add(numberLabel);
                allButtons.Add(occupiedButton);
            }
            connection.Close();
            foreach (System.Windows.Forms.Button buttonToAdd in allButtons)
            {
                string buttonNumber = buttonToAdd.Name.Replace("button", "");
                Panel targetPanel = Controls.Find("panel" + buttonNumber, true).FirstOrDefault() as Panel;
                if (targetPanel != null && targetPanel.Controls.Count == 0)
                {
                    System.Windows.Forms.Button newButton = new System.Windows.Forms.Button();
                    newButton.Size = buttonToAdd.Size;
                    newButton.Text = buttonToAdd.Controls[0].Text + "\n" + buttonToAdd.Controls[1].Text;
                    newButton.BackColor = buttonToAdd.BackColor;
                    targetPanel.Controls.Add(newButton);
                    newButton.Location = new Point(0, 0);
                    newButton.MouseDown += Button_MouseDown;
                    newButton.MouseUp += Button_MouseUP;
                    newButton.MouseMove += Button_MouseMove;
                }
            }

            // Re-enable drag-and-drop for panels
            foreach (Control control in this.Controls)
            {
                if (control is Panel)
                {
                    Panel panel = control as Panel;
                    panel.DragEnter += panel_DragEnter;
                    panel.DragDrop += panel_DragDrop;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttonnssadd();
        }









        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;

                string panelName = button.Parent.Name.Replace("panel", "");

                // Обновляем текст кнопки на имя палаты
                button.Text = panelName;
            }
        }
        private void Button_MouseUP(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;

            if (button != null)
            {
                Panel panel = button.Parent as Panel;

                if (panel != null)
                {
                    string newRoomNumber = panel.Name.Replace("panel", "");
                    string oldRoomNumber = button.Text; // Старый номер палаты - текущий текст кнопки

                    if (newRoomNumber != oldRoomNumber)
                    {
                        MySqlConnection connection = new MySqlConnection(connections);

                        // Обновляем базу данных с новым номером палаты
                        string updateQuery = "UPDATE bolnie SET NomerPalatbl = @newRoomNumber WHERE NomerPalatbl = @oldRoomNumber";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@newRoomNumber", newRoomNumber);
                        updateCommand.Parameters.AddWithValue("@oldRoomNumber", oldRoomNumber);

                        try
                        {
                            connection.Open();
                            updateCommand.ExecuteNonQuery();
                            connection.Close();

                            // Обновляем текст кнопки на новый номер палаты
                            button.Text = newRoomNumber;
                        }
                        catch (Exception ex)
                        {
                            // Обработка ошибок при работе с базой данных
                            MessageBox.Show("Ошибка при обновлении данных в базе: " + ex.Message);
                        }
                    }
                }
            }
        }
        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Windows.Forms.Button button = sender as System.Windows.Forms.Button;
                button.DoDragDrop(button, DragDropEffects.Move);
            }



        }
        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            System.Windows.Forms.Button button = e.Data.GetData(typeof(System.Windows.Forms.Button)) as System.Windows.Forms.Button;
            Panel panel = sender as Panel;

            if (button != null && panel != null && panel.Controls.Count == 0)
            {
                if (button.Parent is Panel previousPanel)
                {
                    if (panel.Name != previousPanel.Name)
                    {
                        string newRoomNumber = panel.Name.Replace("panel", "");
                        string oldRoomNumber = button.Text;

                        MySqlConnection connection = new MySqlConnection(connections);
                        connection.Open();

                        string updateQuery = "UPDATE bolnie SET NomerPalatbl = @newRoomNumber WHERE NomerPalatbl = @oldRoomNumber";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@newRoomNumber", newRoomNumber);
                        updateCommand.Parameters.AddWithValue("@oldRoomNumber", oldRoomNumber);

                        updateCommand.ExecuteNonQuery();

                        connection.Close();

                        button.Size = panel.Size;
                        button.Location = new Point(0, 0);
                        panel.Controls.Add(button);
                        button.Text = newRoomNumber;
                        //
                        // По завершении перетаскивания обновляем текст кнопки на новый номер палаты
                        //
                    }
                }
            }

        }



        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(System.Windows.Forms.Button)))
            {
                Panel panel = sender as Panel;
                if (panel.Controls.Count == 0) // Проверяем, что на панели нет других элементов
                {
                    e.Effect = DragDropEffects.Move;

                    // Удаляем существующую кнопку с предыдущей панели, если она была
                    if (panel.Controls.Count > 0)
                    {
                        System.Windows.Forms.Button button = panel.Controls[0] as System.Windows.Forms.Button;
                        if (button != null)
                        {
                            button.Parent.Controls.Remove(button);
                        }
                    }
                }
            }
        }

























        private void Panel_MouseUP(object sender, MouseEventArgs e)
        {


        


        }


        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {





        }
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {








        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {









  
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        //НЕ РАБОТАЕТ НАДО ДОДЕЛАТЬ    ПЕРЕМЕЩЕНИЕ ДРАГ/ДРОП   и сделать вывод имеющихся уже строчек в базе данных

    }
}





