<template >
  <v-stepper v-model="step">
    <v-stepper-header>
      <v-stepper-step
        :complete="step > 1"
        step="1"
        :color="step > 1 ? 'grey' : 'primary'"
      >
        Bemerkungsbogen ausfüllen
      </v-stepper-step>
      <v-divider></v-divider>
      <v-stepper-step
        :complete="step > 2"
        step="2"
        :color="step > 2 ? 'grey' : 'primary'"
      >
        Kursliste importieren
      </v-stepper-step>
      <v-divider></v-divider>
      <v-stepper-step
        :complete="step > 3"
        step="3"
        :color="step > 3 ? 'grey' : 'primary'"
      >
        Kurshefte generieren
      </v-stepper-step>
    </v-stepper-header>
  </v-stepper>
</template>

<script>
export default {
  name: "Steps",
  props: {
    step: Number,
  },
  data() {
    return {};
  },
};
</script>

<style>
</style>