var $url = "/exam/examPractice/ready";
var $urlSubmit = $url + "/submit";
var $urlSearch = $url + "/search";

var data = utils.init({
  form: null,
  txList: null,
  zsdinputVisible: false,
  zsdinputValue: '',
});

var methods = {
  apiGet: function () {
    var $this = this;
    $api.get($url).then(function (response) {
      var res = response.data;

      $this.txList = res.txList;
      $this.form = _.assign({}, res.item);

      $this.setMineTmCount();

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  setMineTmCount: function () {
    if (this.form.tmCount > 0) {
      if (this.form.tmCount >= 100) {
        this.form.mineTmCount = 100;
      }
      else {
        this.form.mineTmCount = this.form.tmCount;
      }
    }
    else {
      this.form.mineTmCount = 0;
    }
  },
  apiGetSearch: function () {
    var $this = this;
    utils.loading(this, true, '正在加载配置...');
    $api.post($urlSearch, { txIds: this.form.txIds, nds: this.form.nds, zsds: this.form.zsds }).then(function (response) {
      var res = response.data;

      $this.form.tmCount = res.tmCount;
      $this.form.tmIds = res.tmIds;

      $this.setMineTmCount();

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnSubmitClick: function () {
    var $this = this;
    if (this.form.tmCount > 0) {
      this.$refs.form.validate(function (valid) {
        if (valid) {
          $this.apiSubmit();
        }
      });
    }
    else {
      utils.error("没有找到题目", { layer: true });
    }

  },
  apiSubmit: function () {
    var $this = this;

    utils.loading(this, true);

    $api.post($urlSubmit, { item: this.form }).then(function (response) {
      var res = response.data;
      if (res.value > 0) {
        location.href = utils.getExamUrl('examPracticing', { id: res.value });
      }
      else {
        utils.error("自定义刷题错误，请重新尝试", { layer: true });
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  goPractice: function (id) {
    var $this = this;
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getExamUrl('examPracticing', { id: id }),
      width: "68%",
      height: "88%",
      end: function () {
        $this.apiGet();
      }
    });
  },
  zsdbtnLogClick: function () {
    location.href = utils.getExamUrl('examPracticeLog');
  },
  zsdhandleClose(tag) {
    this.form.zsds.splice(this.form.zsds.indexOf(tag), 1);
    this.apiGetSearch();
  },

  zsdshowInput() {
    var $this = this;
    this.zsdinputVisible = true;
    this.$nextTick(_ => {
      $this.$refs.zsdsaveTagInput.$refs.input.focus();
    });
  },

  zsdhandleInputConfirm() {
    let inputValue = this.zsdinputValue;
    if (inputValue && !utils.contains(this.form.zsds, inputValue)) {
      this.form.zsds.push(inputValue);
      this.apiGetSearch();
    }
    this.zsdinputVisible = false;
    this.zsdinputValue = '';
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
