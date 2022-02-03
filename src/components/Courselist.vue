<template >
  <v-container>
    <v-row>
      <v-btn class="mr-6" @click="$refs.import.click()" color="primary"
        >Importieren</v-btn
      >
      <input
        id="courselist-file"
        v-show="false"
        ref="import"
        type="file"
        @change="onFileImport"
      />
      <v-spacer></v-spacer>
      <v-btn class="mr-6" @click="$emit('back')">Zurück</v-btn>
      <v-btn @click="$emit('next')" color="primary" :disabled="!hasData">
        Weiter
      </v-btn>
    </v-row>

    <v-row>
      <v-col cols="12" class="mt-6">
        <v-data-table
          :headers="headers"
          :items="frontList"
          sort-by="class"
          :sort-desc="true"
          no-data-text="Bitte importieren Sie die Kursliste"
        ></v-data-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import CSVUtils from "@/logic/CSVUtils.js";

export default {
  name: "Courselist",
  data() {
    return {
      hasData: false,
      list: new Map(),
      condensedList: [],
      headers: [
        { text: "Stufe", value: "class", align: "start" },
        { text: "Fach", value: "subject" },
        { text: "Lehrer|In", value: "teacher" },
        { text: "Wochentage", value: "days" },
      ],
      frontList: [],
    };
  },
  methods: {
    async onFileImport() {
      let file = this.$refs.import.files[0];
      if (file) {
        CSVUtils.toGradeLists(file, this);
        this.hasData = true;
      }
      // must play with dom to remove the pending file
      document.getElementById("courselist-file").value = "";
    },
    fetchList() {
      const hasData = sessionStorage.getItem("kht.courselist.hasData");
      if (hasData !== "true") {
        this.hasData = false;
        return;
      }

      const res = sessionStorage.getItem("kht.courselist");
      if (res) {
        this.condensedList = JSON.parse(res);
        this.hasData=true;
      }
    },
    updateStorage() {
      sessionStorage.setItem("kht.courselist.hasData", this.hasData);
      sessionStorage.setItem(
        "kht.courselist",
        JSON.stringify(this.condensedList)
      );
    },
  },
  watch: {
    condensedList() {
      console.log("list changed");
      this.condensedList.forEach((course) => {
        let copy = JSON.parse(JSON.stringify(course));
        copy.days = copy.days.map(
          (arr) =>
            `${arr[0]}${arr[1] == "" ? "" : " ("}${arr[1] == "" ? "" : arr[1]}${
              arr[1] == "" ? "" : ")"
            }`
        );
        copy.days = copy.days.join(", ");
        this.frontList.push(copy);
      });
      this.updateStorage();
    },
  },
  mounted() {
    this.fetchList();
  },
};
</script>

<style>
</style>