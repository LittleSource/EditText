using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
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
        /// <summary>
        /// 退出程序
        /// </summary>
        private void label2_Click(object sender, EventArgs e)
        {
            if(tabCount == 0)
            {
                Application.Exit();
            }
            else
            {
                for(int i = pageList.Count - 1; i >= 0; i--)
                {
                    closePage(i);
                }
            }
        }
        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 文件菜单下的相关操作
        /// </summary>
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
        private void 关闭CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                int index = tabControl1.SelectedIndex;
                closePage(index);
            }
        }
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                savePage(tabControl1.SelectedIndex);
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
        private void 自动保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                if (this.自动保存ToolStripMenuItem.Checked)
                {
                    this.自动保存ToolStripMenuItem.Checked = false;
                }
                else
                {
                    if (!pageList[tabControl1.SelectedIndex].Issave)
                        savePage(tabControl1.SelectedIndex);
                    this.自动保存ToolStripMenuItem.Checked = true;
                }
            }
        }

        /// <summary>
        /// 格式菜单下的相关操作
        /// </summary>
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
        /// 扩展菜单下的相关操作
        /// </summary>
        private void 运行命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                string command = pageList[tabControl1.SelectedIndex].RichTextBox1.Text;
                if (command.Trim() != "")
                {
                    pageList[tabControl1.SelectedIndex].RichTextBox1.Text += Tools.RunCmd(command);
                }
                else
                {
                    MessageBox.Show("检测到您还没有任何输入，请先输入Cmd命令!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void 打印ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;
                StringReader lineReader = new StringReader(pageList[tabControl1.SelectedIndex].RichTextBox1.Text);
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        printDocument.Print();

                    }
                    catch (Exception excep)
                    {
                        MessageBox.Show(excep.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        printDocument.PrintController.OnEndPrint(printDocument, new PrintEventArgs());
                    }
                }
            }
        }
        private void 彩蛋ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        /// <summary>
        /// 帮助菜单下的相关操作
        /// </summary>
        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
        private void 获取更多帮助信息MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ymkjUrl = "http://www.ym998.cn";
            System.Diagnostics.Process.Start("iexplore.exe", ymkjUrl);
        }

        /// <summary>
        /// 底部状态栏相关操作
        /// </summary>
        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (issetpage())
            {
                if (this.自动保存ToolStripMenuItem.Checked)
                {
                    savePage(tabControl1.SelectedIndex);
                }
                else
                {
                    if (pageList[tabControl1.SelectedIndex].Issave)
                    {
                        //设置为未保存状态
                        pageList[tabControl1.SelectedIndex].Issave = false;
                        pageList[tabControl1.SelectedIndex].TabPage1.Text += '*';
                    }
                }
            }
            countWord();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RichTextBox1_TextChanged(sender, e);
        }

        /// <summary>
        /// 以下是自定义函数
        /// </summary>
        private void addPage(string fileName, string rootFileName)
        {
            Page page = new Page(this.tabCount, fileName, rootFileName);
            //注册TextChanged事件
            page.RichTextBox1.TextChanged += new EventHandler(this.RichTextBox1_TextChanged);
            pageList.Add(page);
            if (this.tabControl1 == null)
            {
                this.tabControl1 = page.creatTabControl();
                //注册TabPage双击事件,用来关闭TabPage
                this.tabControl1.DoubleClick += new EventHandler(this.关闭CToolStripMenuItem_Click);
                //注册SelectedIndexChanged事件
                this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
                this.Controls.Add(this.tabControl1);
                this.Controls.Remove(label1);//移除控件
                // toolStripStatusLabel统计字符
                this.toolStripStatusLabel统计字符.Name = "toolStripStatusLabel2";
                this.toolStripStatusLabel统计字符.Size = new Size(99, 20);
            }
            this.tabControl1.TabPages.Add(page.TabPage1);
            //切换到新的TabPage
            tabControl1.SelectedTab = tabControl1.TabPages[this.tabCount];
            page.RichTextBox1.Focus();
            this.tabCount++;
            this.toolStripComboBox1.SelectedItem = "宋体";//设置字体显示内容
            this.toolStripComboBox2.SelectedItem = "20";//设置字号显示内容
            countWord();
        }
        private void countWord()
        {
            if (tabControl1.SelectedIndex >= 0)
            {
                string s = pageList[tabControl1.SelectedIndex].RichTextBox1.Text;
                this.toolStripStatusLabel统计字符.Text = "共计：" + s.Length + " 个字符";
                this.toolStripStatusLabel汉字.Text = " " + Tools.getChinese(s) + " 个汉字";
            }
            else
            {
                this.toolStripStatusLabel统计字符.Text = "共计：0 个字符";
                this.toolStripStatusLabel汉字.Text = " 0 个汉字";
            }
        }
        private bool issetpage()
        {
            if (this.tabCount == 0)
            {
                MessageBox.Show("请先打开或创建一个文件！","提示",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }
        private void savePage(int index)
        {
            Page page = pageList[index];
            if (!file.saveFile(page.RichTextBox1, page.PathAndFileName))
            {
                MessageBox.Show("保存失败！");
                return;
            }
            else
            {
                if (!自动保存ToolStripMenuItem.Checked)
                {
                    //设置为保存状态
                    pageList[index].Issave = true;
                    pageList[index].TabPage1.Text = pageList[tabControl1.SelectedIndex].TabPage1.Text.Remove(pageList[tabControl1.SelectedIndex].TabPage1.Text.Length - 1, 1);
                }
            }
        }
        private void closePage(int index)
        {
            if (pageList[index].Issave == false)
            {
                DialogResult result = MessageBox.Show(pageList[index].TabPage1.Text.Remove(pageList[index].TabPage1.Text.Length - 1, 1) + "还未保存，是否保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    savePage(index);
                }
            }
            if (pageList.Count == 1)
                Application.Exit();
            tabControl1.TabPages.Remove(pageList[index].TabPage1);
            pageList.Remove(pageList[index]);
            if (tabCount > 0)
                tabCount--;
        }
    }
}