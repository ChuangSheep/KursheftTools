<template>
  <v-container>
    <v-row>
      <!-- Create and edit the board -->
      <create-noteboard-dialog
        :hasData="hasData"
        :initDates="dates"
        @create="onNewBoardCreate"
        @update="onBoardUpdate"
        @delete="onBoardDelete"
      />

      <!-- Import new board -->
      <comfirmation-dialog
        v-if="hasData"
        maxWidth="500px"
        yesText="Ja, Änderungen verwerfen"
        activatorText="Importieren"
        @accept="$refs.import.click()"
        ><template slot="content">
          <v-card-text class="black--text"
            >Wenn Sie einen anderen Bemerkungsbogen importieren,
            <b>verlieren Sie hier alle Daten</b>, die Sie nicht
            gespeichert/exportiert haben.
          </v-card-text>
          <v-card-text class="black--text" style="margin-top: -10px">
            Sind Sie sicher?
          </v-card-text>
        </template></comfirmation-dialog
      >
      <v-btn class="mr-6" @click="$refs.import.click()" v-else
        >Importieren</v-btn
      >
      <input
        id="noteboard-file"
        v-show="false"
        ref="import"
        type="file"
        @change="onFileImport"
      />

      <!-- Export the board -->
      <v-btn @click="onFileExport" v-if="hasData">Exportieren</v-btn>
      <v-spacer></v-spacer>

      <!-- To next step -->
      <v-btn color="primary" :disabled="!hasData" @click="onNextClick"
        >Weiter</v-btn
      >
    </v-row>

    <!-- when no data: skeleton -->
    <v-row justify="space-between" class="mt-10" v-if="!hasData">
      <v-col cols="12" sm="6" v-for="i in 2" :key="i">
        <v-subheader class="text-h5 font-weiht-bold">
          {{ i }}. Abschnitt
        </v-subheader>
        <v-skeleton-loader
          class="mx-4 mb-3"
          type="text"
          style="width: 250px"
          boilerplate
        ></v-skeleton-loader>
        <v-skeleton-loader
          class="mx-4 gradient-out"
          type="image"
          boilerplate
        ></v-skeleton-loader>
      </v-col>
      <v-col cols="12" class="text-center" style="margin-top: -100px"
        ><span class="text-h5 grey--text font-weight-black text--darken-2"
          >Bemerkungsbogen neu erstellen oder von einer Datei importieren</span
        ></v-col
      >
    </v-row>

    <!-- Otherwise, show the data view -->
    <v-row justify="space-between" class="mt-10" v-else>
      <v-col cols="12" sm="6" v-for="(data, i) in terms" :key="i">
        <v-list>
          <v-subheader class="text-h5 font-weight-bold black--text">
            {{ i + 1 }}. Abschnitt
          </v-subheader>
          <v-subheader style="margin-top: -15px"
            >{{ dateUtils.toGermanDateFormat(data.termBegin) }} -
            {{ dateUtils.toGermanDateFormat(data.termEnd) }}</v-subheader
          >
          <v-responsive
            min-height="100"
            class="fill-height"
            color="transparent"
            v-for="day in data.days"
            :key="getDayUID(day)"
          >
            <v-lazy
              :options="{
                threshold: 0.5,
              }"
              transition="fade-transition"
            >
              <v-list-item fluid>
                <daynote-entry
                  :date="day.date"
                  :initNotes="day.entries"
                  :allowedGrades="allowedGrades"
                  @update="(args) => onNoteUpdate(day.date, args)"
                  style="width: 100%"
                />
              </v-list-item>
            </v-lazy>
          </v-responsive>
        </v-list>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import DateUtils from "@/logic/DateUtils.js";
import { Plan } from "@/models/Plan";

import CreateNoteboardDialog from "./widgets/CreateNoteboardDialog.vue";
import ComfirmationDialog from "./widgets/ComfirmationDialog.vue";
import DaynoteEntry from "./DaynoteEntry.vue";

