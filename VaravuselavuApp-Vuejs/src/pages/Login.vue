<template>
    <div>
        <b-row>
            <b-col :offset="4" :md="4">
                <loading :active.sync="isLoading"></loading>
                <b-card footer-tag="footer" title="Varavuselavu Login" img-src="https://picsum.photos/1000/300?image=366" img-alt="Image" img-top style="max-width: 40rem;">
                <em slot="footer">
                  <b-alert variant="danger" dismissible :show="alertVisible" @dismissed="alertVisible=false">
                    {{alertMessage}}
                  </b-alert>
                </em>
                    <b-container>
                        <b-form @submit="handleSubmit" @reset="handleReset">
                            <b-form-group id="userName" label="Email id" label-for="inputUserEmail">
                                <b-form-input id="inputUserEmail" v-model="login.emailId" required placeholder="Please enter the user name">
                                </b-form-input>
                            </b-form-group>
                            <b-form-group id="password" label="Password" label-for="inputPassword">
                                <b-form-input id="inputPassword" type="password" v-model="login.password" required placeholder="Password">
                                </b-form-input>
                            </b-form-group>
                            <b-button type="submit" variant="primary">
                                Login
                            </b-button>
                            <b-button type="reset" variant="danger">
                                Clear
                            </b-button>
                        </b-form>
                    </b-container>
                </b-card>
            </b-col>
        </b-row>
    </div>
</template>

<script>
import authService from "./../services/authservice.js";
import { EventBus } from "./../globalevent.js";
import Store from "./../store.js";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.min.css";

export default {
  components: {
    Loading
  },
  data: function() {
    return {
      login: { Id: 0, emailId: "", password: "" },
      alertMessage: "",
      alertVisible: false,
      isValidLogin: false,
      isLoading: false,
      user: ""
    };
  },
  methods: {
    handleSubmit: function(evt) {
      evt.preventDefault();
      if (this.login.emailId.trim() == "")
        this.alertMessage = "Please enter the user name";

      if (this.login.password.trim() == "")
        this.alertMessage = "Please enter the password";
      this.isLoading = true;
      authService.appLogin(this.login).then(
        response => {
          if (response.body.success) {
            Store.commit("changeLoggedInState", {
              value: true
            });
            this.user = response.body.data;
            Store.commit("setAppUsername", {
              value: this.user.Username
            });
            localStorage.setItem("userInfo", JSON.stringify(this.user));
            if (this.user.IsAdmin) {
              Store.commit("changeisAdminstate", {
                value: true
              });
            }
            Store.commit("setAppUsername", {
              value: this.user.Username
            });
            this.isLoading = false;
            this.$router.push("dashboard");
          } else {
            this.alertMessage = "Invalid username or password";
            this.alertVisible = true;
            this.isLoading = false;
          }
        },
        response => {
          this.alertMessage =
            "Internal server error, Please contact administrator.";
          this.alertVisible = true;
          this.isLoading = false;
        }
      );
    },

    handleReset: function(evt) {
      evt.preventDefault();
      this.login.emailId = "";
      this.login.password = "";
    }
  }
};
</script>

