var $url = 'study/studyCourseEvaluationLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  item: '',
  itemList: null
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
      $this.item = res.item;
      $this.itemList = res.itemList;
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
