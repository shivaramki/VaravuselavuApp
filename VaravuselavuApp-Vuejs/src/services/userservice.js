import Vue from 'vue'

export default {
  getUsers: function () {
    return Vue.http.get("/api/users/list")
  },
  getUser: function (id) {
    return Vue.http.get("/api/user/search?id=" + id)
  },
  addUser: function (appuser) {
    return Vue.http.post("/api/user/add", appuser, {
      emulateJSON: true
    })
  },
  editUser: function (appuser) {
    return Vue.http.post("/api/user/edit", appuser, {
      emulateJSON: true
    })
  },
  changePassword: function (appuser) {
    return Vue.http.post("/api/user/changepassword", appuser, {
      emulateJSON: true
    })
  }
}
