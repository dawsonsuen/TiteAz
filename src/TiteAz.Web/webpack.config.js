var webpack = require("webpack");
const path = require("path");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const package = require("./package.json");
const prod = process.argv.indexOf('--prod') !== -1;

module.exports = {
    resolve: {
        modules: ['src', 'node_modules'],
        // Add '.ts' and '.tsx' as resolvable extensions.
        extensions: [".webpack.js", ".web.js", ".ts", ".tsx", ".js", ".less"]
    },
    entry: {
        main: "./src/main.tsx",
        vendor: Object.keys(package.dependencies)
    },
    output: {
        path: path.resolve(__dirname, "build"),
        publicPath: "/",
        filename: "[name].bundle.js"
    },
    module: {
        rules: [{
                test: /\.css$/,
                use: ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: 'css-loader'
                }),
            },
            {
                test: /\.less$/,
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: ['css-loader', 'less-loader']
                }),
            },
            {
                test: /\.woff($|\?)|\.woff2($|\?)|\.ttf($|\?)|\.eot($|\?)|\.svg($|\?)/,
                loader: 'url-loader'
            },
            {
                test: /\.ts(x?)$/,
                use: ['react-hot-loader', 'babel-loader?presets[]=react,presets[]=es2015,presets[]=stage-0', 'ts-loader'],
                exclude: /node_modules/
            },
        ]
    },
    plugins: [
        new HtmlWebpackPlugin({
            title: 'cerebralts-base',
            template: path.resolve(__dirname, "./src/index.html"),
            inject: 'body'
        }),
        new ExtractTextPlugin({
            filename: 'styles.css'
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name: "vendor",
            filename: "vendor.bundle.js"
        }),
    ],
     devServer: {
        proxy: {
        '/api': {
            target: 'http://localhost:5050',
            secure: false
        }
    }
  }
};