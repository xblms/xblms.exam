var $url = 'common/examCerLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  cer: null
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

      $this.cer = res.item;
    }).catch(function (error) {
      utils.error(error, { layer:true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnViewCer: function () {
    top.utils.openLayerPhoto({
      title: this.cer.name,
      id: this.cer.id,
      src: this.cer.backgroundImg.replace('/cer/', '/cer/preview_cer_') + '?r=' + Math.random()
    })
  },
  btnCountClick: function () {
    var $this = this;
    top.utils.openLayer({
      title: this.cer.name + '-获证人员列表',
      url: utils.getExamUrl('examCerUsers', { id: this.cer.id }),
      width: "88%",
      height: "98%"
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
