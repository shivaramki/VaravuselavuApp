<template>
    <div>
        <b-modal hide-header-close no-close-on-esc no-close-on-backdrop size="sm" v-model="showModal" @ok="updateNewPassword" :title="title" @cancel="cancelChangePassword">
            <b-alert :show="alertVisible" dismissible :variant="alertType" @dismissed="closeAlert">
                {{alertDesc}}
            </b-alert>
            <loading :active.sync="isLoading"></loading>
            <b-row>
                <b-col>
                    <label>
                        Email id:
                    </label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-input v-model="changePassword.emailId" length="35" type="text" placeholder="Enter user email id">

                    </b-form-input>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label>
                        Old password:
                    </label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-input v-model="changePassword.oldPassword" length="35" type="password" placeholder="Enter user old password">

                    </b-form-input>
                </b-col>
            </b-row>
            <br>
            <b-row>
                <b-col>
                    <label>
                        New password:
                    </label>
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <b-form-input v-model="changePassword.newPassword" length="35" type="password" placeholder="Enter new password">

                    </b-form-input>
                </b-col>
            </b-row>
        </b-modal>
    </div>
</template>

<script>
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.min.css";
export default {
  props: ["openModal", "showSpinner", "alertArgs"],
  components: {
    Loading
  },
  watch: {
    openModal: function(newVal, oldVal) {
      if (newVal == true) {
        this.showModal = true;
      } else {
        this.showModal = false;
      }
    },
    showSpinner: function(newVal, oldVal) {
      this.isLoading = this.showSpinner;
    },
    alertArgs: {
      handler: function(newVal) {
        (this.alertDesc = this.alertArgs.alertDesc),
          (this.alertTitle = this.alertArgs.alertTitle),
          (this.alertVisible = this.alertArgs.alertVisible),
          (this.alertType = this.alertArgs.alertType);
      },
      deep: true
    }
  },
  data: function() {
    return {
      changePassword: { emailId: "", oldPassword: "", newPassword: "" },
      isLoading: false,
      title: "Change password",
      showModal: false,
      alertDesc: "",
      alertTitle: "",
      alertVisible: false,
      alertType: ""
    };
  },
  methods: {
    updateNewPassword: function(evt) {
      evt.preventDefault();
      this.$emit("updatePassword", this.changePassword);
    },
    cancelChangePassword: function() {
      this.showModal = false;
      this.alertVisible = false;
      this.changePassword.emailId = "";
      this.changePassword.oldPassword = "";
      this.changePassword.newPassword = "";
      this.$emit("handleCancelChangePassword");
    },
    closeAlert: function() {
      this.alertVisible = false;
      this.$emit("handleCloseAlert");
    }
  }
};
</script>

