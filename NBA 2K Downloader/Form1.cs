/*  Copyright (C) 2013 Ranbir Aulakh

    Visit: http://elegantdevs.com  Thank you.
 
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NBA_2K_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Fetch Files from Server.
        private void Form1_Load(object sender, EventArgs e)
        {
            this.RefreshCMG("http://forumzero.net78.net/MyPlayerList.ini");
        }

        //Refresh the List.
        private void RefreshCMG(string fetchUrls)
        {
            this.listView1.Items.Clear();
            try
            {
                new WebClient().DownloadFile(fetchUrls, Application.UserAppDataPath + @"\AppSettings.ini");
                foreach (string str in System.IO.File.ReadAllLines(Application.UserAppDataPath + @"\AppSettings.ini"))
                {
                    if (str.StartsWith("["))
                    {
                        Class1 file = new Class1(Application.UserAppDataPath + @"\AppSettings.ini");
                        string playerName = str.Replace("[", "").Replace("]", "");
                        string size = file.IniReadValue(playerName, "Size").Replace(",", "");
                        string url = file.IniReadValue(playerName, "URL");
                        string text = size;
                        if (Convert.ToInt32(size) >= 0x40000000)
                        {
                            size = Convert.ToString((long)((((long)Math.Round((double)(((double)Convert.ToInt32(size)) / 1024.0))) / 0x400L) / 0x400L)) + " GBs";
                        }
                        else if (Convert.ToInt32(size) >= 0x100000)
                        {
                            size = Convert.ToString((long)(((long)Math.Round((double)(((double)Convert.ToInt32(size)) / 1024.0))) / 0x400L)) + " MBs";
                        }
                        else if (Convert.ToInt32(size) >= 0x400)
                        {
                            size = Convert.ToString((double)(((double)Convert.ToInt32(size)) / 1024.0)) + " KBs";
                        }
                        ListViewItem item = new ListViewItem(playerName);
                        item.SubItems.Add(size);
                        item.SubItems.Add(text);
                        item.Tag = url;
                        this.listView1.Items.Add(item);
                        Class1 file2 = new Class1(Application.UserAppDataPath + @"\AppSettings.ini");
                        string playerName_2 = str.Replace("[", "").Replace("]", "");
                        string size_2 = file2.IniReadValue(playerName, "Size").Replace(",", "");
                        string url_2 = file2.IniReadValue(playerName, "URL");
                        string text_2 = size_2;
                        if (Convert.ToInt32(size_2) >= 0x40000000)
                        {
                            size_2 = Convert.ToString((long)((((long)Math.Round((double)(((double)Convert.ToInt32(size_2)) / 1024.0))) / 0x400L) / 0x400L)) + " GBs";
                        }
                        else if (Convert.ToInt32(size_2) >= 0x100000)
                        {
                            size_2 = Convert.ToString((long)(((long)Math.Round((double)(((double)Convert.ToInt32(size_2)) / 1024.0))) / 0x400L)) + " MBs";
                        }
                        else if (Convert.ToInt32(size_2) >= 0x400)
                        {
                            size_2 = Convert.ToString((double)(((double)Convert.ToInt32(size_2)) / 1024.0)) + " KBs";
                        }
                        ListViewItem item2 = new ListViewItem(playerName_2);
                        item2.SubItems.Add(size_2);
                        item2.SubItems.Add(text_2);
                        item2.Tag = url_2;
                        this.listView2.Items.Add(item2);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error Connecting to Server! Try again later!", "Lost Connection",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Refresh Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.RefreshCMG("http://forumzero.net78.net/MyPlayerList.ini");
        }

        //Download CMG Button
        private void button1_Click(object sender, EventArgs e)
        {
            this.DownloadCMG(this.listView1.Name);
        }

        //Download Files from List
        private void DownloadCMG(string Selected)
        {
            if (this.listView1.Items.Count == 0)
            {
                MessageBox.Show("Select a MP item first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Title = "Download MyPlayer...",
                    FileName = ""
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    IEnumerator enumerator;
                    try
                    {
                        enumerator = this.listView1.SelectedItems.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            ListViewItem current = (ListViewItem)enumerator.Current;
                            string text = current.Text;
                            try
                            {
                                Uri urlAddress = new Uri(current.Tag.ToString());
                                new WebClient().DownloadFileAsync(urlAddress, dialog.FileName + ".zip");
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    finally
                    {

                    }
                    MessageBox.Show("Download Complete. \nCreated by Ranbir Aulakh \nThis is Open-Source. Visit: http://www.elegantdevs.com", "Done");
                }
            }
        }

        //Search Files
        private void SearchFor(TextBox Search)
        {
            IEnumerator enumerator;
            bool flag = false;
            try
            {
                enumerator = this.listView1.Items.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ListViewItem current = (ListViewItem)enumerator.Current;
                    if (current.Text.ToLower().Trim().Contains(Search.Text.ToLower().Trim()))
                    {
                        current.BackColor = Color.Lime;
                        current.EnsureVisible();
                        flag = true;
                        if (MessageBox.Show("My Player Found " + current.Text + " Would You Like To Download This My Player", "", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            current.Selected = true;
                            this.DownloadCMG(System.Convert.ToString(current.Selected));
                        }
                    }
                }
            }
            finally
            {

            }
            if (!flag)
            {
                MessageBox.Show("No matches found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        
        //Search File Button
        private void button3_Click(object sender, EventArgs e)
        {
            this.SearchFor(this.textBox1);
        }

        //About
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NBA 2K Downloader 1.0.0 \nCreated by Ranbir Aulakh \nThis is Open-Source. Visit: http://www.elegantdevs.com", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
