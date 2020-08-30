import time
import os 

classes = ["A", "B", "C", "D", "E", "F"]


fileName = input("Geben Sie bitte den Namen der Datei ein...\r\n")

with open(fileName) as f:
  lines = f.readlines()

toDelete = []
for line in lines:
  if "EF" in line:
      for cls in classes:
        if ("\"d" + cls) in line or ("\"e" + cls) in line or ("\"m" + cls) in line:
          if ("EF" + cls.lower()) not in line:
            toDelete.append(line)
          break

for i in range(0, len(toDelete)):
  lines.remove(toDelete[i])


newName = str(int(time.time()))[-4:] + "FIXED-" + fileName

with open(newName, "a") as f:
  f.writelines(lines)

print(newName + " wurde erfolgreich unter " + os.path.dirname(os.path.realpath(__file__)) + " gespeichert. ")

os.system('pause')