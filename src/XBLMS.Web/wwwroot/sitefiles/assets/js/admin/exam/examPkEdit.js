var $url = 'exam/examPkEdit';

var data = utils.init({
  id: utils.getQueryInt('id'),
  userGroupList: null,
  tmGroupList: null,
  form: null,
  ruleList:null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.userGroupList = res.userGroupList;
      $this.tmGroupList = res.tmGroupList;
      $this.ruleList = res.ruleList;

      if ($this.id === 0) {
        res.item.userGroupId = null;
      }

      $this.form = _.assign({}, res.item);

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
  },
  apiSubmit: function () {

    var $this = this;
    utils.loading($this, true);
    $api.post($url, { item: $this.form }).then(function (response) {
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
