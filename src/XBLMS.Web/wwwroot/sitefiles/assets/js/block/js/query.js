var $url = '/settings/block/query';

var data = utils.init({
  form: {
    ipAddress: null
  },
  submitted: false,
  isAllowed: null,
  area: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;

      $this.form.ipAddress = res.value;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      ipAddress: this.form.ipAddress
    }).then(function (response) {
      var res = response.data;

      $this.submitted = true;
      $this.isAllowed = res.isAllowed;
      $this.area = res.area;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  getArea: function() {
    if (this.area) {
      return this.area.areaEn + '(' + this.area.areaCn + ')';
    }
    return '未知区域';
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
