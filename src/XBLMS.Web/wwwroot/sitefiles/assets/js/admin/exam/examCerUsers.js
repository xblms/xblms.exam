var $url = '/exam/examCerUsers';
var $urlExport = $url + '/export';
var $urlZip = $url + '/zip';

var data = utils.init({
  id: 0,
  title: '',
  form: {
    id: 0,
    keywords: '',
    dateFrom: null,
    dateTo: null,
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url, { params: $this.form }).then(function (response) {
      var res = response.data;
      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  userHandleCurrentChange: function (val) {
    this.form.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.form.pageIndex = 1;
    this.apiGet();
  },
  btnViewCer: function (cer) {
    top.utils.openLayerPhoto({
      title: cer.name,
      id: cer.id,
      src: cer.cerImg + '?r=' + Math.random()
    })
  },
  btnUserPaperView: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('examPaperUserLayerView', { id: id }),
      width: "99%",
      height: "99%"
    });
  },
  btnCerExportClick: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($urlExport, { params: this.form }).then(function (response) {
      var res = response.data;

      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnCerZipClick: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($urlZip, { params: this.form }).then(function (response) {
      var res = response.data;
      window.open(res.value);
    }).catch(function (error) {
      utils.error(error, { layer: true });
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
    this.id = this.form.id = utils.getQueryInt("id");
    this.apiGet();
  }
});
