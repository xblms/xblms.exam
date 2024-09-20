var data = utils.init({
  attributeName: 'attributeName',
  editContent: '',
  pf: utils.getQueryString("pf"),
  ptype: utils.getQueryString("ptype")
});

var methods = {
  btnSaveClick: function () {
    var parentLayer = top.frames[this.pf];
    if (this.ptype === 'form') {
      var ref = utils.getQueryString("ref");
      parentLayer.$vue.form[ref] = this.editContent;
    }
    if (this.ptype === 'options') {
      var ref = utils.toInt(utils.getQueryInt("ref"));
      parentLayer.$vue.setOptionsValue(ref, this.editContent);
    }

    utils.closeLayerSelf();
  },
  setEditContent: function () {
    var parentLayer = top.frames[this.pf];
    if (this.ptype === 'form') {
      var ref = utils.getQueryString("ref");
      this.editContent = parentLayer.$vue.form[ref];
    }
    if (this.ptype === 'options') {
      var ref = utils.getQueryInt("ref");
      this.editContent = parentLayer.$vue.options[ref];
    }

  }
};

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    var $this = this;
    utils.loading(this, false);

    setTimeout(function () {
      var editor = utils.getEditor($this.attributeName, $(window).innerHeight() - 290);
      $this.setEditContent();
      editor.ready(function () {
        this.addListener("contentChange", function () {
          $this.editContent = this.getContent();
        });
      });
    }, 100);
  }
});


