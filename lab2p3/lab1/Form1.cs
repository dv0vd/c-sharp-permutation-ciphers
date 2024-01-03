using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private const string inputFile = "Входной файл...";
        private string inputFileName;
        private char[] mas = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ь', 'Ы', 'Э', 'Ю', 'Я', 'а','б','в','г','д','е','ё','ж','з','и','й','к','л','м','н','о','п','р','с','т','у','ф','х','ц','ч','ш','щ','ъ','ь','ы','э','ю','я',' ',',','.'};
        private string key;
        private string[] transpositionsMas;
        byte[] b;
        string[] a;
        private int keyIndex;
        bool firstTime = true;
        int pos = 0;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            EnableFile();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DisableFile();
        }

        private void DisableFile()
        {
            label1.Enabled = false;
            button1.Enabled = false;
            textBox4.ReadOnly = false;
        }

        private void EnableFile()
        {
            label1.Enabled = true;
            textBox4.ReadOnly = true;
            inputFileName = "";
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ураааа!\nВаш ключ: " + key);
            Finish();
        }

        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true ;
            EnableFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Finish();
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Файлы |*.txt";
            OPF.Title = "Выбрать файл";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                inputFileName = OPF.FileName;
                label1.Text = inputFileName;
                textBox4.Text = "";
                if(!CheckingFile())
                {
                    using (StreamReader fin = new StreamReader(inputFileName))
                    {
                        string strTemp = fin.ReadLine();
                        while (strTemp != null)
                        {
                            strTemp += "\r\n";
                            textBox4.Text += strTemp;
                            strTemp = fin.ReadLine();
                        }
                        fin.Close();
                    }
                }
            }
        }

        private bool CheckAlphabet(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                bool check = false;
                for (int j = 0; j < mas.Length; j++)
                {
                    if ((str[i] == mas[j]) || (str[i] == '!'))
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                    return false;
            }
            return true;
        }

        private bool CheckingFile()
        {
            bool check = false;
            using (StreamReader fin = new StreamReader(inputFileName))
            {
                string str;
                while ((str = fin.ReadLine()) != null)
                {
                    str = str.ToLower();
                    if (!CheckAlphabet(str))
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                {
                    MessageBox.Show("В файле присутствуют недопустимые символы!");
                    fin.Close();
                    return true;
                }
                fin.Close();
            }
            return false;
        }

        private void Finish()
        {
            label1.Text = inputFile;
            textBox4.Text = "";
            inputFileName = "";
            textBox5.Text = "";
            firstTime = true;
            numericUpDown1.ReadOnly = false;
            pos = 0;
            key = "";
            keyIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Finish();
        }


        private void per(byte k)
        {
            for(int i=0; i<numericUpDown1.Value; i++)
            {
                if(b[i] == 0)
                {
                    key += a[i];
                    b[i] = 1;
                    if (k != 1)
                        per((byte)(k-1));
                    else
                    {
                        transpositionsMas[pos] = key;
                        pos++;
                    }
                    b[i] = 0;
                    key = key.Substring(0, key.Length - 1);
                }
            }
        }

        private void Decoding(string key, string[]s)
        {
            textBox5.Text = "";
            for (int k = 0; k < s.Length; k++)
            {

                int size = (int)numericUpDown1.Value;

                string strTemp = s[k];
                decimal groupsCount = Math.Ceiling(strTemp.Length / (decimal)size);
                string[] table = new string[(int)groupsCount];
                for (int i = 0; i < groupsCount; i++)
                    table[i] = "";
                for (int i = 0; i < size * (int)groupsCount; i += (int)groupsCount)
                {
                    string temp = "";
                    for (int j = i; j < groupsCount + i; j++)
                    {
                        if (j < strTemp.Length)
                            temp += strTemp[j];
                        else
                            break;
                    }
                    for (int j = 0; j < groupsCount; j++)
                    {
                        if (j < temp.Length)
                            table[j] += temp[j];
                        else
                            break;
                    }
                }
                char[] group = new char[(int)(groupsCount * size)];
                for (int i = 0; i < group.Length; i++)
                    group[i] = '\0';
                for (decimal i = 0; i < groupsCount; i++)
                {
                    strTemp = table[(int)i];
                    string groupTemp = "";
                    for (int j = 0; j < size; j++)
                    {
                        if ((j) >= strTemp.Length)
                            break;
                        groupTemp += strTemp[j];
                    }
                    for (int j = 0; j < size; j++)
                    {
                        int poss = key.ToString()[j] - '0';
                        if (j < groupTemp.Length)
                        {
                            if (groupTemp[j] != '!')
                                group[(int)i * (int)size + (poss - 1)] = groupTemp[j];
                        }
                        else
                            break;
                    }
                }
                string str1 = new string(group);
                string str = "";
                for (int i = 0; i < str1.Length; i++)
                    if (str1[i] != '\0')
                        str += str1[i];
                if (k != s.Length - 1)
                    str += "\r\n";
                textBox5.Text = textBox5.Text + str;
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked && (inputFileName == ""))
            {
                MessageBox.Show("Ошибка! Выберите файл!");
            }
            else
            {
                String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                numericUpDown1.ReadOnly = true;
                if (firstTime)
                {
                    firstTime = false;
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            firstTime = true;
                            return;
                        }
                    }
                    int factorial = 1;  
                    for (int i = 2; i <= numericUpDown1.Value; i++)
                    {
                        factorial = factorial * i;
                    }
                    transpositionsMas = new string[factorial];
                    b = new byte[(int)numericUpDown1.Value];
                    a = new string[(int)numericUpDown1.Value];
                    for (int i = 0; i < a.Length; i++)
                        a[i] = (i + 1).ToString();
                    per((byte)numericUpDown1.Value);
                    keyIndex = 0;
                    key = transpositionsMas[keyIndex];
                    Decoding(key,s);
                }
                else
                {
                    if(keyIndex == (transpositionsMas.Length-1))
                    {
                        MessageBox.Show("К сожалению все варианты ключей были перепробованы(\nПопробуйте изменить длину ключа");
                        firstTime = true;
                        pos = 0;
                        key = "";
                        numericUpDown1.ReadOnly = false;
                        keyIndex = 0;
                    }
                    else
                    {
                        keyIndex++;
                        key = transpositionsMas[keyIndex];
                        Decoding(key,s);
                    }
                }
            }
        }
    }
}
