var $url = '/study/studyCourseSelect';

var data = utils.init({
  formInline: {
    keyword: '',
    type:'',
    pageIndex: 1,
    pageSize: PER_PAGE
  },
  list: null,
  total: 0
});

var methods = {
  apiGet: function () {
    var $this = this;
    utils.loading(this, true);
    $api.get($url, { params: $this.formInline }).then(function (response) {
      var res = response.data;

      $this.list = res.list;
      $this.total = res.total;

    }).catch(function (error) {
      utils.loading($this, false);
      utils.error(error);
    }).then(function () {
      utils.loading($this, false);
    });
  },
  handleCurrentChange: function (val) {
    this.formInline.pageIndex = val;
    this.apiGet();
  },
  btnSearchClick: function () {
    this.formInline.pageIndex = 1;
    this.apiGet();
  },
  btnAddClick: function (isOnline) {
    var $this = this;
    var layerWidth = "68%";

    var url = utils.getStudyUrl('studyCourseFaceEdit', { id: 0, face: true });
    if (isOnline) {
      layerWidth = "98%";
      url = utils.getStudyUrl('studyCourseEdit', { id: 0, face: false });
    }

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: url,
      width: layerWidth,
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  btnEditClick: function (course) {
    var $this = this;

    var layerWidth = "68%";

    var url = utils.getStudyUrl('studyCourseFaceEdit', { id: course.id, face: true });
    if (!course.offLine) {
      layerWidth = "98%";
      url = utils.getStudyUrl('studyCourseEdit', { id: course.id, face: false });
    }

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: url,
      width: layerWidth,
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });

  },
  btnCopyClick: function (course) {
    var $this = this;
   
    var layerWidth = "68%";

    var url = utils.getStudyUrl('studyCourseFaceEdit', { id: course.id, copyId: course.id });
    if (!course.offLine) {
      layerWidth = "98%";
      url = utils.getStudyUrl('studyCourseEdit', { id: course.id, copyId: course.id });
    }

    top.utils.openLayer({
      title: false,
      closebtn: 0,
      url: url,
      width: layerWidth,
      height: "98%",
      end: function () {
        $this.btnSearchClick();
      }
    });
  },
  btnSelectClick: function () {
    var $this = this;

    var nodes = this.$refs.table.selection;

    var courses = [];
    for (var i = 0; i < nodes.length; i++) {
      courses.push(nodes[i])
    }
    if (courses.length > 0) {
      var parentFrameName = utils.getQueryString("pf");
      var parentLayer = top.frames[parentFrameName];

      var isSelect = utils.getQueryBoolean("isSelect");
      parentLayer.$vue.selectCourseCallback(courses,isSelect);
      utils.closeLayerSelf();
    }
    else {
      utils.error("请至少选择一门课程", { layer: true });
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
