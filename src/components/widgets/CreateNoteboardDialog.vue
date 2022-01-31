<template>
  <v-dialog max-width="700px" persistent>
    <template v-slot:activator="{ on, attrs }">
      <v-btn class="mr-6" :color="activatorColor" v-on="on" v-bind="attrs">{{
        activatorTitle
      }}</v-btn>
    </template>
    <template v-slot:default="dialogResult">
      <v-card>
        <v-card-title>{{ title }}</v-card-title>
        <v-container class="pl-8 mt-4" fluid>
          <v-row
            class="pb-1 px-6"
            align="center"
            v-for="(data, i) in dates"
            :key="i"
          >
            <v-col cols="3" class="pa-0 pb-6">
              <span>{{ data.name }}</span>
            </v-col>
            <v-col cols="4" class="pa-0">
              <v-text-field
                type="date"
                placeholder="dd.mm.yyyy"
                label="Von"
                v-model="data.start"
                :rules="[validationRules[i].start]"
                :error="validationRules[i].start !== true"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
            <v-col cols="1" class="text-center"></v-col>
            <v-col cols="4" class="pa-0">
              <v-text-field
                type="date"
                placeholder="dd.mm.yyyy"
                label="Bis"
                v-model="data.end"
                :rules="[validationRules[i].end]"
                :error="validationRules[i].end !== true"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-container>
        <v-card-actions class="px-6 pb-4 pt-6"
          ><v-spacer></v-spacer
          ><v-btn color="primary" @click="onNoteboardChange(dialogResult)"
            >Bestätigen</v-btn
          >
          <div class="px-2"></div>
          <v-btn @click="dialogResult.value = false">Abbrechen</v-btn>
        </v-card-actions>
      </v-card>
    </template>
  </v-dialog>
</template>

<script>
import DateUtils from "@/logic/DateUtils.js";

export default {
  name: "CreateNoteboardDialog",
  props: {
    hasData: {
      required: true,
      type: Boolean,
    },
    initDates: {
      type: Array,
      default() {
        return [
          { name: "1. Abschnitt", start: "", end: "" },
          { name: "2. Abschnitt", start: "", end: "" },
          { name: "1. Ferien", start: "", end: "" },
          { name: "2. Ferien", start: "", end: "" },
        ];
      },
    },
  },
  data() {
    return {
      dates: [],
      validationRules: [
        { start: true, end: true },
        { start: true, end: true },
        { start: true, end: true },
        { start: true, end: true },
      ],
    };
  },
  methods: {
    onNoteboardChange(dialogResult) {
      if (this.validate()) {
        if (this.hasData) this.$emit("update", this.dates);
        else this.$emit("create", this.dates);
        dialogResult.value = false;
      }
    },
    validate() {
      let res = DateUtils.validateCourseDates(this.dates);
      for (let i = 0; i < res.length; i++) {
        const entry = res[i];
        this.$set(this.validationRules, i, entry);
      }

      for (let i = 0; i < res.length; i++) {
        const entry = res[i];
        if (entry.start !== true || entry.end !== true) return false;
      }
      return true;
    },
  },
  computed: {
    title() {
      return this.hasData
        ? "Bemerkungsbogen bearbeiten"
        : "Neuer Bemerkungsbogen";
    },
    activatorTitle() {
      return this.hasData ? "Bearbeiten" : "Neu";
    },
    activatorColor() {
      return this.hasData ? "" : "primary";
    },
  },
  watch: {
    initDates: {
      immediate: true,
      handler() {
        this.dates = this.initDates;
      },
    },
  },
  created() {
    if (this.hasData) this.dates = JSON.parse(JSON.stringify(this.initDates));
    else this.dates = this.initDates;
  },
};
</script>

<style>
</style>
