var $url = '/common/adminLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  admin: null,
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

      $this.admin = res.administrator;

    }).catch(function (error) {
         utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnCancelClick: function () {
    utils.closeLayerSelf();
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
