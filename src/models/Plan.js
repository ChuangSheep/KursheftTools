
class Plan {
  constructor(term1Begin, term1End, term2Begin, term2End) {
    this.term1Begin = term1Begin;
    this.term1End = term1End;
    this.term2Begin = term2Begin;
    this.term2End = term2End;

    // date (as string yyyymmdd) -> Array<Entry>
    this.entries = new Map();
    // [[holidayStart: Date, holidayEnd: Date]]
    this.holidays = [];
  }

  static fromJSON(json) {
    const obj = json;
    if (!(obj && obj.term1Begin && obj.term1End && obj.term2Begin &&
      obj.term2End && obj.holidays && obj.entries))
      throw new Error("Plan.fromJSON: invalid json" + json);

    let ins = new Plan(new Date(obj.term1Begin), new Date(obj.term1End),
      new Date(obj.term2Begin), new Date(obj.term2End));

    ins.holidays.forEach(v =>
      ins.setHoliday(-1, new Date(v[0]), new Date(v[1])));

    ins.entries = new Map(Object.entries(json.entries));

    return ins;
  }

  toJSON() {
    return {
      term1Begin: this.term1Begin,
      term1End: this.term1End,
      term2Begin: this.term2Begin,
      term2End: this.term2End,

      holidays: this.holidays,
      entries: Object.fromEntries(this.entries)
    }
  }

  setHoliday(id, holidayStart, holidayEnd) {
    if (this.holidays.length <= id)
      throw new Error("Plan.setHoliday: invalid id");

    if (id < 0) this.holidays.push([holidayStart, holidayEnd]);
    else this.holidays[id] = [holidayStart, holidayEnd];

    // now sort all holidays
    this.holidays.sort((a, b) => a[0] - b[0]);
  }

  removeHoliday(id) {
    this.holidays.splice(id, 1);
  }

  setTerms(term1Begin, term1End, term2Begin, term2End) {
    this.term1Begin = term1Begin;
    this.term1End = term1End;
    this.term2Begin = term2Begin;
    this.term2End = term2End;
  }
}

// {grades:[],content:""}

export { Plan };
