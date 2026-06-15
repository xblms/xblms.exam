var $url = '/settings/serverConfig';
var $urlAI = $url + '/ai';
var $urlAITest = $urlAI + '/test';

var data = utils.init({
  systemCodeOld: null,
  form: {
    systemCode: null,
    systemCodeName: null
  },
  isModels: false,
  aiRunningModels: null,
  formAi: {
    aiServe: null,
    aiHostUrl: null,
    aiRunningModel: null
  },
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.form.systemCode = $this.form.systemCodeOld = res.systemCode;
      $this.form.systemCodeName = res.systemCodeName;
      $this.formAi.aiServe = res.aiServe;
      $this.formAi.aiHostUrl = res.aiHostUrl;
      $this.formAi.aiRunningModel = res.aiRunningModel;
      $this.aiRunningModels = res.aiRunningModels;
      $this.isModels = res.isModels;

    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);
    $api.post($url, {
      systemCode: this.form.systemCode,
      systemCodeName: this.form.systemCodeName
    }).then(function (response) {
      var res = response.data;

      utils.success('操作成功！');

      if ($this.systemCode !== $this.form.systemCodeOld) {
        top.location.href = utils.getIndexUrl();
      }

    }).catch(function (error) {
      utils.error(error);
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
  btnAISubmitClick: function () {
    var $this = this;
    this.$refs.formAi.validate(function (valid) {
      if (valid) {
        $this.apiAiSubmit();
      }
    });
  },
  apiAiSubmit: function () {
    var $this = this;
    utils.loading(this, true);
    $api.post($urlAI, $this.formAi).then(function (response) {
      var res = response.data;
      utils.success('操作成功！');
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnRefreshAiClick: function () {
    this.apiAiTest();
  },
  btnAITestClick: function () {
    var $this = this;
    this.$refs.formAi.validate(function (valid) {
      if (valid) {
        $this.apiAiTest();
      }
    });
  },
  apiAiTest: function () {
    var $this = this;
    utils.loading(this, true, '正在请求AI服务器，请稍等...');
    $api.get($urlAITest, { params: { aiHostUrl: this.formAi.aiHostUrl } }).then(function (response) {
      var res = response.data;
      if (res.success) {
        $this.aiRunningModels = res.models;
        $this.formAi.aiServe = true;
        $this.isModels = res.isModels;
        top.utils.alertSuccess({ title: "AI连接成功", text: "版本：" + res.msg })
      }
      else {
        utils.error(res.msg);
      }
    }).catch(function (error) {
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },

  btnCloseClick: function () {
    utils.removeTab();
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
