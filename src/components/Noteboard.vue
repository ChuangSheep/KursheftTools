<template>
  <v-container>
    <v-row justify="center">
      <!-- Create and edit the board -->
      <create-noteboard-dialog
        :hasData="hasData"
        :initDates="dates"
        @create="onNewBoardCreate"
        @update="onNewBoardUpdate"
      />

      <!-- Import new board -->
      <v-dialog v-if="hasData" max-width="500px">
        <template v-slot:activator="{ on, attrs }">
          <v-btn class="mr-6" v-on="on" v-bind="attrs">Importieren</v-btn>
        </template>
        <template v-slot:default="dialogResult">
          <v-card>
            <v-card-title>Warnung</v-card-title>
            <v-card-text class="black--text"
              >Wenn Sie einen anderen Bemerkungsbogen importieren,
              <b>verlieren Sie hier alle Daten</b>, die Sie nicht gespeichert
              haben.
            </v-card-text>
            <v-card-text class="black--text" style="margin-top: -10px">
              Sind Sie sicher?
            </v-card-text>
            <v-card-actions class="pb-4"
              ><v-btn
                @click="
                  $refs.import.click();
                  dialogResult.value = false;
                "
                class="mr-3"
                color="error"
                >Ja, Änderungen verwerfen</v-btn
              ><v-btn @click="dialogResult.value = false"
                >Nein</v-btn
              ></v-card-actions
            >
          </v-card>
        </template>
      </v-dialog>
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
import DaynoteEntry from "./DaynoteEntry.vue";

export default {
  name: "Noteboard",
  components: {
    DaynoteEntry,
    CreateNoteboardDialog,
  },
  data() {
    return {
      dateUtils: DateUtils,
      hasData: true,
      terms: [
        new Term(new Date(2022, 1, 1), new Date(2022, 1, 7)),
        new Term(new Date(2022, 1, 8), new Date(2022, 1, 14)),
      ],
      holidays: [
        {
          type: 1,
          start: new Date(2022, 1, 1, 12),
          end: new Date(2022, 1, 2, 12),
        },
        {
          type: 2,
          start: new Date(2022, 1, 3, 12),
          end: new Date(2022, 1, 4, 12),
        },
      ],
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
            type: entry.type,
          });
        }
        for (let i = 0; i < data.terms.length; i++) {
          const entry = data.terms[i];
          this.$set(this.terms, i, Term.fromJSON(entry));
        }
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
        { type: 1, start: dates[2].start, end: dates[2].end },
        { type: 2, start: dates[3].start, end: dates[3].end },
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
          type: 1,
          start: new Date(dates[2].start),
          end: new Date(dates[2].end),
        },
        {
          type: 2,
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
      }
    },
    updateStorage() {
      console.log("changed");
      localStorage.setItem(
        "kht.noteboard",
        JSON.stringify(this.generateFullData())
      );
    },
  },
  computed: {
    dates() {
      return [
        {
          name: "1. Abschnitt",
          start: DateUtils.toNormalString(this.terms[0].termStart),
          end: DateUtils.toNormalString(this.terms[0].termEnd),
        },
        {
          name: "2. Abschnitt",
          start: DateUtils.toNormalString(this.terms[1].termStart),
          end: DateUtils.toNormalString(this.terms[1].termEnd),
        },
        {
          name: "1. Ferien",
          start: DateUtils.toNormalString(this.holidays[0].start),
          end: DateUtils.toNormalString(this.holidays[0].end),
        },
        {
          name: "2. Ferien",
          start: DateUtils.toNormalString(this.holidays[1].start),
          end: DateUtils.toNormalString(this.holidays[1].end),
        },
      ];
    },
  },
  watch: {
    terms: {
      immediate: true,
      handler() {
        this.updateStorage();
      },
    },
  },
  mounted() {
    this.terms[0].fillDays();
    this.terms[1].fillDays();
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
