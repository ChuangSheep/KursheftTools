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
            <component
              :is="pages[step - 1]"
              @next="step == 3 ? (step = 1) : step++"
            ></component>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
import Noteboard from "./components/Noteboard.vue";
import Courselist from "./components/Courselist.vue";
import Steps from "./components/Steps.vue";

export default {
  name: "App",

  components: {
    Noteboard,
    Steps,
    Courselist,
  },

  data: () => ({
    step: 1,
    pages: ["Noteboard", "Courselist"],
  }),
};
</script>
