var $url = '/xbl/crudDemoEdit';
var $urlUpdate = $url + '/actions/update';
var $urlAdd = $url + '/actions/add';

var data = utils.init({
  id: utils.getQueryInt('id'),
  form: {}
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        id: this.id
      }
    }).then(function (response) {
      var res = response.data;

      //$this.form = res.info;
      $this.form = _.assign({}, res.info);

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiEdit: function () {
    var $this = this;

    utils.loading(this, true);

    var editUrl = $urlAdd;

    if (this.id > 0) {
      editUrl = $urlUpdate;
    }
    $api.post(editUrl, { info: this.form }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功")
        utils.closeLayer(false);
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
      if (valid) {
        $this.apiEdit();
      }
    });
  },

  btnCancelClick: function () {
    utils.closeLayer(false);
  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
