const DAY_MILI = 60 * 60 * 24 * 1000;

class Term {
  constructor(start, end) {
    this.termStart = new Date(start.getTime());
    this.termEnd = new Date(end.getTime());
    this.termStart.setHours(12);
    this.termEnd.setHours(12);
    this.days = [];
  }
  static fromJSON(json) {
    const obj = json;
    if (obj && obj.termStart && obj.termEnd && obj.days && obj.days.length) {
      let ins = new Term(new Date(obj.termStart), new Date(obj.termEnd));
      for (const d of obj.days) {
        ins.days.push(Daynote.fromJSON(d));
      }
      return ins;
    }
    else {
      console.error("Class Term: given json not valid\n" + json);
    }
  }

  toJSON() {
    let ds = [];
    for (const d of this.days) {
      ds.push(d.toJSON());
    }
    return { termStart: this.termStart, termEnd: this.termEnd, days: ds };
  }

  // Generate every day in the interval [termStart,termEnd]
  fillDays() {
    if (this.days.length > 0) return;
    for (let d = new Date(this.termStart.getTime()); d.getTime() <= this.termEnd.getTime();) {
      // no weekend
      if (d.getDay() != 6 && d.getDay() != 0)
        this.days.push(new Daynote(d));
      d = new Date(d.getTime() + DAY_MILI);
    }
  }

  modifyDates(start, end) {
    // if no change, don't modify
    if (Math.abs(this.termStart.getTime() - start.getTime()) < DAY_MILI && Math.abs(this.termEnd.getTime() - end.getTime()) < DAY_MILI)
      return;

    // update field variables
    this.termStart = new Date(start.getTime());
    this.termEnd = new Date(end.getTime());

    // no weekend
    while (start.getDay() == 6 || start.getDay() == 0)
      start = new Date(start.getTime() + DAY_MILI);
    while (end.getDay() == 6 || end.getDay() == 0)
      end = new Date(end.getTime() - DAY_MILI);
    if (end.getTime() < start.getTime()) {
      console.error("Term.modifyDates: should be checked somewhere else but Im lazy to implement. Shouldn't be a problem for any expected input though");
      return;
    }

    let temp = new Date(this.days[0].date.getTime());
    // remove days before start
    while (this.days.length > 0 && this.days[0].date.getTime() - start.getTime() < 0) {
      this.days.shift();
    }

    if (this.days.length == 0) {
      this.days.unshift(new Daynote(new Date(end.getTime())));
    }
    // add days until start
    while (this.days[0].date.getTime() - start.getTime() > DAY_MILI) {
      temp = new Date(this.days[0].date.getTime() - DAY_MILI);
      while (temp.getDay() == 6 || temp.getDay() == 0)
        temp = new Date(temp.getTime() - DAY_MILI);
      if (temp.getTime() - start.getTime() > DAY_MILI)
        this.days.unshift(new Daynote(new Date(temp.getTime())));
      else break;
    }

    // remove days after end
    while (this.days.length > 0 && this.days[this.days.length - 1].date.getTime() - end.getTime() > DAY_MILI) {
      this.days.pop();
    }

    if (this.days.length == 0) {
      this.days.push(new Daynote(new Date(start.getTime())));
    }
    // add days before end
    while (this.days[this.days.length - 1].date.getTime() - end.getTime() < DAY_MILI) {
      temp = new Date(this.days[this.days.length - 1].date.getTime() + DAY_MILI);
      while (temp.getDay() == 6 || temp.getDay() == 0)
        temp = new Date(temp.getTime() + DAY_MILI);
      if (temp.getTime() - end.getTime() < DAY_MILI)
        this.days.push(new Daynote(new Date(temp.getTime())));
      else break;
    }
  }
}

class Daynote {
  constructor(date) {
    this.date = new Date(date.getTime());
    this.date.setHours(12);
    this.notes = [];
  }
  static fromJSON(json) {
    const obj = json;
    if (obj && obj.date && obj.notes) {
      let ins = new Daynote(new Date(obj.date));
      ins.notes = obj.notes;
      return ins;
    }
    else {
      console.error("Class Daynote: given json not valid\n" + json);
    }
  }

  toJSON() {
    return {
      date: this.date,
      notes: this.notes
    };
  }
}

export { Term, Daynote };
