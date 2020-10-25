import time
import os

classes = ["A", "B", "C", "D", "E", "F"]


fileName = input("Geben Sie bitte den Namen der Datei ein...\r\n")

with open(fileName, encoding='utf-8') as f:
    lines = f.readlines()

toDelete = []
currentClass = ""
currentInfo = ""
processed = False
for index, line in enumerate(lines):
    processed = False
    if "Q1" not in line and "Q2" not in line:
        if "EF" in line:
            for cls in classes:
                # Fuer Deutsch, Englisch oder Mathe
                # Nur speichern den Kurs der eigenen Klasse
                if ("\"d" + cls) in line or ("\"e" + cls) in line or ("\"m" + cls) in line:
                    if ("EF" + cls.lower()) not in line:
                        toDelete.append(line)
                        processed = True
                    break

        if not processed:
            # Ansonsten nehmen wir nur die EFa
            # aedert den Name zu EF und loescht die andere
            if currentInfo != line[line.find(';')+7:line.find(';')+17]:
                currentClass = line[line.find(';')+2:line.find(';')+5]
                currentInfo = line[line.find(';')+7:line.find(';')+17]
                # lines[index] = line[:line.find(';')+4] + line[line.find(';')+5:]
            elif currentInfo == line[line.find(';')+7:line.find(';')+17] and currentClass != line[line.find(';')+2:line.find(';')+5]:
                toDelete.append(line)
            elif currentInfo == line[line.find(';')+7:line.find(';')+17] and currentClass == line[line.find(';')+2:line.find(';')+5]:
                # Also immer noch derselbe Kurs
                pass
            # lines[index] = line[:line.find(';')+4] + line[line.find(';')+5:]


for i in range(0, len(toDelete)):
    lines.remove(toDelete[i])


newName = str(int(time.time()))[-4:] + "FIXED-" + fileName

with open(newName, "a", encoding="utf-8") as f:
    f.writelines(lines)

print(newName + " wurde erfolgreich unter " +
      os.path.dirname(os.path.realpath(__file__)) + " gespeichert. ")

os.system('pause')
