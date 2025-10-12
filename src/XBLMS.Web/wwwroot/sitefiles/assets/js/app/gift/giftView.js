var $url = "/giftView";

var data = utils.init({
  id: utils.getQueryInt("id"),
  gift: null,
  payDialogVisible: false,
  payForm: {
    id: 0,
    shopType: '',
    linkMan: '',
    linkTel: '',
    linkAddress: ''
  }
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading($this, true);
    $api.get($url, { params: { id: this.id } }).then(function (response) {
      var res = response.data;
      $this.gift = res.item;
      if ($this.gift.shopType !== 'OnSelf') {
        $this.payForm.shopType = $this.gift.shopType;
      }
      $this.payForm.id = $this.gift.id;

      $this.payForm.linkMan = res.item.linkMan;
      $this.payForm.linkTel = res.item.linkTel;
      $this.payForm.linkAddress = res.item.linkAddress;

    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnPaySubmitClick: function () {
    var $this = this;
    this.$refs.payForm.validate(function (valid) {
      if (valid) {
        $this.apiPay();
      }
    });
  },
  apiPay: function () {
    var $this = this;
    utils.loading($this, true);
    $api.post($url, this.payForm).then(function (response) {
      var res = response.data;
      if (res.value) {
        utils.success("兑换成功", { layer: true });
        $this.payDialogVisible = false;
      }
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },

};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.apiGet();
  },
});
