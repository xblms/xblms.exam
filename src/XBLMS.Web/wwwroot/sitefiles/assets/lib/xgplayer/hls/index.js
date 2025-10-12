import { inherits as _inherits, createSuper as _createSuper, createClass as _createClass, classCallCheck as _classCallCheck } from "./_virtual/_rollupPluginBabelHelpers.js";
import { Events, BasePlugin } from "xgplayer";
import Hls from "hls.js/dist/hls.light.js";
import util from "./utils/index.js";
var HlsJsPlugin = /* @__PURE__ */ function(_BasePlugin) {
  _inherits(HlsJsPlugin2, _BasePlugin);
  var _super = _createSuper(HlsJsPlugin2);
  function HlsJsPlugin2(args) {
    var _this;
    _classCallCheck(this, HlsJsPlugin2);
    _this = _super.call(this, args);
    _this.browser = util.getBrowserVersion();
    _this.hls = null;
    _this.hlsOpts = {};
    return _this;
  }
  _createClass(HlsJsPlugin2, [{
    key: "afterCreate",
    value: function afterCreate() {
      var _this2 = this;
      var hlsOpts = this.config.hlsOpts;
      this.hlsOpts = hlsOpts;
      this.on(Events.URL_CHANGE, function(url) {
        if (/^blob/.test(url)) {
          return;
        }
        _this2.playerConfig.url = url;
        _this2.register(url);
      });
      try {
        BasePlugin.defineGetterOrSetter(this.player, {
          url: {
            get: function get() {
              try {
                return _this2.player.video.src;
              } catch (error) {
                return null;
              }
            },
            configurable: true
          }
        });
      } catch (e) {
      }
    }
  }, {
    key: "beforePlayerInit",
    value: function beforePlayerInit() {
      this.register(this.player.config.url);
    }
  }, {
    key: "destroy",
    value: function destroy() {
      this.hls && this.hls.destroy();
      var player = this.player;
      BasePlugin.defineGetterOrSetter(player, {
        url: {
          get: function get() {
            try {
              return player.__url;
            } catch (error) {
              return null;
            }
          },
          configurable: true
        }
      });
    }
  }, {
    key: "register",
    value: function register(url) {
      var _this3 = this;
      var player = this.player;
      if (this.hls) {
        this.hls.destroy();
      }
      this.hls = new Hls(this.hlsOpts);
      this.hls.once(Hls.Events.MEDIA_ATTACHED, function() {
        console.log("Hls.Events.MEDIA_ATTACHED", url);
        _this3.hls.loadSource(url);
      });
      this.hls.on(Hls.Events.ERROR, function(event, data) {
        player.emit("HLS_ERROR", {
          errorType: data.type,
          errorDetails: data.details,
          errorFatal: data.fatal
        });
        if (data.fatal) {
          switch (data.type) {
            case Hls.ErrorTypes.NETWORK_ERROR:
              _this3.hls.startLoad();
              break;
            case Hls.ErrorTypes.MEDIA_ERROR:
              _this3.hls.recoverMediaError();
              break;
            default:
              player.emit("error", data);
          }
        }
      });
      this.hls.attachMedia(this.player.video);
      this._statistics();
    }
  }, {
    key: "_statistics",
    value: function _statistics() {
      var statsInfo = {
        speed: 0,
        playerType: "HlsPlayer"
      };
      var mediainfo = {
        videoDataRate: 0,
        audioDataRate: 0
      };
      var player = this.player, hls = this.hls;
      hls.on(Hls.Events.FRAG_LOAD_PROGRESS, function(flag, payload) {
        statsInfo.speed = payload.stats.loaded / 1e3;
      });
      hls.on(Hls.Events.FRAG_PARSING_DATA, function(flag, payload) {
        if (payload.type === "video") {
          mediainfo.fps = parseInt(payload.nb / (payload.endPTS - payload.startPTS));
        }
      });
      hls.on(Hls.Events.FRAG_PARSING_INIT_SEGMENT, function(flag, payload) {
        mediainfo.hasAudio = !!(payload.tracks && payload.tracks.audio);
        mediainfo.hasVideo = !!(payload.tracks && payload.tracks.audio);
        if (mediainfo.hasAudio) {
          var track = payload.tracks.audio;
          mediainfo.audioChannelCount = track.metadata && track.metadata.channelCount ? track.metadata.channelCount : 0;
          mediainfo.audioCodec = track.codec;
        }
        if (mediainfo.hasVideo) {
          var _track = payload.tracks.video;
          mediainfo.videoCodec = _track.codec;
          mediainfo.width = _track.metadata && _track.metadata.width ? _track.metadata.width : 0;
          mediainfo.height = _track.metadata && _track.metadata.height ? _track.metadata.height : 0;
        }
        mediainfo.duration = payload.frag && payload.frag.duration ? payload.frag.duration : 0;
        mediainfo.level = payload.frag && payload.frag.levels ? payload.frag.levels : 0;
        if (mediainfo.videoCodec || mediainfo.audioCodec) {
          mediainfo.mimeType = 'video/hls; codecs="'.concat(mediainfo.videoCodec, ";").concat(mediainfo.audioCodec, '"');
        }
        player.mediainfo = mediainfo;
        player.emit("media_info", mediainfo);
      });
      this._statisticsTimmer = setInterval(function() {
        player.emit("statistics_info", statsInfo);
        statsInfo.speed = 0;
      }, 1e3);
    }
  }], [{
    key: "pluginName",
    get: function get() {
      return "HlsJsPlugin";
    }
  }, {
    key: "defaultConfig",
    get: function get() {
      return {
        hlsOpts: {}
      };
    }
  }, {
    key: "isSupported",
    get: function get() {
      return Hls.isSupported;
    }
  }]);
  return HlsJsPlugin2;
}(BasePlugin);
export { HlsJsPlugin as default };
