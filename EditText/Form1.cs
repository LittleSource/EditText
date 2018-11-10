using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EditText
{
    public partial class Form1 : Form
    {
        private List<Page> pageList = new List<Page>();
        private FileOperation file = new FileOperation();
        private int tabCount = 0;//计数tabpage
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (FontFamily font in FontFamily.Families)
                this.toolStripComboBox1.Items.Add(font.Name);//设置字体内容
            foreach (string name in FontSizeName)
                this.toolStripComboBox2.Items.Add(name);//设置字号下拉菜单内容
        }
        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void 字体FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                if (tabCount > 0 && fontDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    this.pageList[tabControl1.SelectedIndex - 1].RichTextBox1.Font = fontDialog1.Font;
                }
            }
        }

        private void 颜色LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                if (tabCount > 0 && colorDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    this.pageList[tabControl1.SelectedIndex].RichTextBox1.ForeColor = colorDialog1.Color;
                }
            }
        }
        /// <summary>
        /// 文件菜单下的相关操作
        /// </summary>
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                Page page = pageList[tabControl1.SelectedIndex];
                if (!file.saveFile(page.RichTextBox1, page.PathAndFileName))
                {
                    MessageBox.Show("保存失败！");
                }
            }
        }
        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                Page page = pageList[tabControl1.SelectedIndex];
                if (!file.saveFileAs(page.RichTextBox1))
                {
                    MessageBox.Show("保存失败！");
                }
            }
        }
        private void 退出ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.label2_Click(sender, e);
        }
        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file.creatFile(tabCount + 1);
            addPage(file.NewFileName, file.NewFilePath);
        }
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = file.openFile();
            if (result == DialogResult.OK)
            {
                addPage(file.SafeFileName, file.PathAndFileName);
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                MessageBox.Show("打开文件失败！");
            }
        }

        /// <summary>
        /// 编辑菜单下的相关操作
        /// </summary>
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                if (pageList[tabControl1.SelectedIndex].RichTextBox1.SelectedText != "")
                {
                    pageList[tabControl1.SelectedIndex].RichTextBox1.Copy();
                }
            }
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                if (pageList[tabControl1.SelectedIndex].RichTextBox1.SelectedText != "")
                {
                    this.pageList[tabControl1.SelectedIndex].RichTextBox1.Cut();
                }
            }
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                this.pageList[tabControl1.SelectedIndex].RichTextBox1.Paste();
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pageList[tabControl1.SelectedIndex].RichTextBox1.Text = "";
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                this.pageList[tabControl1.SelectedIndex].RichTextBox1.Undo();
            }
        }
        private void 重做ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                this.pageList[tabControl1.SelectedIndex].RichTextBox1.Redo();
            }
        }


        private void addPage(string fileName, string rootFileName)
        {
            Page page = new Page(this.tabCount, fileName, rootFileName);
            pageList.Add(page);
            if (this.tabCount == 0)
            {
                this.tabControl1 = page.creatTabControl();
                this.Controls.Add(this.tabControl1);
                this.Controls.Remove(label1);//移除控件
            }
            this.tabControl1.TabPages.Add(page.TabPage1);
            //切换到新的TabPage
            tabControl1.SelectedTab = tabControl1.TabPages[this.tabCount];
            page.RichTextBox1.Focus();
            this.tabCount++;
            this.toolStripComboBox1.SelectedItem = "宋体";//设置字体显示内容
            this.toolStripComboBox2.SelectedItem = "8";//设置字号显示内容
        }

        private bool issetpage()
        {
            if (this.tabCount == 0)
            {
                MessageBox.Show("请先打开或创建一个文件！");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
