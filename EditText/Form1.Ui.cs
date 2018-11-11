using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditText
{
    /// <summary>
    /// 界面美化相关操作
    /// </summary>
    public partial class Form1 : Form
    {
        private bool formMove = false;//窗体是否移动
        Point formPoint;//窗体位置
        private void toolStrip1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.toolStrip1.Width - 2, this.toolStrip1.Height - 2);
            e.Graphics.SetClip(rect);
        }
        private void toolStrip2_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.toolStrip1.Width - 320, this.toolStrip1.Height - 4);
            e.Graphics.SetClip(rect);
        }
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            menuItem.ForeColor = Color.Blue;
        }
        //鼠标移动窗体功能
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            formPoint = new Point();
            if (e.Button == MouseButtons.Left)
            {
                formPoint = new Point(-e.X, -e.Y);
                formMove = true;//开始移动
            }
        }
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (formMove == true)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(formPoint.X, formPoint.Y);
                Location = mousePos;
            }
        }
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//按下的是鼠标左键
            {
                formMove = false;//停止移动
            }
        }
    }
}
