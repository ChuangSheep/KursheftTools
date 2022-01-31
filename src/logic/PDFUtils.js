/* eslint-disable no-unused-vars */
const blobStream = require("blob-stream");

const WIDTH = 595.28;
const HEIGHT = 841.89;

function _xPercentToPx(percent) {
  if (!percent || percent < 0 || percent > 100)
    return 0;
  return percent / 100 * WIDTH;
}

function _yPercentToPx(percent) {
  if (!percent || percent < 0 || percent > 100)
    return 0;
  return percent / 100 * HEIGHT;
}

// row: [{day:String, date:String, note:String},{...},...]
function _getRows(holidays, terms, course) {
  const res = [];
  for (let i = 0; i < 70; i++) {
    res.push({ day: "Mo", date: "31.01.2022", note: "Note Note;" })
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


const X_MARGIN = _xPercentToPx(4);
const X_CONTENT_WIDTH = _xPercentToPx(40);
const X_MIDDLE = _xPercentToPx(100) - X_MARGIN * 2 - X_CONTENT_WIDTH * 2;
const X_DAY_WIDTH = 0.1 * X_CONTENT_WIDTH;
const X_DATE_WIDTH = 0.3 * X_CONTENT_WIDTH;
const X_NOTE_WIDTH = X_CONTENT_WIDTH - X_DAY_WIDTH - X_DATE_WIDTH;

const Y_MARGIN = _yPercentToPx(2);
const Y_BOX_HEIGHT = _yPercentToPx(1);
const Y_BOX_OFFSET = _yPercentToPx(0.15);

const COLOR1 = "#d2d2d2";
const COLOR2 = "#e6e6e6";
const COLORS = [COLOR1, COLOR2];
const BLACK = "#0"

const MAX_ROW = 70;

const HEADER = ["Tag", "Datum", "Besonderheiten"]

let xPointer = X_MARGIN, yPointer = Y_MARGIN;


/* eslint-disable no-undef */
let PDFUtils = {
  // eslint-disable-next-line no-unused-vars
  create(holidays, terms, courses, grades, onIncrement, onFinish) {

    const output = new PDFDocument({ autoFirstPage: false, paper: "a4" });
    const stream = output.pipe(blobStream());


    output.addPage({ margin: 0 });
    const row = _getRows(holidays, terms, courses[0]);
    const title = _getTitle(terms, courses[0]);

    if (row.length > MAX_ROW)
      console.warning(`Maximal row limitation ${MAX_ROW} exceeded. Actual: ${row.length}.`);

    // HEAD
    output.font("Times-Bold").fontSize(14).text(title[0], xPointer, yPointer);
    yPointer += _yPercentToPx(2.2);
    output.text(title[1], xPointer, yPointer);
    yPointer += _yPercentToPx(2.5);

    // MAIN TITLE
    output.fontSize(10);
    output.text(HEADER[0], xPointer, yPointer);
    xPointer += X_DAY_WIDTH;
    output.text(HEADER[1], xPointer, yPointer);
    xPointer += X_DATE_WIDTH;
    output.text(HEADER[2], xPointer, yPointer);
    xPointer = X_MARGIN;
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
      xPointer = X_MARGIN;
      yPointer += Y_BOX_HEIGHT;
    }

    output.end();

    stream.on("finish", function () {
      onFinish(stream.toBlob('application/pdf'));
    })
  },
}

export default PDFUtils;