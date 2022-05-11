/* eslint-disable no-unused-vars */
const blobStream = require("blob-stream");

const WIDTH = 595.28;
const HEIGHT = 841.89;

const DAY_MILI = 24 * 60 * 60 * 1000;

function _xPercentToPx(percent) {
  if (!percent || percent < 0 || percent > 100)
    return 0;
  return percent / 100.0 * WIDTH;
}

function _yPercentToPx(percent) {
  if (!percent || percent < 0 || percent > 100)
    return 0;
  return percent / 100.0 * HEIGHT;
}

// row: [{day:String, date:String, note:String},{...},...]
function _getRows(holidays, terms, course) {
  const getWeekNo = (date) => {
    // Copy date so don't modify original
    date = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
    const originYear = date.getFullYear();
    // Set to nearest Thursday: current date + 4 - current day number
    // Make Sunday's day number 7
    date.setUTCDate(date.getUTCDate() + 4 - (date.getUTCDay() || 7));
    // Get first day of year
    const yearStart = new Date(Date.UTC(date.getUTCFullYear(), 0, 1));
    // Calculate full weeks to nearest Thursday
    const weekNo = Math.ceil((((date - yearStart) / 86400000) + 1) / 7);
    // Return array of year and week number
    if (date.getUTCFullYear() < originYear) return 1;
    else return weekNo;
  }

  const hasCourseOn = (date) => {
    for (const entry of course.days) {
      if (entry[0] == date.getDay()) {
        if (entry[1] === "g" && getWeekNo(date) % 2 != 0) {
          return false;
        }
        else if (entry[1] === "u" && getWeekNo(date) % 2 != 1) {
          return false;
        }
        return true;
      }
    }
    return false;
  }

  const getAllNotes = (allNotes) => {
    let res = "";
    for (const note of allNotes) {
      if (note.grade.includes(course.class) || note.grade.includes("Alle"))
        res += `${note.content}; `;
    }
    return res;
  }

  const res = [];

  for (let i = 0; i < 2; i++) {
    for (const entry of terms[i].days) {
      if (hasCourseOn(entry.date)) {
        res.push({
          day: entry.date.toLocaleDateString("de-de", {
            weekday: "short"
          }),
          date: _getPointDateString(entry.date),
          note: getAllNotes(entry.notes)
        })
      }
    }
  }

  return res;
}

function _getHalfYear(date) {
  if (date.getMonth() > 5) return 1;
  else return 2;
}

function _getTitleYear(date) {
  const year = date.getFullYear();
  if (_getHalfYear(date) == 1)
    return `${year}/${year + 1}`;
  else
    return `${year - 1}/${year}`;
}


// return: ["Jahrgangstufe Q1.2 ..","Sollstunde ..."]
function _getTitle(terms, course) {
  const res = [];

  const grade = ["EF", "Q1", "Q2"].includes(course.class) ?
    course.class :
    course.class.substring(0, course.class.length - 1).replace(/^0+/, '');

  res.push(`Jahrgangstufe ${grade}.${_getHalfYear(terms[0].termStart)}    ${_getHalfYear(terms[0].termStart)}. Halbjahr ${_getTitleYear(terms[0].termStart)}`);

  res.push(`Sollstunde für Kurs  ${course.subject}-${course.teacher}-${course.class}`);

  return res;
}

function _getPointDateString(date) {
  return `${date.getDate() < 10 ? "0" : ""}${date.getDate()}.${date.getMonth() < 9 ? "0" : ""}${date.getMonth() + 1}.${date.getFullYear()}`;
}


const X_MARGIN = _xPercentToPx(4);
const X_CONTENT_WIDTH = _xPercentToPx(43);
const X_MIDDLE = _xPercentToPx(100) - X_MARGIN * 2 - X_CONTENT_WIDTH * 2;
const X_DAY_WIDTH = 0.1 * X_CONTENT_WIDTH;
const X_DATE_WIDTH = 0.2 * X_CONTENT_WIDTH;
// const X_NOTE_WIDTH = X_CONTENT_WIDTH - X_DAY_WIDTH - X_DATE_WIDTH;

const Y_MARGIN = _yPercentToPx(1.5);
const Y_BOX_HEIGHT = _yPercentToPx(1.25);
const Y_BOX_OFFSET = _yPercentToPx(0.3);

const COLOR1 = "#d2d2d2";
const COLOR2 = "#e6e6e6";
const COLORS = [COLOR1, COLOR2];
const BLACK = "#0"

const MAX_ROW = 65;

