import papaparse from 'papaparse';

let CSVUtils = {
  // return: a Map object <grade as string => Map<course identifer as
  // string => entries as Array of Object>>. 
  toGradeLists(file, vm) {
    const onComplete = (res) => {
      // console.log(res);
      const map = new Map();

      for (let line of res.data) {
        if (line[1] && line[1].includes("EF"))
          line[1] = "EF";

        // make line into object
        line = {
          class: line[1],
          teacher: line[2],
          subject: line[3],
          day: line[5],
          hour: line[6]
        }

        // if the grade exists
        if (map.has(line.class)) {
          let grade = map.get(line.class);
          // if the course exists in the grade/class
          if (grade.has(line.subject) && !JSON.stringify(grade.get(line.subject)).includes(JSON.stringify(line))) {
            grade.get(line.subject).push(line);
            // otherwise, new course in the grade/class
          } else {
            grade.set(line.subject, [line]);
          }
        }
        // otherwise, new grade with new course
        else {
          map.set(line.class, new Map());
          map.get(line.class).set(line.subject, [line]);
        }
      }

      const courses = [];
      for (const grade of map) {
        for (const cls of grade[1]) {
          let days = [];
          for (const entry of cls[1]) {
            // find index of existing day
            let index = -1;
            for (let i = 0; i < days.length; i++) {
              const day = days[i][0];
              if (day == entry.day)
                index = i;
            }

            if (index >= 0) {
              if (Number.parseInt(entry.hour) == 8) {
                if (days[index][1] == "u")
                  days[index][1] = "";
              }
              else if (Number.parseInt(entry.hour) == 9) {
                if (days[index][1] == "g")
                  days[index][1] = "";
              }
            }
            else {
              if (Number.parseInt(entry.hour) == 8)
                days.push([entry.day, "g"]);
              else if (Number.parseInt(entry.hour) == 9)
                days.push([entry.day, "u"]);
              else days.push([entry.day, ""]);
            }
          }
          const currentCourse = {
            class: cls[1][0].class,
            teacher: cls[1][0].teacher,
            subject: cls[1][0].subject,
            day: days,
          };
          if (currentCourse.subject)
            courses.push(currentCourse);
        }
      }

      vm.list = map;
      vm.condensedList = courses;
    }
    const onError = (err) => {
      console.error(err);
      vm.hasData = false;
    }
    papaparse.parse(file, { complete: onComplete, error: onError })
  }
}

export default CSVUtils;
