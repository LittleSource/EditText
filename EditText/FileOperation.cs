using System;
using System.IO;
using System.Windows.Forms;

namespace EditText
{
    class FileOperation
    {
        private string safeFileName;
        private string pathAndFileName;
        private string newFilePath;
        private string newFileName;
        public string SafeFileName
        {
            get { return safeFileName; }
        }
        public string PathAndFileName
        {
            get { return pathAndFileName; }
        }
        public string NewFilePath
        {
            get { return newFilePath; }
        }
        public string NewFileName
        {
            get { return newFileName; }
        }
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <returns>文件打开结果</returns>
        public DialogResult openFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Word文件(*.doc)|*.doc|文本文件(*.txt)|*.txt|rtf文件(*.rtf)|*.rtf|所有文件(*.*)|*.*";
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.safeFileName = openFileDialog1.SafeFileName;
                this.pathAndFileName = openFileDialog1.FileName;
            }
            return result;
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileSort">文件序号</param>
        public void creatFile(int fileSort)
        {
            string newFileName = "新文件" + fileSort + ".doc";
            string rootFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string newFilePath = rootFilePath + '\\' + newFileName;
            //如果文件不存在就创建文件
            if (!File.Exists(newFilePath))
            {
                FileStream fs = File.Create(newFilePath);
                fs.Close();
            }
            this.newFileName = newFileName;
            this.newFilePath = newFilePath;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="richTextBox1"></param>
        /// <param name="pathAndFileName"></param>
        /// <returns></returns>
        public bool saveFile(RichTextBox richTextBox1, string pathAndFileName)
        {
            try
            {
                richTextBox1.SaveFile(pathAndFileName, RichTextBoxStreamType.PlainText);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="richTextBox1"></param>
        /// <returns></returns>
        public bool saveFileAs(RichTextBox richTextBox1)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word文件(*.doc)|*.doc|文本文件(*.txt)|*.txt|rtf文件(*.rtf)|*.rtf|所有文件(*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录
            if (saveFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                return saveFile(richTextBox1, saveFileDialog.FileName);
            }
            else
            {
                return true;//取消保存也为真，避免弹框
            }
        }
    }
}
