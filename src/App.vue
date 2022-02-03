<template>
  <v-app>
    <v-main>
      <v-container>
        <v-row justify="center">
          <v-col cols="12">
            <Steps :step="step" />
          </v-col>
        </v-row>
        <v-row justify="center">
          <v-col cols="12">
            <!-- <transition name="fade-transition" :duration="{ enter: 500, leave: 500 }"> -->
              <component
                :allowedGrades="allowedGrades"
                :is="pages[step - 1]"
                @back="step == 1 ? (step = 3) : step--"
                @next="step == 3 ? (step = 1) : step++"
              ></component>
            <!-- </transition> -->
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import Noteboard from "./components/Noteboard.vue";
import Courselist from "./components/Courselist.vue";
import BookExport from "./components/BookExport.vue";
import Steps from "./components/Steps.vue";

export default {
  name: "App",

  components: {
    Noteboard,
    Steps,
    Courselist,
    BookExport,
  },

  data: () => ({
    step: 1,
    pages: ["Noteboard", "Courselist", "BookExport"],
    allowedGrades: ["Alle", "EF", "Q1", "Q2"],
  }),
};
</script>
