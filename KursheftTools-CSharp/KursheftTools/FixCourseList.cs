using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursheftTools
{
    static class FixCourseList
    {
        public static string Fix(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            List<string> toDelete = new List<string>();

            string[] CLASSES = new string[] { "A", "B", "C", "D", "E", "F" };

            {
                int i = 0;
                string currentClass = "", currentInfo = "";
                bool processed = false;
                for (; i < lines.Length; i++, processed = false)
                {
                    if (lines[i].IndexOf("Q1") == -1 && lines[i].IndexOf("Q2") == -1)
                    {
                        if (lines[i].IndexOf("EF") != -1)
                        {
                            foreach (string cls in CLASSES)
                            {
                                if (lines[i].IndexOf(("\"d" + cls)) != -1 ||
                                  lines[i].IndexOf(("\"e" + cls)) != -1 ||
                                  lines[i].IndexOf(("\"m" + cls)) != -1)
                                {
                                    if (lines[i].IndexOf("EF" + cls.ToLower()) == -1)
                                    {
                                        toDelete.Add(lines[i]);
                                        processed = true;
                                    }
                                    break;
                                }
                            }
                        }

                        if (!processed)
                        {
                            if (currentInfo != lines[i].Substring(lines[i].IndexOf(";") + 7, 10))
                            {
                                currentClass = lines[i].Substring(lines[i].IndexOf(";") + 2, 3);
                                currentInfo = lines[i].Substring(lines[i].IndexOf(";") + 7, 10);
                            }
                            else if (currentInfo == lines[i].Substring(lines[i].IndexOf(";") + 7, 10) && currentClass != lines[i].Substring(lines[i].IndexOf(";") + 2, 3))
                            {
                                toDelete.Add(lines[i]);
                            }
                        }
                    }
                }
            }

            return String.Join("\r\n", lines.Except(toDelete));
        }
    }
}
