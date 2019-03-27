using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SHDocVw;
using System.Runtime.InteropServices;


namespace HttpReqestsBot
{
    public partial class Form1 : Form
    {
        //IEの入れ物
        SHDocVw.InternetExplorer ie = null;
        //テキストボックス
        StringReader strReader = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //フォームサイズ固定
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            //ファイル読み込みダイアログの設定
            openFileDialog1.Title = "URLリストを選択してください。";
            openFileDialog1.FileName = "URL_List.txt";
            openFileDialog1.Filter = "てきすとファイル(*.txt)|*.txt;|すべてのファイル(*.*)|*.*";
            //ボタン関連
            button3.Enabled = false;
        }

        //ファイル読み込み
        private void button4_Click(object sender, EventArgs e)
        {
            //ファイルダイアログが正常に閉じた場合
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //テキストボックスにURL一覧表示
                StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("Shift_JIS"));

                richTextBox1.Text = sr.ReadToEnd();

                sr.Close();
            }
        }
        //テキストボックス全消し
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
        //一回のみ起動
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            ie = new SHDocVw.InternetExplorer();
            ie.Visible = false;
            read_req_file();
            ie.Quit();
            button1.Enabled = true;
        }
        //ファイル読み込みからのリクエスト
        private void read_req_file()
        {
            strReader = new StringReader(richTextBox1.Text);
            //末端までr－プ
            while (strReader.Peek() > -1)
            {
                http_Req(strReader.ReadLine());
            }
            strReader.Close();
        }
        //HTTPリクエスト
        private void http_Req(String URLStr)
        {
            while(ie.Busy)
            {
            }
            //リクエスト
            ie.Navigate(URLStr);
        }
        //秒おきに実行
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = true;
            ie = new SHDocVw.InternetExplorer();
            ie.Visible = false;
            //タイマーの秒設定
            timer1.Interval = 1000 * int.Parse(textBox1.Text);
            timer1.Enabled = true;
            read_req_file();
        }
        //タイマー
        private void timer1_Tick(object sender, EventArgs e)
        {
            read_req_file();
        }
        //停止ボタン
        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            ie.Quit();
        }
    }
}
