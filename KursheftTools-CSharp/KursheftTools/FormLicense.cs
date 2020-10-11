using System;
using System.Windows.Forms;

namespace KursheftTools
{
    public partial class FormLicense : Form
    {
        public FormLicense()
        {
            InitializeComponent();
        }

        private void FormLicenses_Load(object sender, EventArgs e)
        {
            const string MITLICENSE = "MIT License\r\n\r\n" +
            "Copyright(c) 2005 - 2018 empira Software GmbH, Troisdorf(Germany)\r\n\r\n" +
            "Permission is hereby granted, free of charge, to any person obtaining a copy " +
            "of this software and associated documentation files(the \"Software\"), to deal " +
            "in the Software without restriction, including without limitation the rights " +
            "to use, copy, modify, merge, publish, distribute, sublicense, and/ or sell " +
            "copies of the Software, and to permit persons to whom the Software is " +
            "furnished to do so, subject to the following conditions:\r\n\r\n" +

            "The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.\r\n\r\n" +

             "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR " +
            "IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, " +
            "FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE " +
            "AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER " +
            "OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE " +
            "SOFTWARE.";

            const string THIRDPARTY = "THE FOLLOWING LICENCES ARE FOR THE BUNDLED THIRD PARTY SOFTWARE(s). FOR THE LICENSE FOR THIS OFFICE ADDIN, SEE \"INFO\". \r\n\r\n" +
                "THE FOLLOWING NOTICES ARE PROVIDED FOR THIRD PARTY SOFTWARE THAT MAY BE CONTAINED IN PORTIONS OF THIS OFFICE ADDIN\r\n\r\n-----\r\n\r\n";
            const string INCLUDED = "The following software may be included in this product: \r\n";
            const string NOTICE = "This software contains the following license and notice bellow:\r\n\r\n";
            const string SOURCECODE = "The source code is available at ";

            this.licenseTB.Text = THIRDPARTY + INCLUDED + "PDFsharp. \r\n" + SOURCECODE + "https://github.com/empira/PDFsharp. " + NOTICE + MITLICENSE;

        }

        private void licenseTB_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }
}
