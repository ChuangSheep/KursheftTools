using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursheftTools
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            ContactLL.Text = "Found a bug or have suggesstions? Contact me at chuang__@outlook.com";
            ContactLL.Links.Add(48, 20, "mailto:chuang__@outlook.com?Subject=KursheftTools%20Feedback");

            GithubL.Text = "Or create an issue on Github";
            GithubL.Links.Add(22, 6, "https://github.com/ChuangSheep/KursheftTools");


            LicenseRTB.Text = "Licensed under the Apache License, Version 2.0 (the \"License\");\r\n" +
                    "you may not use this file except in compliance with the License.\r\n\r\n" +
                    "You may obtain a copy of the License at\r\n\r\n" +
                    "http://www.apache.org/licenses/LICENSE-2.0 \r\n\r\n" +
                    "Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an \"AS IS\" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\r\n" +
                    "See the License for the specific language governing permissions and limitations under the License.";
            
        }

        private void ContactLL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ContactLL.Links[ContactLL.Links.IndexOf(e.Link)].Visited = true;
            string targetUrl = e.Link.LinkData as string;
            if (string.IsNullOrEmpty(targetUrl))
                System.Diagnostics.Debug.Write("No Link address found!");
            else
                System.Diagnostics.Process.Start(targetUrl);
        }

        private void LicenseRTB_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void GithubL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GithubL.Links[GithubL.Links.IndexOf(e.Link)].Visited = true;
            string targetUrl = e.Link.LinkData as string;
            if (string.IsNullOrEmpty(targetUrl))
                System.Diagnostics.Debug.Write("No Link address found!");
            else
                System.Diagnostics.Process.Start(targetUrl);
        }
    }
}
