var $url = '/study/studyCourseEvaluationEdit';

var data = utils.init({
  id: utils.getQueryInt('id'),
  copyId: utils.getQueryInt('copyId'),
  form: null,
  itemList: null
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;

      $this.itemList = res.itemList;
      $this.form = _.assign({}, res.item);

      if ($this.id > 0) {

        if ($this.copyId > 0) {
          $this.id = 0;
          $this.form.id = 0;
          $this.form.title = $this.form.title + "-复制";
        }
      }
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
        $this.apiSubmit();
      }
    });
  },

  btnAddItemClick: function (star) {
    var item = { id: 0, evaluationId: 0, title: '评价项', textContent: !star, taxis: 1, guid: utils.uuid() };
    this.itemList.push(item);
    this.setTotalScoreAndTotalItem();
  },
  btnRemoveClick: function (item) {
    this.itemList = this.itemList.filter(f => f.guid !== item.guid);
    this.setTotalScoreAndTotalItem();
  },
  scoreChange: function () {
    this.setTotalScoreAndTotalItem();
  },
  setTotalScoreAndTotalItem: function () {
    this.form.totalScore = 0;
    this.form.totalItem = 0;
    if (this.itemList && this.itemList.length > 0) {
      this.form.totalItem = this.itemList.length;

      var staritemList = this.itemList.filter(f => !f.textContent);
      this.form.totalScore = staritemList.length * this.form.maxStar;
    }
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
