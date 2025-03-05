import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import ManagerUser from "@/views/manager/managerUser.vue";
import ManagerProduct from "@/views/manager/ManagerProduct.vue";

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
  },
  {
    path: "/manager/user",
    name: "ManagerUser",
    component: ManagerUser,
  },
  {
    path: "/manager/product",
    name: "ManagerProduct",
    component: ManagerProduct,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
