import Vue from 'vue'

export default {
  addCategory: function (category) {
    return Vue.http.post("/api/category/add", category, {
      emulateJSON: true
    });
  },

  getCatgeoriesPaged: function (pageNo) {
    return Vue.http.get("/api/category/lists/" + encodeURIComponent(pageNo))
  },

  editCategory: function (category) {
    return Vue.http.post("/api/category/update", category, {
      emulateJSON: true
    });
  },

  deleteCategory: function (id) {
    return Vue.http.delete("/api/category/delete?id=" + encodeURIComponent(id));
  },

  getCategory: function (id) {
    return Vue.http.get("/api/category/search?id=" + id)
  },

  getCategories: function () {
    return Vue.http.get("/api/category/all-lists")
  }
}
