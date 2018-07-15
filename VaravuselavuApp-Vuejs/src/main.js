import Vue from 'vue'
import router from './router.js'
import VueResource from 'vue-resource'
import App from './App.vue'
import VueCharts from "vue-charts"
import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import store from "./store.js"


Vue.use(VueCharts);
Vue.use(VueResource);
Vue.use(BootstrapVue);

new Vue({
  el:"#app",
  store,
  router,
  render: h => h(App)
})
