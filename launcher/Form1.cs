﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;



namespace WindowsFormsApp4
{

    public partial class Form1 : Form
    {
        public class Teams
        {
            public string Name { get; set; }
        }
        public class RoomObject
        {
            public string status { get; set; }
            public List<Teams> teams { get; set; }
            public string contestId { get; set; }
            public string ip { get; set; }
            public string port { get; set; }
            public string createdBy { get; set; }
            public string updatedBy { get; set; }
            public string createdAt { get; set; }
            public string updatedAt { get; set; }
            public string id { get; set; }
        }
        public class Rooms
        {
            public List<RoomObject> room { get; set; }
        }
        public Form1()
        {
            InitializeComponent();
            this.Text = "登录";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入用户名");
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("请输入密码");
            }
            else {
                Program example1 = new Program();
                string data = "{\"username\": \"" + textBox1.Text + "\",\"password\": \"" + textBox2.Text + "\"}";
                //HttpPost本身也会返回布尔值以供判断成功与否
                if (example1.HttpPost("https://api.eesast.com/v1/users/login", data))
                {
                    MessageBox.Show("登陆成功！");
                    button1.Visible = false;
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;

                    serverPort.Visible = true;
                    clientPort.Visible = true;
                    agentNumber.Visible = true;
                    button3.Visible = true;
                    button4.Visible = true;
                    this.Text = "启动器";
                    this.Width = 750;
                    listRoom.Visible = true;
                    this.listRoom.Items.Clear();
                    labelRoom.Visible = true;
                    String rooms_0 = example1.HttpGet("https://api.eesast.com/v1/rooms?contestId=2&status=0");
                    if (rooms_0 == "等待中房间请求失败")
                    {
                        this.listRoom.Items.Add(rooms_0);
                    }
                    else
                    {
                        rooms_0 = "{\"room\":" + rooms_0 + "}";
                        Console.WriteLine(rooms_0);
                        Rooms roooms = JsonConvert.DeserializeObject<Rooms>(rooms_0);
                        foreach (RoomObject rp in roooms.room)
                        {
                            string roomInfo = "ID:" + rp.id + " Port:" + rp.port + " 创建者:" + rp.createdBy + "状态:等待中";
                            this.listRoom.Items.Add(roomInfo);
                        }

                    }
                    String rooms_1 = example1.HttpGet("https://api.eesast.com/v1/rooms?contestId=2&status=1");
                    if (rooms_1 == "运行中房间请求失败")
                    {
                        this.listRoom.Items.Add(rooms_1);
                    }
                    else
                    {
                        rooms_1 = "{\"room\":" + rooms_1 + "}";
                        Console.WriteLine(rooms_1);
                        Rooms roooms = JsonConvert.DeserializeObject<Rooms>(rooms_1);
                        foreach (RoomObject rp in roooms.room)
                        {
                            string roomInfo = "ID:" + rp.id + "  Port:" + rp.port + "  创建者:" + rp.createdBy + "  状态:进行中";
                            this.listRoom.Items.Add(roomInfo);
                        }

                    }

                    //this.Close();
                    //Application.Run(new Form1());
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public class Program
        {

            
            public bool HttpPost(string url, string data)
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                //字符串转换为字节码
                byte[] bs = Encoding.UTF8.GetBytes(data);
                //参数类型，这里是json类型
                httpWebRequest.ContentType = "application/json";
                //参数数据长度
                httpWebRequest.ContentLength = bs.Length;
                //设置请求类型
                httpWebRequest.Method = "POST";
                //设置超时时间
                httpWebRequest.Timeout = 20000;
                //将参数写入请求地址中
                httpWebRequest.GetRequestStream().Write(bs, 0, bs.Length);
                //发送请求
                try
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //这后面的代码是登陆成功后执行的
                    Console.WriteLine("登陆成功");
                    httpWebRequest.Abort();
                    return true;
                }
                catch (WebException ex)
                {
                    //这后面的代码是登录失败后执行的
                    if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("用户名或密码错误");
                    }
                    else
                    {
                        MessageBox.Show("用户不存在");
                    }
                    httpWebRequest.Abort();
                    return false;

                }
            }
            public String HttpGet(string url)
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

