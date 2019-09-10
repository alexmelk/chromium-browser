using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
namespace browser
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        static volatile List<ChromiumWebBrowser> chromeTab = new List<ChromiumWebBrowser>();
        static volatile List<string> Title = new List<string>();
        static volatile public int selectedIndexTab;
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            // Create a browser component
        }
        public Form1()
        {
            InitializeComponent();
            InitializeChromium();

            timer1.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             Cef.Shutdown();
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            chromeTab[selectedIndexTab].Load(textBox1.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            chromeTab[selectedIndexTab].Back();
        }

        private async void Button3_Click(object sender, EventArgs e)
        {
            chromeTab[selectedIndexTab].Reload();
            while (chromeTab[selectedIndexTab].IsLoading) await Task.Delay(3);
        }
        private void TitleChanged(object sender, TitleChangedEventArgs e)
        {
            if (chromeTab.Count > Title.Count){ Title.Add(e.Title); }
            else { Title[selectedIndexTab] = e.Title;}
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //metroTabPage1.Text = Title;
            for(int i = 0; i < Title.Count; i++)
            {
                metroTabControl1.TabPages[i].Text = Title[i];
            }   
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            metroTabControl1.TabPages.Add(new TabPage("tab "+metroTabControl1.TabPages.Count));

            chromeTab.Add(new ChromiumWebBrowser("http://yandex.ru"));

            metroTabControl1.TabPages[metroTabControl1.TabPages.Count-1].Controls.Add(chromeTab[chromeTab.Count-1]);
            chromeTab[chromeTab.Count-1].Dock = DockStyle.Fill;

            chromeTab[chromeTab.Count - 1].TitleChanged += new EventHandler<TitleChangedEventArgs>(TitleChanged);

            textBox1.Text = chromeTab[chromeTab.Count - 1].Address;

            metroTabControl1.SelectTab(chromeTab.Count - 1);

            metroTabControl1.Update();
        }

        private void MetroTabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                textBox1.Text = chromeTab[e.TabPageIndex].Address;

                selectedIndexTab = e.TabPageIndex;
            }
            catch { }
        }

        private void MetroTabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right) {
                List<string> Titlebuf = new List<string>();
                for (int i = 0; i < Title.Count; i++)
                {
                    if (i != selectedIndexTab) { Titlebuf.Add(Title[i]); }
                }
                Title.Clear(); Title.AddRange(Titlebuf); Titlebuf.Clear();

                List<ChromiumWebBrowser> chromeTabBuf = new List<ChromiumWebBrowser>();
                for (int i = 0; i < chromeTab.Count; i++)
                {
                    if (i != selectedIndexTab) { chromeTabBuf.Add(chromeTab[i]); }
                }
                chromeTab.Clear(); chromeTab.AddRange(chromeTabBuf); chromeTabBuf.Clear();

                metroTabControl1.TabPages[selectedIndexTab].Dispose();

            }
        }
    }
}
