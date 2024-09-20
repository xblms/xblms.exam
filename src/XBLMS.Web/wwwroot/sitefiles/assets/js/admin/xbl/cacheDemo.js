var $url = '/xbl/cacheDemo';

var data = utils.init({
  form: {
    key: "",
    value: ""
  }
});

var methods = {

  btnGetValueClick: function () {

    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        utils.loading($this, true);
        $api.get($url, { params: { key: $this.form.key } }).then(function (response) {
          var res = response.data;
          $this.form.value = res.value;
        }).catch(function (error) {
          utils.error(error);
        }).then(function () {
          utils.loading($this, false);
        });
      }
    });


 

  },
  btnSetValueClick: function () {
    var $this = this;

    this.$refs.form.validate(function (valid) {
      if (valid) {
        utils.loading($this, true);
        $api.post($url, $this.form).then(function (response) {
          var res = response.data;
          if (res.value) {
            utils.success("设置成功");
          }
        }).catch(function (error) {
          utils.error(error);
        }).then(function () {
          utils.loading($this, false);
        });
      }
    });

  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    utils.loading(this, false);
  }
});
