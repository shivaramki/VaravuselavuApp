import Vue from 'vue'

export default {
  addExpense: function (expense) {
    return Vue.http.post("/api/expense/add", expense, {
      emulateJSON: true
    });
  },

  editExpense: function (expense) {
    return Vue.http.post("/api/expense/update", expense, {
      emulateJSON: true
    });
  },

  deleteExpense: function (id) {
    return Vue.http.delete("api/expenses/delete?id=" + id);
  },

  getExpenses: function (pageNo) {
    return Vue.http.get("api/expenses/lists/" + encodeURIComponent(pageNo));
  }
}
