var $url = 'study/studyCourseFilesGroupEdit';
var $urlAdd = $url + "/add";
var $urlUpdate = $url + "/update";

var data = utils.init({
  id: utils.getQueryInt('id'),
  groupId: utils.getQueryInt('groupId'),
  form: {}
});

var methods = {
  apiGet: function () {
    var $this = this;
   

    utils.loading(this, true);
    $api.get($url, { params: { id: this.id }}).then(function (response) {
      var res = response.data;

      $this.form = _.assign({}, res.item);
      $this.form.parentId = $this.groupId;

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
    $api.post(editUrl, this.form).then(function (response) {
      var res = response.data;
      parent.$vue.$message({
        message: '操作成功',
        type: 'success',
        showClose: true
      });
      utils.closeLayerSelf();
    }).catch(function (error) {
      utils.error(error, { layer: true });
    }).then(function () {
      utils.loading($this, false);
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