const HEADER = ["Tag", "Datum", "Besonderheiten"]

const HINWEIS = "Alle Termine dieser Liste müssen in der Kursmappe eingetragen sein, auch die unterrichtsfreien Tage. Alle Termine(außer den Ferien) müssen durch ihre Paraphe bestätigt werden. Tragen Sie bitte auch die Fehlstundenzahl sowie die Soll - Ist - Stunden(Kursheft S.5) ein. \nHinweis: Schüler, die aus schulischen Gründen den Unterricht versäumt haben(Klausur, schul.Veranstaltung usw.) müssen im Kursheft aufgeführt werden. Diese Stunden dürfen auf dem Zeugnis aber nicht als Fehlstunden vermerkt werden."


// https://stackoverflow.com/questions/35725594/how-do-i-pass-data-like-a-user-id-to-a-web-worker-for-fetching-additional-push
// Consider Service Worker
/* eslint-disable no-undef */
let PDFUtils = {
  // eslint-disable-next-line no-unused-vars
  async create(plan, courses, onIncrement, onFinish) {

    // Create the document
    const output = new PDFDocument({ autoFirstPage: false, size: "A4" });
    const stream = output.pipe(blobStream());

    // General information
    const courseTerms = [`${_getPointDateString(terms[0].termStart)} - ${_getPointDateString(terms[0].termEnd)}`,
    `${_getPointDateString(terms[1].termStart)} - ${_getPointDateString(terms[1].termEnd)}`];


    // Loop through all exports
    for (const course of courses) {
      output.addPage({ margin: 0, size: "A4" });
      const row = _getRows(holidays, terms, course);
      const title = _getTitle(terms, course);

      if (row.length > MAX_ROW)
        console.warn(`Maximal row limitation ${MAX_ROW} exceeded. Actual: ${row.length}.`);

      for (let col = 0; col < 2; col++) {
        let xInitial = col == 0
          ? X_MARGIN :
          X_MARGIN + X_CONTENT_WIDTH + X_MIDDLE;

        let xPointer = xInitial;
        let yPointer = Y_MARGIN;

        // HEAD
        output.font("Times-Bold").fontSize(14).text(title[0], xPointer, yPointer);
        yPointer += _yPercentToPx(2.2);
        output.text(title[1], xPointer, yPointer);
        yPointer += _yPercentToPx(2.8);

        // MAIN TITLE
        output.fontSize(10);
        output.text(HEADER[0], xPointer, yPointer);
        xPointer += X_DAY_WIDTH;
        output.text(HEADER[1], xPointer, yPointer);
        xPointer += X_DATE_WIDTH;
        output.text(HEADER[2], xPointer, yPointer);
        xPointer = xInitial;
        yPointer += Y_BOX_HEIGHT + Y_BOX_OFFSET * 2;

        // MAIN
        output.fontSize(8);
        output.font("Times-Roman");
        for (let i = 0; i < row.length; i++) {
          const r = row[i];
          output
            .rect(xPointer, yPointer - Y_BOX_OFFSET, X_CONTENT_WIDTH, Y_BOX_HEIGHT)
            .fill(COLORS[i % 2]);
          output.fillColor(BLACK).text(r.day, xPointer, yPointer);
          xPointer += X_DAY_WIDTH;
          output.text(r.date, xPointer, yPointer);
          xPointer += X_DATE_WIDTH;
          output.text(r.note, xPointer, yPointer);
          xPointer = xInitial;
          yPointer += Y_BOX_HEIGHT;
        }

        // FOOTER 
        if (col == 0) {
          yPointer += Y_BOX_HEIGHT * 0.3;
          output.text(`1. Kursabschnitt: ${courseTerms[0]}`, xPointer, yPointer);
          yPointer += Y_BOX_HEIGHT;
          output.text(`2. Kursabschnitt: ${courseTerms[1]}`, xPointer, yPointer);

          yPointer += Y_BOX_HEIGHT*1.1;
          output.fontSize(7);
          output.text(HINWEIS, xPointer, yPointer,
            {
              width: X_CONTENT_WIDTH,
              height: HEIGHT - yPointer,
              align: "justify"
            });
        }
      }

      onIncrement();
      // TODO: service worker to enable multi threading
      await new Promise(r => setTimeout(r, 25));
    }


    // end the write stream
    output.end();

    // pass the pdf as blob to the callback 
    stream.on("finish", function () {
      onFinish(stream.toBlob('application/pdf'));
    })
  },
}

export default PDFUtils;
