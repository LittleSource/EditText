using System;
using System.Drawing;
using System.Windows.Forms;

namespace EditText
{
    /// <summary>
    /// 工具栏or图片按钮相关功能
    /// </summary>
    public partial class Form1 : Form
    {
        //字体字号变量
        public string[] FontSizeName = { "初号", "小初", "一号", "小一", "二号", "小二", "三号", "小三", "四号", "小四", "五号", "小五", "六号", "小六", "七号", "八号", "8", "9", "10", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };
        public float[] FontSize = { 42, 36, 26, 24, 22, 18, 16, 15, 14, 12, 10.5F, 9, 7.5F, 6.5F, 5.5F, 5, 8, 9, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        /// <summary>
        /// 字体下拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (issetpage())
            {
                ChangeFontSizeOrTypeface(1);
            }
        }
        /// <summary>
        /// 字号下拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (issetpage())
            {
                ChangeFontSizeOrTypeface(2);
            }
        }
        /// <summary>
        /// 加粗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton加粗_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Bold);
        }
        /// <summary>
        /// 下划线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton下划线_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Underline);
        }
        /// <summary>
        /// 斜体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton斜体_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Italic);
        }
        /// <summary>
        /// 文本颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton颜色_Click(object sender, EventArgs e)
        {
            if (issetpage())
            {
                if (colorDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionColor = colorDialog1.Color;
                }
            }
        }
        /// <summary>
        /// 居左
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton左_Click(object sender, EventArgs e)
        {
            pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionAlignment = HorizontalAlignment.Left;
        }
        /// <summary>
        /// 居中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton中_Click(object sender, EventArgs e)
        {
            pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }
        /// <summary>
        /// 居右
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton右_Click(object sender, EventArgs e)
        {
            pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionAlignment = HorizontalAlignment.Right;
        }
        /// <summary>
        /// 改变字号和字体共用函数
        /// 避免richbox选中字中有两个字字体不一致
        /// 导致选择到的字体为null，造成空指针异常
        /// </summary>
        /// <param name="type">1：字体 2：字号</param>
        private void ChangeFontSizeOrTypeface(int type)
        {
            RichTextBox tempRichTextBox = new RichTextBox();
            int curRtbStart = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionStart;
            int len = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionLength;
            int tempRtbStart = 0;
            Font font = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont;
            if (len <= 1 && font != null)
            {
                if (type == 1)
                {
                    pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont = new Font(this.toolStripComboBox1.Text, pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont.Size);
                    return;
                }
                else
                {
                    pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont = new Font(this.toolStripComboBox1.Text, FontSize[this.toolStripComboBox2.SelectedIndex], pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont.Style);
                    return;
                }
            }
            tempRichTextBox.Rtf = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectedRtf;
            tempRichTextBox.Select(len - 1, 1); //选中副本中的最后一个文字  
            Font tempFont = (Font)tempRichTextBox.SelectionFont.Clone();//克隆被选中的文字Font，这个tempFont主要是用来判断 
            for (int i = 0; i < len; i++)
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);  //每次选中一个
                if(type == 1)
                {
                    tempRichTextBox.SelectionFont = new Font(this.toolStripComboBox1.Text, tempRichTextBox.SelectionFont.Size);
                }
                else
                {
                    tempRichTextBox.SelectionFont = new Font(this.toolStripComboBox1.Text, FontSize[this.toolStripComboBox2.SelectedIndex],tempRichTextBox.SelectionFont.Style);
                }
            }
            tempRichTextBox.Select(tempRtbStart, len);
            pageList[tabControl1.SelectedIndex].RichTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf; //将设置格式后的副本拷贝给原型  
            pageList[tabControl1.SelectedIndex].RichTextBox1.Select(curRtbStart, len);
        }
        ///<summary> 
        ///自定义方法
        ///设置字体格式：粗体、斜体、下划线
        ///</summary>  
        /// <param name="style">事件触发后传参：字体格式类型</param>  
        private void ChangeFontStyle(FontStyle style)
        {
            RichTextBox tempRichTextBox = new RichTextBox();  //将要存放被选中文本的副本  
            int curRtbStart = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionStart;
            int len = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionLength;
            int tempRtbStart = 0;
            Font font = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont;
            if (len <= 1 && font != null)
            {
                if (style == FontStyle.Bold && font.Bold ||
                    style == FontStyle.Italic && font.Italic ||
                    style == FontStyle.Underline && font.Underline)
                {
                    pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont = new Font(font, font.Style ^ style);
                }
                else if (style == FontStyle.Bold && !font.Bold ||
                         style == FontStyle.Italic && !font.Italic ||
                         style == FontStyle.Underline && !font.Underline)
                {
                    pageList[tabControl1.SelectedIndex].RichTextBox1.SelectionFont = new Font(font, font.Style | style);
                }
                return;
            }
            tempRichTextBox.Rtf = pageList[tabControl1.SelectedIndex].RichTextBox1.SelectedRtf;
            tempRichTextBox.Select(len - 1, 1); //选中副本中的最后一个文字  
            Font tempFont = (Font)tempRichTextBox.SelectionFont.Clone();//克隆被选中的文字Font，这个tempFont主要是用来判断  
            for (int i = 0; i < len; i++)
            {
                tempRichTextBox.Select(tempRtbStart + i, 1);  //每次选中一个，逐个进行加粗或去粗  
                if (style == FontStyle.Bold && tempFont.Bold ||
                    style == FontStyle.Italic && tempFont.Italic ||
                    style == FontStyle.Underline && tempFont.Underline)
                {
                    tempRichTextBox.SelectionFont =
                        new Font(tempRichTextBox.SelectionFont,
                                 tempRichTextBox.SelectionFont.Style ^ style);
                }
                else if (style == FontStyle.Bold && !tempFont.Bold ||
                         style == FontStyle.Italic && !tempFont.Italic ||
                         style == FontStyle.Underline && !tempFont.Underline)
                {
                    tempRichTextBox.SelectionFont =
                        new Font(tempRichTextBox.SelectionFont,
                                 tempRichTextBox.SelectionFont.Style | style);
                }
            }
            tempRichTextBox.Select(tempRtbStart, len);
            pageList[tabControl1.SelectedIndex].RichTextBox1.SelectedRtf = tempRichTextBox.SelectedRtf; //将设置格式后的副本拷贝给原型  
            pageList[tabControl1.SelectedIndex].RichTextBox1.Select(curRtbStart, len);
        }
    }
}
