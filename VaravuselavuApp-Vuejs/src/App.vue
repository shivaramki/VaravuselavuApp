<template>
  <div id="app">
    <b-navbar toggleable="md" type="dark" variant="info">
      <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>
      <b-navbar-brand to="/dashboard">Varavuselavu</b-navbar-brand>
      <b-collapse is-nav id="nav_collapse">
        <b-navbar-nav>
            <b-nav-item v-if="authenticated" to="/category">
                Category
            </b-nav-item>
            <b-nav-item v-if="authenticated" to="/expenses">
                Expenses
            </b-nav-item>
             <b-nav-item v-if="isUserAdmin" to="/user">
                User
            </b-nav-item>
        </b-navbar-nav>
        <b-navbar-nav class="ml-auto">
            <b-nav-item-dropdown right v-if="authenticated">
        <!-- Using button-content slot -->
        <template slot="button-content">
          <em>{{loggedUserName}}</em>
        </template>
        <b-dropdown-item href="#" @click="handleChangePassword">Change password</b-dropdown-item>
        <b-dropdown-item @click="handleLogout" >Signout</b-dropdown-item>
      </b-nav-item-dropdown>
        </b-navbar-nav>
        
      </b-collapse>
    </b-navbar>
     <b-alert :show="showAlertVisible" dismissible :variant="showAlertType" @dismissed="closeAlert">
              {{showAlertDesc}}
      </b-alert>
    <br>
   <router-view></router-view>
   <change-password-modal :open-modal="showChangePasswordModal" :show-spinner="isLoading" :alert-args="pageAlertObj" @updatePassword ="updateNewPassword" @handleCancelChangePassword="cancelChangePassword" @handleCloseAlert="cancelAlert"></change-password-modal>
  </div>
</template>

<script>
import Vue from "vue";
import { EventBus } from "./globalevent.js";
import { mapGetters } from "vuex";
import Store from "./store.js";
import authService from "./services/authservice.js";
import changePasswordModal from "./pages/ChangePassword.vue";
import UserService from "./services/userservice.js";

export default {
  name: "app",
  components: {
    changePasswordModal
  },
  computed: {
    ...mapGetters(["authenticated", "isUserAdmin", "loggedUserName"])
  },
  data: function() {
    return {
      showChangePasswordModal: false,
      showAlertDesc: "Server issue,Please contact the administrator",
      showAlertTitle: "Errors",
      showAlertVisible: false,
      showAlertType: "danger",
      isLoading: false,
      pageAlertObj: {
        alertDesc: "",
        alertTitle: "",
        alertVisible: false,
        alertType: ""
      }
    };
  },
  methods: {
    handleLogout: function() {
      authService.appLogout().then(response => {
        var result = response.body;
        if (result.success) {
          Store.commit("changeLoggedInState", {
            value: false
          });
          Store.commit("changeisAdminstate", {
            value: false
          });
          this.$router.push("/");
        }
      });
    },
    handleChangePassword: function() {
      this.showChangePasswordModal = true;
    },
    updateNewPassword: function(changePassword) {
      this.isLoading = true;
      UserService.changePassword(changePassword).then(
        response => {
          var result = response.body;
          if (result.success) {
            this.showChangePasswordModal = false;
            this.isLoading = false;
            this.showAlertDesc = "Password update successfully";
            this.showAlertTitle = "";
            this.showAlertType = "success";
            this.showAlertVisible = true;
          } else {
            this.pageAlertObj.alertDesc = response.body.error.join();
            this.pageAlertObj.alertTitle = "Errors";
            this.pageAlertObj.alertVisible = true;
            this.pageAlertObj.alertType = "danger";
            this.isLoading = false;
          }
        },
        response => {
          this.isLoading = true;
          this.pageAlertObj.alertTitle = "Errors";
          this.pageAlertObj.alertVisible = true;
          this.pageAlertObj.alertType = "danger";
          this.pageAlertObj.showAlertDesc =
            "Internal server error, please contact administrator";
        }
      );
    },
    cancelChangePassword: function() {
      this.showChangePasswordModal = false;
    },
    closeAlert: function() {
      this.showAlertVisible = false;
    },
    cancelAlert: function() {
      this.pageAlertObj.alertVisible = false;
    }
  }
};
</script>
