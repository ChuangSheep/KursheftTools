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

            // Lass ruhig A bis Z als moegliche Klassen
            char[] CLASSES = Enumerable.Range(0, 26).Select(i => (char)(i+'A')).ToArray();

            {
                int i = 0;
                string currentClass = "", currentTeacher = "", currentSubject = "";
                bool processed = false;
                for (; i < lines.Length; i++, processed = false)
                {
                    // Q1 und Q2 sind bereit richtig
                    if (lines[i].IndexOf("Q1") == -1 && lines[i].IndexOf("Q2") == -1)
                    {
                        // cols: KursNum | Stufe | Lehrer | Fach | Raum | Tag | Stunde | Unused
                        string[] lineInfos = lines[i].Split(';').Select(s => new string((from c in s.Trim()
                                                                                         where c != '"'
                                                                                         select c).ToArray())).ToArray();

                        // Spezial Fall fuer EF in Englisch, Mathe und Deutsch
                        if (lineInfos[1] != "EF")
                        {
                            foreach (char cls in CLASSES)
                            {
                                // Fuer Deutsch, Englisch oder Mathe
                                // Nur speichern den Kurs der eigenen Klasse
                                if (lineInfos[3].Length == 2 && new List<string>() { "d"+cls, "e"+cls, "m"+cls }.Contains(lineInfos[3]))
                                {
                                    // Ist es nun nicht die eigene Klasse
                                    if (lineInfos[1] != ("EF" + cls.ToString().ToLower()))
                                    {
                                        toDelete.Add(lines[i]);
                                        processed = true;
                                    }
                                    break;
                                }
                            }
                        }
                        // Entfernen die Zeilen, die mit einer Stufe EF stehen
                        // Falls diese Eigenschaft stabil ist,
                        // Kann man den obigen Code abschaffen
                        else
                        {
                            toDelete.Add(lines[i]);
                            processed = true;
                        }

                        // Ausser D, M oder E
                        if (!processed)
                        {
                            // Fuer ungueltige Data
                            if (string.IsNullOrWhiteSpace(lineInfos[3]))
                            {
                                toDelete.Add(lines[i]);
                                System.Diagnostics.Debug.WriteLine($"Ignored invalid Line. Deleted. \"{lines[i]}\"");
                                continue;
                            }
                            // Ansonsten nehmen wir nur die EFa
                            // aedert den Name zu EF und loescht die andere
                            if (currentTeacher != lineInfos[2] || currentSubject != lineInfos[3])
                            {
                                currentClass = lineInfos[1];
                                currentSubject = lineInfos[3];
                                currentTeacher = lineInfos[2];
                            }
                            // Derselbe Kurs, duplikate Data
                            else if (currentTeacher == lineInfos[2] && currentSubject == lineInfos[3] && currentClass != lineInfos[1])
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
