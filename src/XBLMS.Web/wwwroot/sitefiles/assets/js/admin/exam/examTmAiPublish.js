var $url = 'exam/examTm/ai/publish';
var $urlSave = $url + "/save";

var data = utils.init({
  aiServe: false,
  tmList: [],
  txList: null,
  publishLoading: false,
  publishCurrent: 0,
  publishTotal:0,
  form: {
    txId: 0,
    zsd: '',
    tmCount: 3
  }
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;
      $this.aiServe = res.aiServe;
      $this.txList = res.txList;
      if (!res.aiServe) {
        utils.error('未连接到ai服务器', { layer: true });
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  apiPulish: function () {
    var $this = this;
    $api.post($url, $this.form).then(function (response) {
      var res = response.data;
      if (res.success) {
        $this.publishCurrent++;
        $this.publishTotal++;
        $this.tmList.unshift(res.item);
        utils.scrollTop();
        if ($this.publishCurrent < $this.form.tmCount) {
          $this.$nextTick(() => {
            $this.apiPulish();
          })
        }
        else {
          $this.publishLoading = false;
          top.utils.alertSuccess({
            title: '成 功',
            text: 'AI出题已完成，保存后数据才会生效',
            callback: function () {
              $this.clearForm();
            }
          });
        }
      }
      else {
        utils.error(res.msg, { layer: true });
        $this.clearForm();
        $this.publishLoading = false;
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
      $this.publishLoading = false;
      $this.clearForm();
    }).then(function () {
    });
  },
  clearForm: function () {
    this.form.txId = 0;
    this.form.zsd = "";
    this.publishCurrent = 0;
  },
  btnPublishClick: function () {
    var $this = this;
    this.$refs.form.validate(function (valid) {
      if (valid) {
        $this.publishLoading = true;
        $this.apiPulish();
      }
    });
  },
  btnSaveClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '提 醒',
      text: '保存出题数据，并退出AI出题界面，确定吗?',
      callback: function () {
        $this.apiSave();
      }
    });
  },
  apiSave: function () {
    var $this = this;
    $api.post($urlSave, { tmList: this.tmList }).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("操作成功");
        utils.closeLayerSelf();
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
    });
  },
  btnClearTmClick: function () {
    var $this = this;
    top.utils.alertWarning({
      title: '提 醒',
      text: '清空右侧所有题目，确定吗?',
      callback: function () {
        $this.publishTotal = 0;
        $this.tmList = [];
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
