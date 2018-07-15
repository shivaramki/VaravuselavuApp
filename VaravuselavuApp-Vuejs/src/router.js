import Vue from "vue";
import Router from "vue-router";
import Category from "./pages/Category.vue";
import Login from "./pages/Login.vue";
import Expenses from "./pages/Expenses.vue";
import Dashboard from "./pages/Dashboard.vue";
import User from "./pages/User.vue"
import AuthService from "./services/authservice.js";
import Store from "./store.js";

Vue.use(Router);

const router = new Router({
  hashbang: false,
  mode: "history",
  routes: [{
      path: "/category",
      name: "category",
      component: Category,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/",
      name: "login",
      component: Login
    },
    {
      path: "/dashboard",
      name: "dashboard",
      component: Dashboard,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/expenses",
      name: "expenses",
      component: Expenses,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/user",
      name: "user",
      component: User,
      meta: {
        requiresAuth: true,
        adminAuth: true
      }
    }
  ]
});

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth) {
    AuthService.checkHealth().then(
      response => {
        var result = response.body;
        if (result.success) {

          Store.commit("changeLoggedInState", {
            value: true
          });

          var userObject = JSON.parse(localStorage.getItem('userInfo'));

          Store.commit("setAppUsername", {
            value: userObject.Username
          });

          if (userObject.IsAdmin) {
            Store.commit("changeisAdminstate", {
              value: true
            });
          }
          next()
        } else {
          Store.commit("changeLoggedInState", {
            value: false
          });
          next("/")
        }
      },
      response => {
        next()
      }
    )
  } else {
    Store.commit("changeLoggedInState", {
      value: false
    });
    next();
  }
});

export default router;
