var $url = 'common/examTmDeleteLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  tm: null
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

      $this.tm = res.item;
    }).catch(function (error) {
      utils.error(error, { layer:true });
    }).then(function () {
      utils.loading($this, false);
    });
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
