using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private const string inputFile = "Входной файл...", outputFile = "Каталог сохранения выходных файлов...";
        private string inputFileName, outputDirectory;
        private char[] mas = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ь', 'Ы', 'Э', 'Ю', 'Я', 'а','б','в','г','д','е','ё','ж','з','и','й','к','л','м','н','о','п','р','с','т','у','ф','х','ц','ч','ш','щ','ъ','ь','ы','э','ю','я',' ',',','.'};
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            EnableFile();
            DisableText();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            EnableText();
            DisableFile();
        }

        private void DisableFile()
        {
            label1.Enabled = false;
            label2.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void EnableFile()
        {
            label1.Enabled = true;
            label2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void DisableText()
        {
            label8.Enabled = false;
            label9.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void EnableText()
        {
            label8.Enabled = true;
            label9.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
        }

        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true ;
            DisableText();
            EnableFile();
            label5.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Файлы |*.txt";
            OPF.Title = "Выбрать файл";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                inputFileName = OPF.FileName;
                label1.Text = inputFileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                outputDirectory = FBD.SelectedPath;
                FBD.Description = "Выбрать директорию";
                label2.Text = outputDirectory;
            }
        }

        private void CopyFile(string str, string path)
        {
            using (StreamReader fin = new StreamReader(str))
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                {
                    string str2= fin.ReadLine();
                    while(str2 != null)
                    {
                        str = str2;
                        str2 = fin.ReadLine();
                        if(str2 != null)
                            fout.WriteLine(str);
                        else
                            fout.Write(str);
                    }
                    fout.Close();
                }
                fin.Close();
                fs.Close();
            }
        }

        private int FindSymbolIndex(char sym)
        {
            int i = 0;
            for(; i<mas.Length; i++)
            {
                if (sym == mas[i])
                    break;
            }   
            return i;
        }

        private bool CheckAlphabet(string str)
        {
            for(int i=0; i< str.Length; i++)
            {
                bool check = false;
                for(int j=0; j<mas.Length; j++)
                {
                    if((str[i] == mas[j])|| (str[i] == '!'))
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
            label2.Text = outputFile;
            textBox2.Text = "";
            textBox1.Text = "";
            textBox3.Text = "";
            MessageBox.Show("Успех!");
        }

        private void FenceCoding_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            textBox5.Text = "";
            if (radioButton1.Checked)
            {
                if ((label1.Text == inputFile) || (label2.Text == outputFile))
                {
                    MessageBox.Show("Не выбраны пути к файлам!");
                }
                else
                {
                    if (!CheckingFile())
                    {
                        string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                        CopyFile(inputFileName, path);
                        path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - ШИФР ИЗГОРОДИ.txt";
                        using (StreamReader fin = new StreamReader(inputFileName))
                        {
                            FileStream fs = new FileStream(path, FileMode.Create);
                            using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                            {
                                string str, strTemp=fin.ReadLine();
                                while (strTemp != null)
                                {
                                    str = strTemp;
                                    symbolsCount += str.Length;
                                    strTemp = fin.ReadLine();
                                    string str1 = "", str2 = "";
                                    for (int i = 0; i < str.Length; i += 2)
                                    {
                                        str1 += str[i];
                                        if ((i + 1) < str.Length)
                                            str2 += str[i + 1];
                                    }
                                    string strCoded = str1 + str2;
                                    if (strTemp != null)
                                        fout.WriteLine(strCoded);
                                    else
                                        fout.Write(strCoded);
                                    str1 = "";
                                    str2 = "";
                                }
                                fout.Close();
                            }
                            fin.Close();
                        }
                        label5.Text = symbolsCount.ToString();
                        Finish();
                    }
                }
            }
            else
            {
                String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < s.Length; i++)
                {
                    if (!CheckAlphabet(s[i]))
                    {
                        MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                        return;
                    }
                }
                string str1="",str2="";
                for(int k = 0; k<s.Length; k++)
                {
                    string strTemp = s[k];
                    symbolsCount += strTemp.Length;
                    for (int i = 0; i < strTemp.Length; i += 2)
                    {
                        str1 += strTemp[i];
                        if ((i+1) < strTemp.Length)
                            str2 += strTemp[i + 1];
                    }
                    string strCoded = str1 + str2;
                    if (k != s.Length - 1)
                        strCoded += "\r\n";
                    textBox5.Text = textBox5.Text + strCoded;
                    str1 = "";
                    str2 = "";
                }
                label5.Text = symbolsCount.ToString();
                Finish();
            }
        }

        private void FenceDecoding_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            textBox5.Text = "";
            if (radioButton1.Checked)
            {
                if ((label1.Text == inputFile) || (label2.Text == outputFile))
                {
                    MessageBox.Show("Не выбраны пути к файлам!");
                }
                else
                {
                    if (!CheckingFile())
                    {
                        string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                        CopyFile(inputFileName, path);
                        path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - ШИФР ИЗГОРОДИ.txt";
                        using (StreamReader fin = new StreamReader(inputFileName))
                        {
                            FileStream fs = new FileStream(path, FileMode.Create);
                            using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                            {
                                string str, strTemp = fin.ReadLine();
                                while (strTemp != null)
                                {
                                    str = strTemp;
                                    symbolsCount += str.Length;
                                    strTemp = fin.ReadLine();
                                    string str1 = "", str2 = "";
                                    if(str=="")
                                    {
                                        if (strTemp != null)
                                            fout.WriteLine();
                                        else
                                            fout.Write("");
                                    }
                                    else
                                    {
                                        int index = (int)Math.Ceiling((float)str.Length / (float)2);
                                        for (int i = 0; i < index; i++)
                                        {
                                            str1 += str[i];
                                        }
                                        for (int i = index; i < str.Length; i++)
                                        {
                                            str2 += str[i];
                                        }
                                        string strDecoded = "";
                                        for (int i = 0; i < str1.Length; i++)
                                        {
                                            strDecoded = strDecoded + str1[i];
                                            if (i < str2.Length)
                                                strDecoded = strDecoded + str2[i];
                                        }
                                        if (strTemp != null)
                                            fout.WriteLine(strDecoded);
                                        else
                                            fout.Write(strDecoded);
                                        str1 = "";
                                        str2 = "";
                                    }
                                }
                                fout.Close();
                            }
                            fin.Close();
                        }
                        label5.Text = symbolsCount.ToString();
                        Finish();
                    }
                }
            }
            else
            {
                String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < s.Length; i++)
                {
                    if (!CheckAlphabet(s[i]))
                    {
                        MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                        return;
                    }
                }
                string str1 = "", str2 = "";
                for (int k = 0; k < s.Length; k++)
                {
                    string strTemp = s[k];
                    symbolsCount += strTemp.Length;
                    int index = (int)Math.Ceiling((float)strTemp.Length / (float)2);
                    for(int i=0; i<index; i++)
                    {
                        str1 += strTemp[i];
                    }
                    for (int i = index; i < strTemp.Length; i++)
                    {
                        str2 += strTemp[i];
                    }
                    string strDecoded = "";
                    for(int i=0; i<str1.Length; i++)
                    {
                        strDecoded = strDecoded + str1[i];
                        if(i<str2.Length)
                            strDecoded = strDecoded + str2[i];
                    }
                    if (k != s.Length - 1)
                        strDecoded += "\r\n";
                    textBox5.Text = textBox5.Text + strDecoded;
                    str1 = "";
                    str2 = "";
                }
                label5.Text = symbolsCount.ToString();
                Finish();
            }
        }

        private void ClueCipherDecode_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            decimal size = numericUpDown1.Value;
            if (KeyChecking(size, textBox1.Text))
            {
                textBox5.Text = "";
                decimal code = Int32.Parse(textBox1.Text);

                if (radioButton1.Checked)
                {
                    if ((label1.Text == inputFile) || (label2.Text == outputFile))
                    {
                        MessageBox.Show("Не выбраны пути к файлам!");
                    }
                    else
                    {
                        if (!CheckingFile())
                        {
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - КЛЮЧЕВОЙ ШИФР.txt";
                            using (StreamReader fin = new StreamReader(inputFileName))
                            {
                                FileStream fs = new FileStream(path, FileMode.Create);
                                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    string str, strTemp = fin.ReadLine();
                                    while (strTemp != null)
                                    {
                                        str = strTemp;
                                        symbolsCount += str.Length;
                                        strTemp = fin.ReadLine();
                                        decimal groupsCount = Math.Ceiling(str.Length / size);
                                        char[] group = new char[(int)(groupsCount * size)];
                                        for (int i = 0; i < group.Length; i++)
                                            group[i] = '\0';
                                        decimal index = 0;
                                        for (decimal i = 0; i < groupsCount; i++)
                                        {
                                            string groupTemp = "";
                                            for (int j = 0; j < size; j++)
                                            {
                                                if ((j + (int)index) >= str.Length)
                                                    break;
                                                groupTemp += str[j + (int)index];
                                            }
                                            for (int j = 0; j < size; j++)
                                            {
                                                int pos = code.ToString()[j] - '0';
                                                if (j < groupTemp.Length)
                                                {
                                                    if (groupTemp[j] != '!')
                                                        group[(int)index + pos - 1] = groupTemp[j];
                                                }
                                                else
                                                    break;
                                            }
                                            index += size;
                                        }
                                        string str1 = new string(group);
                                        string strOut = "";
                                        for (int i = 0; i < str1.Length; i++)
                                            if (str1[i] != '\0')
                                                strOut += str1[i];

                                        if (strTemp != null)
                                            fout.WriteLine(strOut);
                                        else
                                            fout.Write(strOut);
                                    }
                                    fout.Close();
                                }
                                fin.Close();
                            }
                            label5.Text = symbolsCount.ToString();
                            Finish();
                        }
                    }
                }
                else
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    for (int k = 0; k < s.Length; k++)
                    {
                        string strTemp = s[k];
                        symbolsCount += strTemp.Length;
                        decimal groupsCount = Math.Ceiling(strTemp.Length / size);
                        char[] group = new char[(int)(groupsCount*size)];
                        for (int i = 0; i < group.Length; i++)
                            group[i] = '\0';
                        decimal index = 0;
                        for (decimal i = 0; i < groupsCount; i++)
                        {
                            string groupTemp = "";
                            for (int j = 0; j < size; j++)
                            {
                                if ((j + (int)index) >= strTemp.Length)
                                    break;
                                groupTemp += strTemp[j + (int)index];
                            }
                            for (int j = 0; j < size; j++)
                            {
                                int pos = code.ToString()[j] - '0';
                                if (j < groupTemp.Length)
                                {
                                    if (groupTemp[j] != '!')
                                        group[(int)index + pos - 1] = groupTemp[j];
                                }
                                else
                                    break;
                            }
                            index += size;
                        }
                        string str1 = new string(group);
                        string str="";
                        for (int i = 0; i < str1.Length; i++)
                            if (str1[i] != '\0')
                                str += str1[i];
                        if (k != s.Length - 1)
                            str += "\r\n";
                        textBox5.Text = textBox5.Text + str;
                    }
                    label5.Text = symbolsCount.ToString();
                    Finish();
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте правильность ввода ключа!");
            }
        }

        private bool KeyChecking(decimal size, string codeString)
        {
            decimal code;
            try
            {
                code = Int32.Parse(codeString);
            }
            catch
            {
                return false;
            }
            int[] numbers = new int[(int)size];
            int i = 0;
            while (code >= 10)
            {
                if(numbers.Length<=i)
                    return false;
                numbers[i] = (int)code % 10;
                code = code / 10;
                i++;
            }
            if (i != (size - 1))
            {
                return false;
            }
            numbers[i] = (int)code;
            for (int j = 0; j < numbers.Length; j++)
            {
                if (numbers[j] == 0)
                    return false;
            }
                int max = numbers[0];
            for(int j=1; j<numbers.Length; j++)
            {
                if (numbers[j] > max)
                    max = numbers[i];
            }
            if (max > size)
                return false;
            for(int j=0; j<numbers.Length; j++)
            {
                int value = numbers[j];
                for(int k=j+1; k<numbers.Length; k++)
                {
                    if (value == numbers[k])
                        return false;
                }
            }
            return true;
        }

        private void ClueCipherCode_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            decimal size = numericUpDown1.Value;
            if(KeyChecking(size, textBox1.Text))
            {
                decimal code = Int32.Parse(textBox1.Text);
                textBox5.Text = "";
                if (radioButton1.Checked)
                {
                    if ((label1.Text == inputFile) || (label2.Text == outputFile))
                    {
                        MessageBox.Show("Не выбраны пути к файлам!");
                    }
                    else
                    {
                        if (!CheckingFile())
                        {
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - КЛЮЧЕВОЙ ШИФР.txt";
                            using (StreamReader fin = new StreamReader(inputFileName))
                            {
                                FileStream fs = new FileStream(path, FileMode.Create);
                                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    string str, strTemp = fin.ReadLine();
                                    while (strTemp != null)
                                    {
                                        str = strTemp;
                                        symbolsCount += str.Length;
                                        strTemp = fin.ReadLine();
                                        decimal groupsCount = Math.Ceiling(str.Length / size);
                                        string group = "";
                                        decimal index = 0;
                                        for (decimal i = 0; i < groupsCount; i++)
                                        {
                                            string groupTemp = "";
                                            for (int j = 0; j < size; j++)
                                            {
                                                if ((j + (int)index) >= str.Length)
                                                    break;
                                                groupTemp += str[j + (int)index];
                                            }
                                            index += size;
                                            for (int j = 0; j < size; j++)
                                            {
                                                int pos = code.ToString()[j] - '0';
                                                if (pos > groupTemp.Length)
                                                    group += "!";
                                                else
                                                {
                                                    group += groupTemp[pos - 1];
                                                }
                                            }
                                        }
                                        if (strTemp != null)
                                            fout.WriteLine(group);
                                        else
                                            fout.Write(group);
                                    }
                                    fout.Close();
                                }
                                fin.Close();
                            }
                            label5.Text = symbolsCount.ToString();
                            Finish();
                        }
                    }
                }
                else
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    for (int k = 0; k < s.Length; k++)
                    {
                        string strTemp = s[k];
                        symbolsCount += strTemp.Length;
                        decimal groupsCount = Math.Ceiling(strTemp.Length/size);
                        string  group = "";
                        decimal index = 0;
                        for (decimal i=0; i<groupsCount; i++)
                        {
                            string groupTemp = "";
                            for(int j=0; j<size; j++)
                            {
                                if ((j + (int)index) >= strTemp.Length)
                                    break;
                                groupTemp += strTemp[j + (int)index];
                            }
                            index += size;
                            for (int j = 0; j < size; j++)
                            {
                                int pos = code.ToString()[j] - '0';
                                if (pos > groupTemp.Length)
                                    group += "!";
                                else
                                {
                                    group += groupTemp[ pos - 1];
                                }
                            }
                        }
                        if (k != s.Length - 1)
                            group += "\r\n";
                        textBox5.Text = textBox5.Text + group;
                    }
                    label5.Text = symbolsCount.ToString();
                    Finish();
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте правильность ввода ключа!");
            }
        }

        private void CombineCipherCode_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            decimal size = numericUpDown2.Value;
            if (KeyChecking(size, textBox2.Text))
            {
                decimal code = Int32.Parse(textBox2.Text);
                textBox5.Text = "";
                if (radioButton1.Checked)
                {
                    if ((label1.Text == inputFile) || (label2.Text == outputFile))
                    {
                        MessageBox.Show("Не выбраны пути к файлам!");
                    }
                    else
                    {
                        if (!CheckingFile())
                        {
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - КОМБИНИРОВАННЫЙ ШИФР.txt";
                            using (StreamReader fin = new StreamReader(inputFileName))
                            {
                                FileStream fs = new FileStream(path, FileMode.Create);
                                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    string str, strTemp = fin.ReadLine();
                                    while (strTemp != null)
                                    {
                                        str = strTemp;
                                        symbolsCount += str.Length;
                                        strTemp = fin.ReadLine();
                                        decimal groupsCount = Math.Ceiling(str.Length / size);
                                        string group = "";
                                        decimal index = 0;
                                        string[] table = new string[(int)groupsCount];
                                        for (int i = 0; i < groupsCount; i++)
                                            table[i] = "";
                                        for (decimal i = 0; i < groupsCount; i++)
                                        {
                                            string groupTemp = "";
                                            for (int j = 0; j < size; j++)
                                            {
                                                if ((j + (int)index) >= str.Length)
                                                    break;
                                                groupTemp += str[j + (int)index];
                                            }
                                            index += size;
                                            for (int j = 0; j < size; j++)
                                            {
                                                int pos = code.ToString()[j] - '0';
                                                if (pos > groupTemp.Length)
                                                    table[(int)i] += "!";
                                                else
                                                {
                                                    table[(int)i] += groupTemp[pos - 1];
                                                }
                                            }
                                        }
                                        for (int i = 0; i < size; i++)
                                        {
                                            for (int j = 0; j < groupsCount; j++)
                                            {
                                                group += (table[j])[i];
                                            }
                                        }
                                        if (strTemp != null)
                                            fout.WriteLine(group);
                                        else
                                            fout.Write(group);
                                    }
                                    fout.Close();
                                }
                                fin.Close();
                            }
                            label5.Text = symbolsCount.ToString();
                            Finish();
                        }
                    }
                }
                else
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    for (int k = 0; k < s.Length; k++)
                    {
                        string strTemp = s[k];
                        symbolsCount += strTemp.Length;
                        decimal groupsCount = Math.Ceiling(strTemp.Length / size);
                        string group = "";
                        decimal index = 0;
                        string[] table = new string[(int)groupsCount];
                        for (int i = 0; i < groupsCount; i++)
                            table[i] = "";

                        for (decimal i = 0; i < groupsCount; i++)
                        {
                            string groupTemp = "";
                            for (int j = 0; j < size; j++)
                            {
                                if ((j + (int)index) >= strTemp.Length)
                                    break;
                                groupTemp += strTemp[j + (int)index];
                            }
                            index += size;
                            for (int j = 0; j < size; j++)
                            {
                                int pos = code.ToString()[j] - '0';
                                if (pos > groupTemp.Length)
                                    table[(int)i] += "!";
                                else
                                {
                                    table[(int)i] += groupTemp[pos - 1];
                                }
                            }
                        }
                        for(int i=0; i<size; i++)
                        {
                            for (int j = 0; j < groupsCount; j++)
                            {
                                group += (table[j])[i];
                            } 
                        }
                        if (k != s.Length - 1)
                            group += "\r\n";
                        textBox5.Text = textBox5.Text + group;
                    }
                    label5.Text = symbolsCount.ToString();
                    Finish();
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте правильность ввода ключа!");
            }
        }

        private void CombineCipherDecode_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            decimal size = numericUpDown2.Value;
            if (KeyChecking(size, textBox2.Text))
            {
                textBox5.Text = "";
                decimal code = Int32.Parse(textBox2.Text);
                if (radioButton1.Checked)
                {
                    if ((label1.Text == inputFile) || (label2.Text == outputFile))
                    {
                        MessageBox.Show("Не выбраны пути к файлам!");
                    }
                    else
                    {
                        if (!CheckingFile())
                        {
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - КОМБИНИРОВАННЫЙ ШИФР.txt";
                            using (StreamReader fin = new StreamReader(inputFileName))
                            {
                                FileStream fs = new FileStream(path, FileMode.Create);
                                using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    string str, strTemp = fin.ReadLine();
                                    while (strTemp != null)
                                    {
                                        str = strTemp;
                                        symbolsCount += str.Length;
                                        strTemp = fin.ReadLine();
                                        decimal groupsCount = Math.Ceiling(str.Length / size);
                                        string[] table = new string[(int)groupsCount];
                                        for (int i = 0; i < groupsCount; i++)
                                            table[i] = "";
                                        for (int i = 0; i < size * (int)groupsCount; i += (int)groupsCount)
                                        {
                                            string temp = "";
                                            for (int j = i; j < groupsCount + i; j++)
                                            {
                                                if (j < str.Length)
                                                    temp += str[j];
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
                                            str = table[(int)i];
                                            string groupTemp = "";
                                            for (int j = 0; j < size; j++)
                                            {
                                                if ((j) >= str.Length)
                                                    break;
                                                groupTemp += str[j];
                                            }
                                            for (int j = 0; j < size; j++)
                                            {
                                                int pos = code.ToString()[j] - '0';
                                                if (j < groupTemp.Length)
                                                {
                                                    if (groupTemp[j] != '!')
                                                        group[(int)i * (int)size + (pos - 1)] = groupTemp[j];
                                                }
                                                else
                                                    break;
                                            }
                                        }
                                        string str1 = new string(group);
                                        string strOut = "";
                                        for (int i = 0; i < str1.Length; i++)
                                            if (str1[i] != '\0')
                                                strOut += str1[i];
                                        if (strTemp != null)
                                            fout.WriteLine(strOut);
                                        else
                                            fout.Write(strOut);
                                    }
                                    fout.Close();
                                }
                                fin.Close();
                            }
                            label5.Text = symbolsCount.ToString();
                            Finish();
                        }
                    }
                }
                else
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    for (int k = 0; k < s.Length; k++)
                    {
                        string strTemp = s[k];
                        symbolsCount += strTemp.Length;
                        decimal groupsCount = Math.Ceiling(strTemp.Length / size);
                        string[] table = new string[(int)groupsCount];
                        for (int i = 0; i < groupsCount; i++)
                            table[i] = "";
                        for(int i=0; i<size* (int)groupsCount; i+=(int)groupsCount)
                        {
                            string temp = "";
                            for (int j=i; j<groupsCount+i; j++)
                            {
                                if (j < strTemp.Length)
                                    temp += strTemp[j];
                                else
                                    break;
                            }
                            for (int j = 0; j < groupsCount;j++)
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
                                int pos = code.ToString()[j] - '0';
                                if (j < groupTemp.Length)
                                {
                                    if (groupTemp[j] != '!')
                                        group[(int)i*(int)size + (pos - 1)] = groupTemp[j];
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
                    label5.Text = symbolsCount.ToString();
                    Finish();
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте правильность ввода ключа!");
            }
        }

        private void DoubleCipherCode_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            decimal size = numericUpDown3.Value;
            if (KeyChecking(size, textBox3.Text))
            {
                decimal code = Int32.Parse(textBox3.Text);
                textBox5.Text = "";
                if (radioButton1.Checked)
                {
                    if ((label1.Text == inputFile) || (label2.Text == outputFile))
                    {
                        MessageBox.Show("Не выбраны пути к файлам!");
                    }
                    else
                    {
                        if (!CheckingFile())
                        {
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - ШИФР С ДВОЙНОЙ ПЕРЕСТАНОВКОЙ.txt";
                            for (int f = 0; f < 2; f++)
                            {
                                string inputFile;
                                if (f == 0)
                                    inputFile = inputFileName;
                                else
                                    inputFile = "temp.txt";
                                using (StreamReader fin = new StreamReader(inputFile))
                                {
                                    string outputFile;
                                    if (f == 0)
                                        outputFile = "temp.txt";
                                    else
                                        outputFile = path;
                                    FileStream fs = new FileStream(outputFile, FileMode.Create);
                                    using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                    {
                                        string str, strTemp = fin.ReadLine();
                                        while (strTemp != null)
                                        {
                                            str = strTemp;
                                            strTemp = fin.ReadLine();
                                            if(f==0)
                                                symbolsCount += str.Length;
                                            decimal groupsCount = Math.Ceiling(str.Length / size);
                                            string group = "";
                                            decimal index = 0;
                                            string[] table = new string[(int)groupsCount];
                                            for (int i = 0; i < groupsCount; i++)
                                                table[i] = "";
                                            for (decimal i = 0; i < groupsCount; i++)
                                            {
                                                string groupTemp = "";
                                                for (int j = 0; j < size; j++)
                                                {
                                                    if ((j + (int)index) >= str.Length)
                                                        break;
                                                    groupTemp += str[j + (int)index];
                                                }
                                                index += size;
                                                for (int j = 0; j < size; j++)
                                                {
                                                    int pos = code.ToString()[j] - '0';
                                                    if (pos > groupTemp.Length)
                                                        table[(int)i] += "!";
                                                    else
                                                    {
                                                        table[(int)i] += groupTemp[pos - 1];
                                                    }
                                                }
                                            }
                                            for (int i = 0; i < size; i++)
                                            {
                                                for (int j = 0; j < groupsCount; j++)
                                                {
                                                    group += (table[j])[i];
                                                }
                                            }
                                            if (strTemp != null)
                                                fout.WriteLine(group);
                                            else
                                                fout.Write(group);
                                        }
                                        fout.Close();
                                    }
                                    fin.Close();
                                }
                               
                            }
                            label5.Text = symbolsCount.ToString();
                            Finish();
                            System.IO.File.Delete("temp.txt");
                        }
                    }
                }
                else
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    for (int f = 0; f < 2; f++)
                    {
                        for (int k = 0; k < s.Length; k++)
                        {
                            string strTemp = s[k];
                            if(f == 0)
                                symbolsCount += strTemp.Length;
                            decimal groupsCount = Math.Ceiling(strTemp.Length / size);
                            string group = "";
                            decimal index = 0;
                            string[] table = new string[(int)groupsCount];
                            for (int i = 0; i < groupsCount; i++)
                                table[i] = "";
                            for (decimal i = 0; i < groupsCount; i++)
                            {
                                string groupTemp = "";
                                for (int j = 0; j < size; j++)
                                {
                                    if ((j + (int)index) >= strTemp.Length)
                                        break;
                                    groupTemp += strTemp[j + (int)index];
                                }
                                index += size;
                                for (int j = 0; j < size; j++)
                                {
                                    int pos = code.ToString()[j] - '0';
                                    if (pos > groupTemp.Length)
                                        table[(int)i] += "!";
                                    else
                                    {
                                        table[(int)i] += groupTemp[pos - 1];
                                    }
                                }
                            }
                            for (int i = 0; i < size; i++)
                            {
                                for (int j = 0; j < groupsCount; j++)
                                {
                                    group += (table[j])[i];
                                }
                            }
                            s[k] = group;
                        }
                    }
                    for (int k = 0; k < s.Length; k++)
                    {
                        string str = s[k];
                        if (k != s.Length - 1)
                            str += "\r\n";
                        textBox5.Text = textBox5.Text + str;
                    }
                    label5.Text = symbolsCount.ToString();
                    Finish();
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте правильность ввода ключа!");
            }
        }

        private void DoubleCipherDecode_Click(object sender, EventArgs e)
        {
            int symbolsCount = 0;
            decimal size = numericUpDown3.Value;
            if (KeyChecking(size, textBox3.Text))
            {
                textBox5.Text = "";
                decimal code = Int32.Parse(textBox3.Text);
                if (radioButton1.Checked)
                {
                    if ((label1.Text == inputFile) || (label2.Text == outputFile))
                    {
                        MessageBox.Show("Не выбраны пути к файлам!");
                    }
                    else
                    {
                        if (!CheckingFile())
                        {
                            string path = outputDirectory + "\\ИСХОДНЫЙ ФАЙЛ.txt";
                            CopyFile(inputFileName, path);
                            path = outputDirectory + "\\РЕЗУЛЬТИРУЮЩИЙ ФАЙЛ - ШИФР С ДВОЙНОЙ ПЕРЕСТАНОВКОЙ.txt";
                            for (int f = 0; f < 2; f++)
                            {
                                string inputFile;
                                if (f == 0)
                                    inputFile = inputFileName;
                                else
                                    inputFile = "temp.txt";
                                using (StreamReader fin = new StreamReader(inputFile))
                                {
                                    string outputFile;
                                    if (f == 0)
                                        outputFile = "temp.txt";
                                    else
                                        outputFile = path;
                                    FileStream fs = new FileStream(outputFile, FileMode.Create);
                                    using (StreamWriter fout = new StreamWriter(fs, Encoding.UTF8))
                                    {
                                        string str, strTemp = fin.ReadLine();
                                        while (strTemp != null)
                                        {
                                            str = strTemp;
                                            strTemp = fin.ReadLine();
                                            if (f == 0)
                                                symbolsCount += str.Length;
                                            decimal groupsCount = Math.Ceiling(str.Length / size);
                                            string[] table = new string[(int)groupsCount];
                                            for (int i = 0; i < groupsCount; i++)
                                                table[i] = "";
                                            for (int i = 0; i < size * (int)groupsCount; i += (int)groupsCount)
                                            {
                                                string temp = "";
                                                for (int j = i; j < groupsCount + i; j++)
                                                {
                                                    if (j < str.Length)
                                                        temp += str[j];
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
                                                str = table[(int)i];
                                                string groupTemp = "";
                                                for (int j = 0; j < size; j++)
                                                {
                                                    if ((j) >= str.Length)
                                                        break;
                                                    groupTemp += str[j];
                                                }
                                                for (int j = 0; j < size; j++)
                                                {
                                                    int pos = code.ToString()[j] - '0';
                                                    if (j < groupTemp.Length)
                                                    {
                                                        if (f == 0)
                                                            group[(int)i * (int)size + (pos - 1)] = groupTemp[j];
                                                        else
                                                            if (groupTemp[j] != '!')
                                                            group[(int)i * (int)size + (pos - 1)] = groupTemp[j];
                                                    }
                                                    else
                                                        break;
                                                }
                                            }
                                            string strOut = "";
                                            for (int i = 0; i < group.Length; i++)
                                            {
                                                if (group[i] == '\0')
                                                    break;
                                                strOut += group[i];
                                            }

                                            if (strTemp != null)
                                                fout.WriteLine(strOut);
                                            else
                                                fout.Write(strOut);
                                        }
                                        fout.Close();
                                    }
                                    fin.Close();
                                }

                            }
                            label5.Text = symbolsCount.ToString();
                            Finish();
                            System.IO.File.Delete("temp.txt");
                        }
                    }
                }
                else
                {
                    String[] s = textBox4.Text.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (!CheckAlphabet(s[i]))
                        {
                            MessageBox.Show("Ошибка! В тексте присутствуют недопустимые символы!");
                            return;
                        }
                    }
                    for(int f=0; f<2; f++)
                    {
                        for (int k = 0; k < s.Length; k++)
                        {
                            string strTemp = s[k];
                            symbolsCount += strTemp.Length;
                            decimal groupsCount = Math.Ceiling(strTemp.Length / size);
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
                                    int pos = code.ToString()[j] - '0';
                                    if (j < groupTemp.Length)
                                    {
                                        if(f==0)
                                            group[(int)i * (int)size + (pos - 1)] = groupTemp[j];
                                        else
                                            if (groupTemp[j] != '!')
                                                group[(int)i * (int)size + (pos - 1)] = groupTemp[j];
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
                            s[k] = str;
                        }
                    }

                    for(int k=0; k<s.Length; k++)
                    {
                        string str = s[k];
                        if (k != s.Length - 1)
                            str += "\r\n";
                        textBox5.Text = textBox5.Text + str;
                    }
                    label5.Text = symbolsCount.ToString();
                    Finish();
                }
            }
            else
            {
                MessageBox.Show("Ошибка! Проверьте правильность ввода ключа!");
            }
        }
    }
}
