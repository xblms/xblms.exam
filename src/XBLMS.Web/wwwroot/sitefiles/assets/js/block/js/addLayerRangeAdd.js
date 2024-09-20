var data = utils.init({
  isAllowList: utils.getQueryBoolean('isAllowList'),
  form: {
    range: ''
  }
});

var methods = {
  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
      if (valid) {
        parent.$vue.addRange(
          $this.isAllowList,
          $this.form.range
        );
        utils.closeLayer();
      }
    });
  },

  btnCancelClick: function () {
    utils.closeLayer();
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
  }
});
