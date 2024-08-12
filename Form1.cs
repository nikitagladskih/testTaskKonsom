using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string[] linesLic;
        string nameCheckFile;
        string nameLic;
        
        string path = Path.Combine(Environment.CurrentDirectory, "Log.txt");
        
        public Form1()
        {
            try
            {
                InitializeComponent();
                listBox1.Click += new EventHandler(listBox1_CleClick);
                
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, "\n"+ex.Message);
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            try {
                if (nameCheckFile.Count() != 0)
                {
                    listBox2.Items.Clear();
                    label5.Text = "";
                    label6.Text = "";
                    string file = File.ReadAllText(nameCheckFile);
                    string error = fileBracketCheck(file);
                    if (error.Length != 0)
                    {
                        label5.Text = "Не пройдена";
                        listBox2.Items.Add(error);
                    }
                    else
                    {
                        label5.Text = "Пройдена успешно";
                    }
                    if (fileLiteralCheck(file))
                    {
                        label6.Text = "Не пройдена";                        
                    }
                    else
                    {
                        label6.Text = "Пройдена успешно";
                    }
                    
                }
                else
                {
                    listBox2.Items.Add("Не выбран файл");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, "\n" + ex.Message);

            }

        }

        private bool fileLiteralCheck(string str)
        {
            try
            {
                bool errors = false;
                Regex regex = new Regex(@"(\s@\s)|(\s'\s)|(\s""\s)|(\s\\\s)|(\s/\s)|(^@\s)|(^'\s)|(^""\s)|(^\\\s)|(^/\s)|(^@$)|(^'$)|(^""$)|(^\\$)|(^/$)|(\s@$)|(\s'$)|(\s""$)|(\s\\$)|(\s/$)");
                MatchCollection matches = regex.Matches(str);
                if (matches.Count > 0)
                {
                    errors = true;
                }
                return errors;
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, "\n" + ex.Message);
                bool errors = true;
                return errors;
            }

        }


        private string fileBracketCheck(string str)
        {
            try {
                string errors = "";
                int line = 1;
                var stack = new Stack<char>();
                var lineErrorStack = new Stack<int>();
                foreach (var c in str)
                {
                    char i = 'q';
                    switch (c)
                    {
                        case '\n':
                            line++;
                            break;
                        case '{':
                        case '(':
                        case '[':
                            stack.Push(c);
                            lineErrorStack.Push(line);
                            break;

                        case '}':
                            if (stack.Count == 0) { return errors = ("В стр. " + line + " незакрытая }"); };
                            i = stack.Pop();
                            if (i != '{') { return errors = ("В стр. " + lineErrorStack.Pop() + " незакрытая " + i); };
                            lineErrorStack.Pop();
                            break;
                        case ']':
                            if (stack.Count == 0) { return errors = ("В стр. " + line + " незакрытая ]"); };
                            i = stack.Pop();
                            if (i != '[') { return errors = ("В стр. " + lineErrorStack.Pop() + " незакрытая " + i); };
                            lineErrorStack.Pop();
                            break;
                        case ')':
                            if (stack.Count == 0) { return errors = ("В стр. " + line + " незакрытая )"); };
                            i = stack.Pop();
                            if (i != '(') { return errors = ("В стр. " + lineErrorStack.Pop() + " незакрытая " + i); };
                            lineErrorStack.Pop();
                            break;
                    }

                }
                if (stack.Count != 0) { return errors = ("В стр. " + lineErrorStack.Pop() + " незакрытая " + stack.Pop()); };
                return errors;
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, "\n" + ex.Message);
                string errors = "";
                return errors;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {//Добавление файла лицензии
            try {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    linesLic = File.ReadAllLines(openFileDialog.FileName); // читаем все строки из файла  
                    label9.Text = openFileDialog.SafeFileName;
                    nameLic = openFileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, "\n" + ex.Message);

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {//Добавление из файла лицензии в проверяемый файл
            try {
                if (nameLic.Count() != 0 && nameCheckFile.Count() != 0)
                {
                    File.WriteAllLines(nameCheckFile, linesLic.Concat(File.ReadAllLines(nameCheckFile)).ToArray());
                }
                else
                {
                    listBox2.Items.Add("Не выбран проверяемый файл или файл лицензии");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, ex.Message);

            }


        }

        private void button3_Click(object sender, EventArgs e)
        {//Добавление Файла
            try {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "txt files (*.cs)|*.cs|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    label5.Text = "";
                    label6.Text = "";
                    nameCheckFile = openFileDialog.FileName;
                    label4.Text = openFileDialog.FileName;
                    listBox1.Items.Add(nameCheckFile);

                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, "\n" + ex.Message);

            }

        }
        
        private void listBox1_CleClick(object sender, EventArgs e)
        {
            try {
                if (listBox1.SelectedItem != null)
                {
                    label5.Text = "";
                    label6.Text = "";
                    nameCheckFile = listBox1.SelectedItem.ToString();
                    label4.Text = listBox1.SelectedItem.ToString();
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, "\n" + ex.Message);

            }

        }
    }
}
