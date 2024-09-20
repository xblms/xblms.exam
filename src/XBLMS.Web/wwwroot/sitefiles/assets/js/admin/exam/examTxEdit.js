var $url = 'exam/examTxEdit';
var $urlUpdate = $url + '/update';
var $urlAdd = $url + '/add';

var data = utils.init({
  id: utils.getQueryInt('id'),
  typeList:null,
  form: {}
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id }}).then(function (response) {
      var res = response.data;
      $this.typeList = res.typeList;
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
        if ($this.id > 0) {
          utils.success("成功修改题型")
        }
        else {
          utils.success("成功新增题型")
        }
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      utils.closeLayerSelf();
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$refs.form.validate(function(valid) {
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
