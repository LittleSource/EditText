using System;
using System.Drawing;
using System.Windows.Forms;

namespace EditText
{
    /// <summary>
    /// 此类封装文件编辑页面
    /// 将RichTextBox装进TabPage
    /// 并得到文件的名字safeFileName和绝对路径pathAndFileName
    /// </summary>
    class Page
    {
        private RichTextBox richTextBox1;
        private TabPage tabPage1;
        private string safeFileName;
        private string pathAndFileName;
        public RichTextBox RichTextBox1 { get => richTextBox1; set => richTextBox1 = value; }
        public TabPage TabPage1 { get => tabPage1; set => tabPage1 = value; }
        public string SafeFileName { get => safeFileName; set => safeFileName = value; }
        public string PathAndFileName { get => pathAndFileName; set => pathAndFileName = value; }

        public Page(int tabCount, string fileName, string rootFileName)
        {
            this.pathAndFileName = rootFileName;
            this.safeFileName = fileName;

            this.TabPage1 = new TabPage
            {
                Location = new Point(4, 25),
                Name = "tabPage" + tabCount,
                Padding = new Padding(3),
                Size = new Size(192, 71),
                TabIndex = tabCount,
                Text = fileName,
                UseVisualStyleBackColor = true
            };
            this.RichTextBox1 = new RichTextBox
            {
                BorderStyle = BorderStyle.None,
                Location = new Point(1, 1),
                Name = "richTextBox" + tabCount,
                Size = new Size(460, 590),
                TabIndex = 5,
                Text = "",
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };
            //向TabPage添加richTextBox
            this.TabPage1.Controls.Add(this.RichTextBox1);
            //richTextBox加载文件
            this.RichTextBox1.LoadFile(this.pathAndFileName, RichTextBoxStreamType.PlainText);
            this.RichTextBox1.SelectionStart = RichTextBox1.TextLength;
        }
        public TabControl creatTabControl()
        {
            return new TabControl
            {
                Location = new Point(15, 91),
                Name = "tabControl1",
                SelectedIndex = 0,
                Size = new Size(478, 585),
                TabIndex = 7
            };
        }
    }
}
