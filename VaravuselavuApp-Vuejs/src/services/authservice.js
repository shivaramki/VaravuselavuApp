import Vue from 'vue'

export default {

  appLogin: function (login) {
    return Vue.http.post("/api/login/auth", login, {
      emulateJSON: true
    });
  },

  checkHealth: function () {
    return Vue.http.get("api/health")
  },

  appLogout: function () {
    return Vue.http.post("/api/logout")
  }
}
