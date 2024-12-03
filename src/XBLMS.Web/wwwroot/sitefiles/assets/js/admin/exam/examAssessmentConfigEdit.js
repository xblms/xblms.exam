var $url = '/exam/examAssessmentConfigEdit';

var data = utils.init({
  id: utils.getQueryInt('id'),
  copyId: utils.getQueryInt('copyId'),
  form: null,
  itemList: null,
  maxScore: 0,
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.itemList = res.itemList;
      $this.form = _.assign({}, res.item);


    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, { item: this.form, itemList: this.itemList }).then(function (response) {
      utils.success('操作成功！', { layer: true });
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
      utils.closeLayerSelf();
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

  btnAddItemClick: function (star) {
    var minScore = this.maxScore + 1;
    this.maxScore = this.maxScore + 10;
    var item = { guid: utils.uuid(), id: 0, result: '结果项名称', minScore: minScore, maxScore: this.maxScore };
    this.itemList.push(item);
  },
  btnRemoveClick: function (item) {
    this.itemList = this.itemList.filter(f => f.guid !== item.guid);
  },
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  }
});
