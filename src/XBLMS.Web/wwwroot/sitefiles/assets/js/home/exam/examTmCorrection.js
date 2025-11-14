var $url = '/exam/examTmCorrection';
var $urlSubmit = $url + '/submit';

var data = utils.init({
  tmId: utils.getQueryInt('tmId'),
  examPaperId: utils.getQueryInt('examPaperId'),
  total: 0,
  list: null,
  title:null,
  form: { tmId: null, reason: null,examPaperId:0 },
  success:null
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: { id: this.tmId } }).then(function (response) {
      var res = response.data;
      $this.total = res.total;
      $this.list = res.list;
      $this.title = res.title;
      $this.form.tmId = $this.tmId;
      $this.form.examPaperId = $this.examPaperId;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiSubmit: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlSubmit, this.form).then(function (response) {
      var res = response.data;
      if (res.value) {
        $this.success = true;
      }
      else {
        utils.error("提交失败，请重新尝试", { layer: true });
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.apiSubmit();
      }
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