                //设置请求类型
                httpWebRequest.Method = "GET";

                httpWebRequest.Accept = "application/json";
                //设置超时时间
                httpWebRequest.Timeout = 20000;
                //发送请求
                try
                {
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //这后面的代码是登陆成功后执行的

                    System.IO.Stream getStream = httpWebResponse.GetResponseStream();
                    System.IO.StreamReader streamreader = new System.IO.StreamReader(getStream);
                    String result = streamreader.ReadToEnd();
                    httpWebRequest.Abort();
                    return result;
                }
                catch (WebException ex)
                {
                    return "请求失败";
                }
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int clientNumber = Convert.ToInt32(this.agentNumber.Text);
            string serverPort = this.serverPort.Text;
            string clientPort = this.clientPort.Text;
            string argumentsServer, argumentsAgent, argumentsClient;


            System.Diagnostics.Process processServer = new System.Diagnostics.Process();
            processServer.StartInfo.FileName = ".\\THUAI3.0\\Logic.Server.exe";
            //传递进exe的参数
            argumentsServer = "-p " + serverPort + " -d 1 -c 2 -a 1 -t 600";
            processServer.StartInfo.Arguments = argumentsServer;
            processServer.Start();
            Console.WriteLine("进程Server");

            System.Diagnostics.Process processAgent = new System.Diagnostics.Process();
            processAgent.StartInfo.FileName = ".\\THUAI3.0\\Communication.Agent.exe";

            argumentsAgent = "--server 127.0.0.1:" + serverPort + " --port " + clientPort + " -t THUAI/offline -d 0";
            processAgent.StartInfo.Arguments = argumentsAgent;
            processAgent.Start();
            Console.WriteLine("进程Agent");


            for (int k = 0; k < clientNumber; k++)
            {
                System.Diagnostics.Process processClient = new System.Diagnostics.Process();
                processClient.StartInfo.FileName = ".\\THUAI3.0\\Logic.Client.exe";

                argumentsClient = "-p " + clientPort + " -d 1 -t 2";
                processClient.StartInfo.Arguments = argumentsClient;
                processClient.Start();
                Console.Write("进程Client");
                Console.WriteLine(k);
            }


            processServer.WaitForExit();
            processAgent.WaitForExit();
            //processClient.WaitForExit();
            Console.WriteLine("进程全部退出");
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void labelRoom_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program example1 = new Program();
            this.listRoom.Items.Clear();
            String rooms_0 = example1.HttpGet("https://api.eesast.com/v1/rooms?contestId=2&status=0");
            if (rooms_0 == "等待中房间请求失败")
            {
                this.listRoom.Items.Add(rooms_0);
            }
            else
            {
                rooms_0 = "{\"room\":" + rooms_0 + "}";
                Console.WriteLine(rooms_0);
                Rooms roooms = JsonConvert.DeserializeObject<Rooms>(rooms_0);
                foreach (RoomObject rp in roooms.room)
                {
                    string roomInfo = "ID:" + rp.id + " Port:" + rp.port + " 创建者:" + rp.createdBy + "状态:等待中";
                    this.listRoom.Items.Add(roomInfo);
                }

            }
            String rooms_1 = example1.HttpGet("https://api.eesast.com/v1/rooms?contestId=2&status=1");
            if (rooms_1 == "运行中房间请求失败")
            {
                this.listRoom.Items.Add(rooms_1);
            }
            else
            {
                rooms_1 = "{\"room\":" + rooms_1 + "}";
                Console.WriteLine(rooms_1);
                Rooms roooms = JsonConvert.DeserializeObject<Rooms>(rooms_1);
                foreach (RoomObject rp in roooms.room)
                {
                    string roomInfo = "ID:" + rp.id + "  Port:" + rp.port + "  创建者:" + rp.createdBy + "  状态:进行中";
                    this.listRoom.Items.Add(roomInfo);
                }

            }
        }
    }
}