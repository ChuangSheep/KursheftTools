<template >
  <v-dialog :max-width="maxWidth">
    <template v-slot:activator="{ on, attrs }">
      <v-btn class="mr-6" v-on="on" v-bind="attrs" :color="activatorColor">{{
        activatorText
      }}</v-btn>
    </template>
    <template v-slot:default="dialogResult">
      <v-card>
        <v-card-title>{{ title }}</v-card-title>
        <slot name="content"
          ><v-card-text class="black--text">Sind Sie sicher</v-card-text></slot
        >
        <v-card-actions class="pb-4"
          ><v-btn
            @click="
              $emit('accept');
              dialogResult.value = false;
            "
            class="mr-3"
            color="error"
            >{{ yesText }}</v-btn
          ><v-btn
            @click="
              $emit('reject');
              dialogResult.value = false;
            "
            >{{ noText }}</v-btn
          ></v-card-actions
        >
      </v-card>
    </template>
  </v-dialog>
</template>

<script>
export default {
  name: "ComfirmationDialog",
  props: {
    maxWidth: {
      default: "500px",
      type: String,
    },
    title: {
      default: "Warnung",
      type: String,
    },
    yesText: {
      default: "Ja",
      type: String,
    },
    noText: {
      default: "Nein",
      type: String,
    },
    activatorText: {
      required: true,
      type: String,
    },
    activatorColor: {
      default: "",
      type: String,
    },
  },
  data() {
    return {};
  },
  methods: {},
};
</script>

<style>
</style>