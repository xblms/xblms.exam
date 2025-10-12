var $url = 'exam/examTxEdit';
var $urlUpdate = $url + '/update';
var $urlAdd = $url + '/add';

var data = utils.init({
  id: utils.getQueryInt('id'),
  typeList: null,
  tmTotal: 0,
  form: {}
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.typeList = res.typeList;
      $this.tmTotal = res.tmTotal;
      $this.form = _.assign({}, res.item);
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
    $api.post(editUrl, { item: this.form }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
        utils.closeLayerSelf();
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
        $this.apiEdit();
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
