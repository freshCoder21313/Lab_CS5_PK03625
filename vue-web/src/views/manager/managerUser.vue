<template>
  <div>
    <table class="table" id="dt-mU"></table>
  </div>
</template>

<script>
//#region Import range
import defaultLanguageDatatable from "@/constants/configsDatatable.js";
import * as axiosClient from "@/api/axiosClient";
//#endregion

export default {
  name: "ManagerUser",
  components: {},
  props: {},
  data() {
    return {};
  },
  computed: {},
  watch: {},
  mounted() {
    function loadDt_mU() {
      axiosClient.getFromApi("/manager/nhanvien/get");
    }
  },
  methods: {},
};
</script>

<style scoped></style>