export default {
  name: "Noteboard",
  components: {
    DaynoteEntry,
    CreateNoteboardDialog,
    ComfirmationDialog,
  },
  props: {
    allowedGrades: {
      default() {
        return ["Alle", "EF", "Q1", "Q2"];
      },
      type: Array,
    },
  },
  data() {
    return {
      dateUtils: DateUtils,
      hasData: false,

      plan: null,
    };
  },
  methods: {
    async onFileImport() {
      let file = this.$refs.import.files[0];
      if (file) {
        const fileText = await file.text();
        const data = JSON.parse(fileText);

        // DEBUG: does it work?
        this.plan = Plan.fromJSON(data);

        this.hasData = true;

        this.updateStorage();
      }
      // must play with dom to remove the pending file
      document.getElementById("noteboard-file").value = "";
    },
    onFileExport() {
      const data = JSON.stringify(this.generateFullData());
      const filename = `Bemerkungsbogen-${
        this.plan.term1Begin.getYear() + 1900
      }-${this.plan.term1Begin.getMonth() + 1}`;
      const blob = new Blob([data], { type: "application/json" });
      if (window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveBlob(blob, filename);
      } else {
        const elem = window.document.createElement("a");
        elem.href = window.URL.createObjectURL(blob, { oneTimeOnly: true });
        elem.download = filename;
        elem.style.display = "none";
        document.body.appendChild(elem);
        elem.click();
        document.body.removeChild(elem);
      }
    },
    onNewBoardCreate(dates) {
      const np = new Plan(
        new Date(dates[0].start),
        new Date(dates[0].end),
        new Date(dates[1].start),
        new Date(dates[1].end)
      );

      for (let i = 2; i < dates.length; i++) {
        const e = dates[i];
        np.setHoliday(-1, new Date(e.start), new Date(e.end));
      }

      this.plan = np;
      this.hasData = true;
      this.updateStorage();
    },
    onBoardUpdate(dates) {
      this.plan.setTerms(
        new Date(dates[0].start),
        new Date(dates[0].end),
        new Date(dates[1].start),
        new Date(dates[1].end)
      );

      this.plan.holidays = [];
      for (let i = 2; i < dates.length; i++) {
        const e = dates[i];
        this.plan.setHoliday(-1, new Date(e.start), new Date(e.end));
      }

      this.plan = Plan.fromJSON(JSON.parse(JSON.stringify(this.plan.toJSON())));

      this.updateStorage();
    },
    onBoardDelete() {
      this.hasData = false;
      this.plan = null;
      this.updateStorage();
    },
    onNoteUpdate(date, notes) {
      this.plan.entries.set(DateUtils.toNormalString(date), notes);
      this.updateStorage();
    },
    onNextClick() {
      this.$emit("next");
    },
    generateFullData() {
      return this.plan.toJSON();
    },
    fetchSavedBoard() {
      this.hasData = false;
      const hasData = localStorage.getItem("kht.noteboard.hasData");
      if (hasData !== "true") {
        return false;
      }

      const res = localStorage.getItem("kht.noteboard");
      if (res !== null) {
        const data = JSON.parse(res);
        this.plan = Plan.fromJSON(data);
        this.hasData = true;
        return true;
      } else return false;
    },
    updateStorage() {
      localStorage.setItem("kht.noteboard.hasData", this.hasData);
      if (!this.hasData) {
        localStorage.removeItem("kht.noteboard");
        return;
      }
      localStorage.setItem(
        "kht.noteboard",
        JSON.stringify(this.generateFullData())
      );
    },
    getDayUID(day) {
      return `${DateUtils.toNormalString(day.date)}_${day.entries.map(e => e.content).join("-")}`;
    }
  },
  computed: {
    dates() {
      if (!this.plan) return null;

      const res = [
        {
          start: DateUtils.toNormalString(this.plan.term1Begin),
          end: DateUtils.toNormalString(this.plan.term1End),
        },
        {
          start: DateUtils.toNormalString(this.plan.term2Begin),
          end: DateUtils.toNormalString(this.plan.term2End),
        },
      ];

      for (let i = 0; i < this.plan.holidays.length; i++) {
        const e = this.plan.holidays[i];
        res.push({
          start: DateUtils.toNormalString(e[0]),
          end: DateUtils.toNormalString(e[1]),
        });
      }
      return res;
    },
    terms() {
      if (!this.hasData || !this.plan) return [];

      const rtr = [
        {
          termBegin: this.plan.term1Begin,
          termEnd: this.plan.term1End,
          days: [],
        },
        {
          termBegin: this.plan.term2Begin,
          termEnd: this.plan.term2End,
          days: [],
        },
      ];

      for (let i = 0; i < 2; i++) {
        for (
          let d = new Date(rtr[i].termBegin);
          d <= rtr[i].termEnd;
          d.setDate(d.getDate() + 1)
        ) {
          // not on weekend
          if (d.getDay() == 0 || d.getDay() == 6) continue;
          // not in holiday
          if (
            this.plan.holidays.some((h) =>
              DateUtils.isDateBetween(d, h[0], h[1])
            )
          )
            continue;

          const day = {
            date: new Date(d),
            entries: this.plan.entries.get(DateUtils.toNormalString(d)) || [],
          };
          rtr[i].days.push(day);
        }
      }

      return rtr;
    },
  },
  // watch: {
  //   terms: {
  //     handler() {
  //       this.updateStorage();
  //     },
  //   },
  // },
  mounted() {
    this.fetchSavedBoard();
  },
};
</script>

<style>
.gradient-out {
  mix-blend-mode: hard-light;
}

.gradient-out::after {
  position: absolute;
  content: "";
  left: 0px;
  top: 0px;
  height: 100%;
  width: 100%;
  background: linear-gradient(transparent, gray);
  pointer-events: none;
}
</style>
