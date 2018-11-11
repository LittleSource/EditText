using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditText
{
    /// <summary>
    /// 工具类
    /// 主要统计字数等
    /// </summary>
    class Tools
    {
        public static int getChinese(string s)
        {
            return getByteLength(s) - s.Length;
        }
        /// <summary>
        /// 返回数字（0~9）字数数量
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int getdigitalLength(string s)
        {
            int lx = 0;
            char[] q = s.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if ((int)q[i] >= 48 && (int)q[i] <= 57)
                {
                    lx += 1;
                }
            }
            return lx;
        }

        /// <summary>
        /// 返回字母（A~Z-a~z）字数数量
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int getcharLength(string s)
        {
            int lz = 0;
            char[] q = s.ToLower().ToCharArray();//大写字母转换成小写字母
            for (int i = 0; i < q.Length; i++)
            {
                if ((int)q[i] >= 97 && (int)q[i] <= 122)//小写字母
                {
                    lz += 1;
                }
            }
            return lz;
        }

        /// <summary>
        /// 返回字节数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int getByteLength(string s)
        {
            int lh = 0;
            char[] q = s.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if ((int)q[i] >= 0x4E00 && (int)q[i] <= 0x9FA5) // 汉字
                {
                    lh += 2;
                }
                else
                {
                    lh += 1;
                }
            }
            return lh;
        }
        public static string RunCmd(string command)
        {
            string output = ""; //输出字符串 
            if (command != null && !command.Equals(""))
            {
                //创建进程对象 
                Process process = new Process();
                //设定需要执行的命令 
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";
                //“/C”表示执行完命令后马上退出 
                startInfo.Arguments = "/C " + command;
                //不使用系统外壳程序启动
                startInfo.UseShellExecute = false;
                //不重定向输入 
                startInfo.RedirectStandardInput = false;
                //重定向输出 
                startInfo.RedirectStandardOutput = true;
                //不创建窗口 
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;
                try
                {
                    //开始进程
                    if (process.Start())
                    {
                        //等待进程结束，等待时间为指定的毫秒 
                        process.WaitForExit(5000);
                    }
                    //读取进程的输出
                    output = process.StandardOutput.ReadToEnd();
                }
                catch (Exception ex)
                {
                    output = ("出现异常：" + ex.Message);
                }
                finally
                {
                    if (process != null)
                    {
                        process.Close();
                    }
                }

            }
            return output;
        }
    }
}
