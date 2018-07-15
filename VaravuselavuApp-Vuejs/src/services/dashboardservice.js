import Vue from 'vue'

export default {
  getDashboardCategoryWiseData: function () {
    return Vue.http.get("/api/dashboard/category")
  },
  getDashboardMonthWiseData: function () {
    return Vue.http.get("/api/dashboard/monthwise")
  }

}
