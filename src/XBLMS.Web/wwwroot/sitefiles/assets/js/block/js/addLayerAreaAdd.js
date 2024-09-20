var $url = '/settings/block/addLayerAreaAdd';

var data = utils.init({
  areas: null,
  form: {
    areaIds: null
  }
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;
      $this.areas = res.areas;
      $this.form.areaIds = null;
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function() {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      areaIds: this.form.areaIds
    }).then(function (response) {
      var res = response.data;

      var areas = res.areas;
      areas.forEach(function (area) {
        parent.$vue.addArea(
          area.id,
          area.name
        );
      });
      
      utils.closeLayer();
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnCancelClick: function () {
    utils.closeLayer();
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
