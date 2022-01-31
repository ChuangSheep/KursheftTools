<template>
  <v-container>
    <v-row justify="center">
      <!-- Create and edit the board -->
      <create-noteboard-dialog
        :hasData="hasData"
        :initDates="dates"
        @create="onNewBoardCreate"
        @update="onNewBoardUpdate"
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
            <b>verlieren Sie hier alle Daten</b>, die Sie nicht gespeichert
            haben.
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
            >{{ dateUtils.toGermanDateFormat(data.termStart) }} -
            {{ dateUtils.toGermanDateFormat(data.termEnd) }}</v-subheader
          >
          <v-list-item v-for="(entry, j) in data.days" :key="j" fluid>
            <!-- Don't include weekend -->
            <daynote-entry
              :date="entry.date"
              :initNotes="entry.notes"
              @update="onNoteUpdate(j)"
              style="width: 100%"
            />
          </v-list-item>
        </v-list>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import DateUtils from "@/logic/DateUtils.js";
import { Term } from "@/models/Term";

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
  data() {
    return {
      dateUtils: DateUtils,
      hasData: false,
      terms: [],
      holidays: [],
    };
  },
  methods: {
    async onFileImport() {
      let file = this.$refs.import.files[0];
      if (file) {
        const fileText = await file.text();
        const data = JSON.parse(fileText);
        for (let i = 0; i < data.holidays.length; i++) {
          const entry = data.holidays[i];
          this.$set(this.holidays, i, {
            start: new Date(entry.start),
            end: new Date(entry.end),
          });
        }
        for (let i = 0; i < data.terms.length; i++) {
          const entry = data.terms[i];
          this.$set(this.terms, i, Term.fromJSON(entry));
        }
        this.hasData = true;
      }
      // must play with dom to remove the pending file
      document.getElementById("noteboard-file").value = "";
    },
    onFileExport() {
      const data = JSON.stringify(this.generateFullData());
      const filename = `Bemerkungsbogen-${
        this.terms[0].termStart.getYear() + 1900
      }-${this.terms[0].termStart.getMonth() + 1}`;
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
      this.terms = [
        new Term(new Date(dates[0].start), new Date(dates[0].end)),
        new Term(new Date(dates[1].start), new Date(dates[1].end)),
      ];
      for (const t of this.terms) {
        t.fillDays();
      }

      this.holidays = [
        {
          start: new Date(dates[2].start),
          end: new Date(dates[2].end),
        },
        {
          start: new Date(dates[3].start),
          end: new Date(dates[3].end),
        },
      ];
      for (let i = 0; i < this.holidays.length; i++) {
        this.holidays[i].start.setHours(12);
        this.holidays[i].end.setHours(12);
      }

      this.hasData = true;
      console.log(dates);
    },
    onNewBoardUpdate(dates) {
      this.terms[0].modifyDates(
        new Date(dates[0].start),
        new Date(dates[0].end)
      );
      this.terms[1].modifyDates(
        new Date(dates[1].start),
        new Date(dates[1].end)
      );

      this.holidays = [
        {
          start: new Date(dates[2].start),
          end: new Date(dates[2].end),
        },
        {
          start: new Date(dates[3].start),
          end: new Date(dates[3].end),
        },
      ];
      for (let i = 0; i < this.holidays.length; i++) {
        this.holidays[i].start.setHours(12);
        this.holidays[i].end.setHours(12);
      }

      this.updateStorage();
    },
    onBoardDelete() {
      this.hasData = false;
      this.holidays = [];
      this.terms = [];
    },
    onNoteUpdate(i) {
      console.log(i);
      this.updateStorage();
    },
    onNextClick() {
      this.$emit("next");
    },
    generateFullData() {
      return { holidays: this.holidays, terms: this.terms };
    },
    fetchSavedBoard() {
      const hasData = localStorage.getItem("kht.hasData");
      if (hasData !== "true") {
        this.hasData = false;
        return false;
      }

      const res = localStorage.getItem("kht.noteboard");
      if (res !== null) {
        const data = JSON.parse(res);
        for (let i = 0; i < data.holidays.length; i++) {
          const entry = data.holidays[i];
          this.$set(this.holidays, i, {
            start: new Date(entry.start),
            end: new Date(entry.end),
          });
        }
        for (let i = 0; i < data.terms.length; i++) {
          const entry = data.terms[i];
          this.$set(this.terms, i, Term.fromJSON(entry));
        }

        this.hasData = true;
        return true;
      } else return false;
    },
    updateStorage() {
      console.log("changed");
      localStorage.setItem("kht.hasData", this.hasData);
      localStorage.setItem(
        "kht.noteboard",
        JSON.stringify(this.generateFullData())
      );
    },
  },
  computed: {
    dates() {
      const res = [
        { name: "1. Abschnitt", start: "", end: "" },
        { name: "2. Abschnitt", start: "", end: "" },
        { name: "1. Ferienphase", start: "", end: "" },
        { name: "2. Ferienphase", start: "", end: "" },
      ];

      if (this.terms.length == 2 && this.terms[0] instanceof Term) {
        for (let i = 0; i < this.terms.length; i++) {
          const e = this.terms[i];
          res[i].start = DateUtils.toNormalString(e.termStart);
          res[i].end = DateUtils.toNormalString(e.termEnd);
        }
      }

      if (this.holidays.length == 2 && this.holidays[0].start) {
        for (let i = 0; i < this.holidays.length; i++) {
          const e = this.holidays[i];
          res[i + 2].start = DateUtils.toNormalString(e.start);
          res[i + 2].end = DateUtils.toNormalString(e.end);
        }
      }
      return res;
    },
  },
  watch: {
    terms: {
      handler() {
        this.updateStorage();
      },
    },
  },
  mounted() {
    if (this.fetchSavedBoard()) {
      this.terms[0].fillDays();
      this.terms[1].fillDays();
    }
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


// terms: [
//         {
//           termStart: new Date(2022, 0, 31),
//           termEnd: new Date(2022, 3, 8),
//           days: [
//             {
//               date: new Date(2022, 0, 31),
//               notes: [
//                 { grade: ["Q1"], content: "One Note" },
//                 { grade: ["Q1"], content: "Two Note" },
//               ],
//             },
//             {
//               date: new Date(2022, 1, 1),
//               notes: [{ grade: ["Q1"], content: "One Note" }],
//             },
//             {
//               date: new Date(2022, 1, 2),
//               notes: [],
//             },
//           ],
//         },
//         {
//           termStart: new Date(2022, 0, 31),
//           termEnd: new Date(2022, 3, 8),
//           days: [
//             {
//               date: new Date(2022, 0, 31),
//               notes: [
//                 { grade: ["Q1"], content: "One Note" },
//                 { grade: ["Q1"], content: "Two Note" },
//               ],
//             },
//           ],
//         },
//       ],

//  holidays: [
//         {
//           start: new Date(2022, 1, 1, 12),
//           end: new Date(2022, 1, 2, 12),
//         },
//         {
//           start: new Date(2022, 1, 3, 12),
//           end: new Date(2022, 1, 4, 12),
//         },
//       ],
