import Vue from "vue"
import Vuex from "vuex"

Vue.use(Vuex);

export default new Vuex.Store({

  state: {
    isLoggedIn: false,
    isAdmin: false,
    appUserName: ""
  },
  mutations: {
    changeLoggedInState(state, payload) {
      state.isLoggedIn = payload.value
    },
    changeisAdminstate(state, payload) {
      state.isAdmin = payload.value
    },
    setAppUsername(state, payload) {
      state.appUserName = payload.value
    }
  },
  getters: {
    authenticated: state => {
      return state.isLoggedIn
    },
    isUserAdmin: state => {
      return state.isAdmin
    },
    loggedUserName: state => {
      return state.appUserName
    }
  }
})