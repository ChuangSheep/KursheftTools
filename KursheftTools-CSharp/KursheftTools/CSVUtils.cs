using Microsoft.VisualBasic.FileIO;
using System.Data;


namespace KursheftTools
{
    public static class CSVUtils
    {
        /// <summary>
        /// Import a csv file as a Datatable
        /// </summary>
        /// <param name="path">String: where the csv file is stored</param>
        /// <param name="datatableName">String: the name of this datatable</param>
        /// <param name="hasTitle">Bool: whether this datatable has a name or not</param>
        /// <param name="columnNames">if hasTitle=false, then the titles of the row should be provided as a string array here</param>
        /// <returns>a datatable containing all data form that csv file</returns>
        public static DataTable ImportCSVasDT(string path, string datatableName = null, bool hasTitle = true, string[] columnNames = null)
        {
            DataTable datatableCSV = new DataTable(datatableName);
            //The encodeing is set to Windows 1252: Western European
            using (TextFieldParser csvParser = new TextFieldParser(path, System.Text.Encoding.GetEncoding(1252)))
            {
                csvParser.SetDelimiters(";");
                csvParser.HasFieldsEnclosedInQuotes = true;

                if (hasTitle)
                {
                    string[] titles = csvParser.ReadFields();
                    foreach (string title in titles)
                    {
                        DataColumn dCl = new DataColumn(title, typeof(string));
                        datatableCSV.Columns.Add(dCl);
                    }
                }

                else
                {
                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        DataColumn dCl = new DataColumn(columnNames[i], typeof(string));
                        datatableCSV.Columns.Add(dCl);
                    }
                }

                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    datatableCSV.Rows.Add(fields);
                }

                csvParser.Close();
            };


            return datatableCSV;
        }
    }
}
