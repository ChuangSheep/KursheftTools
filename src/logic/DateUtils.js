let DateUtils = {
  toWeekdayAndFullDate(date) {
    return date.toLocaleDateString("de-de", {
      weekday: "short",
      year: "numeric",
      month: "long",
      day: "2-digit",
    });
  },

  toGermanDateFormat(date) {
    return date.toLocaleDateString("de-de", {
      year: "numeric",
      month: "long",
      day: "2-digit",
    });
  },

  toNormalString(date) {
    return `${date.getFullYear()}-${date.getMonth() < 9 ? "0" : ""}${date.getMonth() + 1}-${date.getDate() < 10 ? "0" : ""}${date.getDate()}`
  },

  isDateBetween(date, from, to) {
    return date.getFullYear() >= from.getFullYear() &&
      date.getMonth() >= from.getMonth() && date.getDate() >= from.getDate() &&
      date.getFullYear() <= to.getFullYear() && date.getMonth() <= to.getMonth() &&
      date.getDate() <= to.getDate();
  },

  validateCourseDates(dates) {
    let res = [];
    for (let i = 0; i < dates.length; i++) {
      res.push({ start: true, end: true });
    }

    // rule 1: all dates must be filled
    for (let i = 0; i < dates.length; i++) {
      const entry = dates[i];
      if (!entry.start || !entry.start.trim())
        res[i].start = "erforderlich";
      if (!entry.end || !entry.end.trim()) {
        res[i].end = "erforderlich";
        console.log(i);
      }
    }

    // rule 2: end dates must be later than start dates
    for (let i = 0; i < dates.length; i++) {
      const entry = dates[i];
      const start = new Date(entry.start);
      const end = new Date(entry.end);
      if (start.getTime() >= end.getTime()) {
        if (res[i].start === true)
          res[i].start = "";
        res[i].end = "ungültiges Datum (zu klein)";
      }
    }

    // rule 3: 2. date range must be later than 1. date range for the terms
    const end1 = new Date(dates[0].end);
    const start2 = new Date(dates[1].start);
    if (end1.getTime() >= start2.getTime()) {
      if (res[0].end === true)
        res[0].end = "";
      res[1].start = "ungültiges Datum (zu klein)";
    }

    return res;
  }
}

export default DateUtils;
