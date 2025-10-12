var $url = '/common/userLayerView';

var data = utils.init({
  id: utils.getQueryInt('id'),
  user: null,
  groupName: null,
  systemCode: null,
});

var methods = {
  apiGet: function () {
    var $this = this;

    utils.loading(this, true);
    $api.get($url, {
      params: {
        id: this.id
      }
    }).then(function (response) {
      var res = response.data;

      $this.systemCode = res.systemCode;
      $this.user = res.user;
      $this.groupName = res.groupName;
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
    });
  },
  btnDocClick: function () {
    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: utils.getCommonUrl('userDocLayerView', { id: this.id }),
      width: "98%",
      height: "98%"
    });
  },
  btnCancelClick: function () {
    utils.closeLayerSelf();
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

