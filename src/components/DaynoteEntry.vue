<template>
  <v-card class="mb-2">
    <v-subheader>{{ dateUtils.toWeekdayAndFullDate(date) }}</v-subheader>
    <v-row v-for="(data, i) in notes" :key="i" class="pa-4">
      <v-col cols="8" class="py-0">
        <v-select
          v-model="data.grades"
          :items="allowedGrades"
          @change="onNoteGradeChange(i, data)"
          :style="[
            data.grades.length > 1 ? { width: '160px' } : { width: '70pt' },
          ]"
          class="pa-0"
          multiple
          hide-details
        >
          <template v-slot:selection="{ item, index }">
            <v-chip v-if="index < 2" style="height: 30px"
              ><span>{{ item }}</span></v-chip
            >
            <span v-if="index == 2" class="text-caption grey--text pl-2"
              >+{{ data.grades.length - 2 }}</span
            >
          </template>
        </v-select>
      </v-col>
      <v-spacer></v-spacer>
      <v-col cols="3" class="text-right pa-0">
        <v-btn icon @click="onNoteDelete(i)">
          <v-icon>mdi-delete</v-icon></v-btn
        >
      </v-col>

      <v-col cols="12" class="py-0">
        <v-textarea
          rows="1"
          placeholder="Notiz"
          v-model="data.content"
          @input="$emit('update', notes)"
          auto-grow
          hide-details
        ></v-textarea
      ></v-col>
    </v-row>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn text @click="onNoteCreate">Neuer Eintrag</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import DateUtils from "@/logic/DateUtils.js";

export default {
  name: "DaynoteEntry",
  props: {
    date: Date,
    initNotes: [],
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
      prevNotes: [],
      notes: [],
    };
  },
  methods: {
    onNoteGradeChange(i, note) {
      if (
        !this.prevNotes[i].grades.includes("Alle") &&
        note.grades.includes("Alle")
      )
        note.grades = ["Alle"];
      else if (
        this.prevNotes[i].grades.includes("Alle") &&
        note.grades.length > 0
      )
        note.grades = note.grades.filter((g) => g != "Alle");

      this.$emit("update", this.notes);
      this.prevNotes = JSON.parse(JSON.stringify(this.notes));
    },
    onNoteCreate() {
      this.notes.push({ grades: ["Alle"], content: "" });
      this.$emit("update", this.notes);
      this.prevNotes = JSON.parse(JSON.stringify(this.notes));
    },
    onNoteDelete(i) {
      this.notes.splice(i, 1);
      this.$emit("update", this.notes);
      this.prevNotes = JSON.parse(JSON.stringify(this.notes));
    },
  },
  created() {
    this.notes = this.initNotes;
    this.prevNotes = JSON.parse(JSON.stringify(this.notes));
  },
};
</script>

<style>
</style>