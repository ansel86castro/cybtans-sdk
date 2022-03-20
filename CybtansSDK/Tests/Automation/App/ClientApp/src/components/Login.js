"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var Login = /** @class */ (function (_super) {
    __extends(Login, _super);
    function Login(props) {
        var _this = _super.call(this, props) || this;
        _this.onInputChange = function (e) {
            var _a;
            _this.setState((_a = {}, _a[e.target.name] = e.target.value, _a));
        };
        _this.onSubmit = function (e) {
            e.preventDefault();
            fetch('api/login', {
                method: 'POST',
                body: JSON.stringify({ Username: _this.state.username, Password: _this.state.password }),
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                }
            })
                .then(function (response) {
                if (response.status == 200) {
                    _this.props.history.push('/');
                }
                else {
                    _this.setState({ errorMessage: 'Authentication fail' });
                }
            }, function (error) {
                _this.setState({ errorMessage: "logging off fail" });
            });
            return false;
        };
        _this.onSignOff = function (e) {
            e.preventDefault();
            fetch('api/login/logoff', {
                method: 'GET',
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                }
            })
                .then(function (response) {
                if (response.status == 200) {
                    _this.props.history.push('/');
                }
                else {
                    _this.setState({ errorMessage: 'Authentication fail' });
                }
            }, function (error) {
                _this.setState({ errorMessage: "logging off fail" });
            });
            return false;
        };
        _this.state = {};
        return _this;
    }
    Login.prototype.render = function () {
        return (React.createElement("form", null,
            this.state.errorMessage && React.createElement("div", { className: "alert alert-danger", role: "alert" }, this.state.errorMessage),
            React.createElement("div", { className: "form-group" },
                React.createElement("label", { htmlFor: "exampleInputEmail1" }, "Username"),
                React.createElement("input", { name: "username", onChange: this.onInputChange, className: "form-control", id: "exampleInputEmail1", "aria-describedby": "emailHelp", placeholder: "Enter Username" })),
            React.createElement("div", { className: "form-group" },
                React.createElement("label", { htmlFor: "exampleInputPassword1" }, "Password"),
                React.createElement("input", { name: "password", onChange: this.onInputChange, type: "password", className: "form-control", id: "exampleInputPassword1", placeholder: "Password" })),
            React.createElement("button", { className: "btn btn-primary", onClick: this.onSubmit }, "Sign in"),
            React.createElement("button", { className: "btn btn-secundary", onClick: this.onSignOff }, "Sign Off")));
    };
    return Login;
}(React.PureComponent));
exports.default = Login;
//# sourceMappingURL=Login.js.map