<template>
  <v-container>
    <v-row>
      <v-dialog max-width="700px" persistent>
        <template v-slot:activator="{ on, attrs }">
          <v-btn class="mr-6" color="primary" v-on="on" v-bind="attrs"
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
                  label="Stufen zum Exportieren"
                  multiple
                  hide-details
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
              <v-btn color="primary" @click="onGenerate(dialogResult)"
                >Bestätigen</v-btn
              >
              <div class="px-2"></div>
              <v-btn @click="dialogResult.value = false">Abbrechen</v-btn>
            </v-card-actions>
          </v-card>
        </template>
      </v-dialog>
      <v-spacer></v-spacer>
      <v-btn
        class="mr-6"
        @click="$emit('back')"
        :disabled="progress >= 0 && progress < 100"
        >Zurück</v-btn
      >
    </v-row>
    <v-row class="mt-16" justify="space-around">
      <v-col cols="10">
        <v-progress-linear
          v-model="progress"
          height="25"
          :color="progress >= 0 ? 'primary' : 'grey'"
          striped
        >
        </v-progress-linear>
      </v-col>
    </v-row>
    <v-row justify="space-around" class="mt-6">
      <v-col cols="10">
        <v-card>
          <v-card-title> Zusammenfassung </v-card-title>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { Term } from "@/models/Term";

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
      progress: -1,
      prevGrades: [],
      gradesToExport: [],
      holidays: [],
      terms: [],
      courselist: [],
    };
  },
  methods: {
    onGradesChange() {
      if (!this.prevGrades.includes("Alle") && this.gradesToExport.includes("Alle"))
        this.gradesToExport = ["Alle"];
      else if (
        this.prevGrades.includes("Alle") &&
        this.gradesToExport.length > 0
      )
        this.gradesToExport = this.gradesToExport.filter((g) => g != "Alle");

      this.prevGrades = JSON.parse(JSON.stringify(this.gradesToExport));
    },
    onGenerate(dialogResult) {
      dialogResult.value = false;
    },
    fetchData() {
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

      {
        const hasData = localStorage.getItem("kht.noteboard.hasData");
        if (hasData !== "true") {
          console.error("No data for noteboard found");
          return;
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
        }
      }
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
