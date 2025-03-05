import { createApp } from "vue"; // Đúng cách cho Vue 3
import BootstrapVueNext from "bootstrap-vue-next";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue-3/dist/bootstrap-vue-3.css";

// Import Materialize CSS và JavaScript
import "materialize-css/dist/css/materialize.min.css";
import "materialize-css/dist/js/materialize.min.js";

import App from "./App.vue"; // App chính của bạn
import router from "./router"; // Router của Vue Router
import store from "./store"; // Store của Vuex hoặc Pinia (tùy theo bạn dùng cái gì)

const app = createApp(App);

// Sử dụng các plugin trong Vue 3
app.use(store); // Tích hợp store quản lý trạng thái
app.use(router); // Router dùng để điều hướng
app.use(BootstrapVueNext);

// Mount app
app.mount("#app");

/*
<script>
import M from "materialize-css"; // Import Materialize JS

export default {
  name: "DropdownExample",
  mounted() {
    // Khởi tạo dropdown như sau
    const elems = document.querySelectorAll(".dropdown-trigger");
    M.Dropdown.init(elems);
  },
};
</script>
*/
