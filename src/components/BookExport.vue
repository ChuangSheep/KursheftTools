<template>
  <v-container>
    <v-row>
      <v-dialog max-width="700px" persistent>
        <template v-slot:activator="{ on, attrs }">
          <v-btn
            class="mr-6"
            color="primary"
            v-on="on"
            v-bind="attrs"
            :disabled="progress > 0 && progress < 100"
            >Kurshefte generieren</v-btn
          >
        </template>
        <template v-slot:default="dialogResult">
          <v-card>
            <v-card-title> Kurshefte Generieren </v-card-title>
            <v-container class="pl-8 mt-4" fluid>
              <v-row class="pb-1 px-6" align="center">
                <v-select
                  v-model="gradesToExport"
                  :items="allowedGrades"
                  @change="onGradesChange"
                  :rules="[gradesToExport.length > 0 || 'Erforderlich']"
                  label="Stufen zum Exportieren"
                  multiple
                >
                  <template v-slot:selection="{ item }">
                    <v-chip style="height: 30px"
                      ><span>{{ item }}</span></v-chip
                    >
                  </template>
                </v-select>
              </v-row>
            </v-container>
            <v-card-actions class="pr-6 pl-10 pb-6 pt-4">
              <v-spacer></v-spacer>
              <v-btn
                color="primary"
                @click="onGenerate(dialogResult)"
                :disabled="gradesToExport.length == 0"
                >Bestätigen</v-btn
              >
              <div class="px-2"></div>
              <v-btn @click="dialogResult.value = false">Abbrechen</v-btn>
            </v-card-actions>
          </v-card>
        </template>
      </v-dialog>
      <v-btn @click="onRedownload" :disabled="progress < 0 || res == null"
        >Herunterladen</v-btn
      >
      <v-spacer></v-spacer>
      <v-btn
        class="mr-6"
        @click="$emit('back')"
        :disabled="progress >= 0 && progress < 100"
        >Zurück</v-btn
      >
    </v-row>

    <v-row justify="space-around">
      <v-col cols="10" class="mt-12">
        <v-progress-linear
          :value="progress"
          height="25"
          :color="progress >= 0 ? 'light-blue' : 'grey'"
          striped
        >
          <strong
            class="grey--text"
            :class="{ 'text--darken-4': progress >= 0 }"
            >{{ progressDisplay }}</strong
          >
        </v-progress-linear>
      </v-col>
    </v-row>
    <v-row justify="space-around" class="mt-6">
      <v-col cols="10">
        <v-card>
          <v-card-title> Zusammenfassung </v-card-title>
          <v-card-text>
            <p>
              Zu exportierende Kurse:
              {{ progress >= 0 ? `${exportCount}/${toExport}` : "N/A" }}
            </p>
            <p>
              Stufen: {{ progress >= 0 ? gradesToExport.join(", ") : "N/A" }}
            </p>
            <v-container fluid>
              <v-row v-for="(t, i) in dates" :key="i">
                <v-col cols="2" class="pa-0 my-0 mb-4">
                  {{ intervalName(i) }}:
                </v-col>
                <v-col cols="10" class="pa-0 my-0 mb-4">
                  {{ dateUtils.toGermanDateFormat(t.start) }} -
                  {{ dateUtils.toGermanDateFormat(t.end) }}</v-col
                >
              </v-row>
            </v-container>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import DateUtils from "@/logic/DateUtils.js";
import { Plan } from "@/models/Plan";
import PDFUtils from "@/logic/PDFUtils.js";

export default {
  name: "BookExport",
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
      progress: -1,
      prevGrades: [],
      gradesToExport: [],
      plan: null,
      courselist: [],
      res: null,
      exportCount: 0,
      toExport: 0,
    };
  },
  methods: {
    onGradesChange() {
      if (
        !this.prevGrades.includes("Alle") &&
        this.gradesToExport.includes("Alle")
      )
        this.gradesToExport = ["Alle"];
      else if (
        this.prevGrades.includes("Alle") &&
        this.gradesToExport.length > 0
      )
        this.gradesToExport = this.gradesToExport.filter((g) => g != "Alle");

      this.prevGrades = JSON.parse(JSON.stringify(this.gradesToExport));
    },
    async onGenerate(dialogResult) {
      dialogResult.value = false;
      window.URL.revokeObjectURL(this.res);
      this.res = null;

      const grades =
        this.gradesToExport == "Alle"
          ? this.allowedGrades
          : this.gradesToExport;

      const coursesToExport = this.courselist.filter((c) =>
        grades.includes(c.class)
      );
      this.exportCount = 0;
      this.toExport = coursesToExport.length;

      PDFUtils.create(
        this.plan,
        coursesToExport,
        () => {
          this.exportCount++;
          this.progress = (this.exportCount / this.toExport) * 100;
        },
        (blob) => {
          const filename = `merged-${this.gradesToExport.join("-")}`;
          this.res = window.URL.createObjectURL(blob);

          if (window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveBlob(blob, filename);
          } else {
            this.triggerDownload(filename, this.res);
          }
        }
      );
    },
    triggerDownload(filename, objectURL) {
      const elem = window.document.createElement("a");
      elem.href = objectURL;
      elem.download = filename;
      elem.style.display = "none";
      document.body.appendChild(elem);
      elem.click();
      document.body.removeChild(elem);
    },
    onRedownload() {
      this.triggerDownload(`merged-${this.gradesToExport.join("-")}`, this.res);
    },
    fetchData() {
      // try to fetch the course list
      {
        const hasData = sessionStorage.getItem("kht.courselist.hasData");
        if (hasData !== "true") {
          console.error("No data for courselist found");
          return;
        }

        const res = sessionStorage.getItem("kht.courselist");
        if (res) {
          this.courselist = JSON.parse(res);
        }
      }

      // try to fetch the noteboard
      {
        const hasData = localStorage.getItem("kht.noteboard.hasData");
        if (hasData !== "true") {
          console.error("No data for noteboard found");
          return;
        }

        const res = localStorage.getItem("kht.noteboard");
        if (res !== null) {
          const data = JSON.parse(res);
          this.plan = Plan.fromJSON(data);

          this.hasData = true;
        }
      }
    },
    intervalName(i) {
      return i < 2 ? `${i + 1}. Abschnitt` : `${i - 1}. Ferienphase`;
    },
  },
  computed: {
    progressDisplay() {
      return this.progress < 0
        ? '"Kurshefte Generieren" Klicken'
        : Math.ceil(this.progress);
    },
    dates() {
      if (!this.plan) return null;

      const res = [
        {
          start: this.plan.term1Begin,
          end: this.plan.term1End,
        },
        {
          start: this.plan.term2Begin,
          end: this.plan.term2End,
        },
      ];

      for (let i = 0; i < this.plan.holidays.length; i++) {
        const e = this.plan.holidays[i];
        res.push({
          start: e[0],
          end: e[1],
        });
      }
      return res;
    },
  },
  mounted() {
    this.fetchData();
    this.gradesToExport = ["Alle"];
    this.prevGrades = ["Alle"];
  },
};
</script>

<style >
</style>
