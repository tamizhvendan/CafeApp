var productionConfig = require('./webpack.production.config');
var webpack = require('webpack');
var config = productionConfig;

config.entry =
  config.entry.concat('webpack-dev-server/client?http://0.0.0.0:3000').concat('webpack/hot/only-dev-server');

config.plugins[1] = new webpack.HotModuleReplacementPlugin()

config.devtool = 'eval'

config.module.loaders[0].loaders = ['react-hot', 'babel']

config.output.filename = 'bundle.js'

module.exports = config;