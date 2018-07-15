<template>
    <div>
        <b-row>
            <b-col>
                <b-alert :show="showAlertVisible" dismissible :variant="showAlertType" @dismissed="closeAlert">
                    {{showAlertDesc}}
                </b-alert>
            </b-col>
        </b-row>
        <b-row>
            <b-col :offset="1" :md="10">
                <b-row>
                    <b-col>
                        <b-card header-tag="header">
                            <div slot="header">
                                <b-row>
                                    <b-col>
                                        <h3>Category wise expenses</h3>
                                    </b-col>
                                </b-row>
                            </div>
                            <b-row>
                                <b-col>
                                    <pie-chart :chart-data="datacollectionForCategoryWise" :options="{responsive: false, maintainAspectRatio: false}"></pie-chart>
                                </b-col>
                            </b-row>
                        </b-card>
                    </b-col>
                    <b-col>
                        <b-card header-tag="header">
                            <div slot="header">
                                <b-row>
                                    <b-col>
                                        <h3>Month wise expenses</h3>
                                    </b-col>
                                </b-row>
                            </div>
                            <b-row>
                                <b-col>
                                    <bar-chart :chart-data="datacollectionForMonthWise" :options="{responsive: false, maintainAspectRatio: false}"></bar-chart>
                                </b-col>
                            </b-row>
                        </b-card>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </div>
</template>

<script>
import PieChart from "./../Charts/DoughnutChart.js";
import BarChart from "./../Charts/BarChart.js";
import dashboardService from "./../services/dashboardservice.js";
export default {
  components: {
    PieChart,
    BarChart
  },
  data: function() {
    return {
      categoriesName: [],
      categoryAmount: [],
      datacollectionForCategoryWise: null,
      datacollectionForMonthWise: null,
      sumForMonthWise: [],
      showAlertDesc: "Internal server issue, please contact the administrator",
      showAlertTitle: "Errors",
      showAlertVisible: false,
      showAlertType: "danger"
    };
  },
  created: function() {
    this.getDashboardCategoryWiseData();
    this.getMonthWiseData();
  },
  methods: {
    getDashboardCategoryWiseData: function() {
      dashboardService.getDashboardCategoryWiseData().then(
        response => {
          var result = response.body;
          if (result.success) {
            var data = response.body.data;
            data.forEach(element => {
              this.categoriesName.push(element.name);
              this.categoryAmount.push(element.amount);
            });
            this.datacollectionForCategoryWise = {
              labels: this.categoriesName,
              datasets: [
                {
                  label: "Mothly category wise split",
                  data: this.categoryAmount,
                  backgroundColor: [
                    "#ff6384",
                    "#36a2eb",
                    "#cc65fe",
                    "#ffce56",
                    "#034f84",
                    "#80ced6"
                  ]
                }
              ]
            };
          } else {
            this.showAlertVisible = true;
          }
        },
        response => {
          this.handleServerError();
        }
      );
    },
    getMonthWiseData: function() {
      dashboardService.getDashboardMonthWiseData().then(
        response => {
          var result = response.body;
          if (result.success) {
            var data = response.body.data;
            data.forEach(element => {
              this.sumForMonthWise.push(element.sum);
            });
            this.datacollectionForMonthWise = {
              labels: [
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Oct",
                "Nov",
                "Dec"
              ],
              datasets: [
                {
                  label: "Month wise expenses",
                  data: this.sumForMonthWise,
                  //data: [25000,22000,23000,18000,14000,35000,28000,0,0,0,0,0],
                  backgroundColor: ["#ff6384", "#36a2eb", "#cc65fe", "#ffce56"]
                }
              ]
            };
          } else {
            this.showAlertVisible = true;
          }
        },
        response => {
          this.handleServerError();
        }
      );
    },
    closeAlert: function() {
      this.showAlertVisible = false;
    },
    handleServerError: function() {
      this.showAlertDesc = "Internal server issue, please contact the administrator";
      this.showAlertTitle = "Errors";
      this.showAlertVisible = true;
      this.showAlertType = "danger";
    }
  }
};
</script>

