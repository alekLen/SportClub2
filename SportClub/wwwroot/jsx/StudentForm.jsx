namespace SportClub.wwwroot.js
{
    class StudentForm extends React.Component {
        constructor(props) {
            super(props);
            let full_name = props.full_name;
            let full_nameIsValid = this.validatefull_Name(full_name);

            let Login = props.Login;
            let LoginIsValid = this.validateLogin(Login);

            let age = props.age;
            let ageIsValid = this.validateAge(age);

            let Password = props.Password;
            let PasswordIsValid = this.validatePassword(Password);

            let PasswordConfirm = props.PasswordConfirm;
            let PasswordConfirmIsValid = this.validatePasswordConfirm(PasswordConfirm);

            this.state = {
                full_name: full_name,
                Login: Login,
                age: age,
                Password: Password,
                PasswordConfirm: PasswordConfirm,
                full_nameValid: full_nameIsValid,
                LoginValid: LoginIsValid,
                ageValid: ageIsValid,
                PasswordValid: PasswordIsValid,
                PasswordConfirmValid: PasswordConfirmIsValid,
            };

            this.onfull_NameChange = this.onfull_NameChange.bind(this);
            this.onLoginChange = this.onLoginChange.bind(this);
            this.onAgeChange = this.onAgeChange.bind(this);
            this.onPasswordChange = this.onPasswordChange.bind(this);
            this.onPasswordConfirmChange = this.onPasswordConfirmChange.bind(this);
            this.handleSubmit = this.handleSubmit.bind(this);
        }

        validateAge(age) {
            return age > 0;
        }

        validatefull_Name(full_name) {
            return full_name.length > 2;
        }

        validateLogin(Login) {
            return Login.length > 2;
        }

        validatePassword(Password) {
            return Password.length > 4;
        }

        validatePasswordConfirm(PasswordConfirm) {
            return PasswordConfirm.length > 4;
        }

        onAgeChange(e) {
            let val = e.target.value;
            let valid = this.validateAge(val);
            this.setState({ age: val, ageValid: valid });
        }

        onfull_NameChange(e) {
            let val = e.target.value;
            let valid = this.validatefull_Name(val);
            this.setState({ full_name: val, full_nameValid: valid });
        }
        onLoginChange(e) {
            let val = e.target.value;
            let valid = this.validateLogin(val);
            this.setState({ Login: val, LoginValid: valid });
        }

        onPasswordChange(e) {
            let val = e.target.value;
            let valid = this.validatePassword(val);
            this.setState({ Password: val, PasswordValid: valid });
        }

        onPasswordConfirmChange(e) {
            let val = e.target.value;
            let valid = this.validatePasswordConfirm(val);
            this.setState({ PasswordConfirm: val, PasswordConfirmValid: valid });
        }

        handleSubmit(e) {
            e.preventDefault();
            if (
                this.state.full_nameValid === true &&
                this.state.LoginValid === true &&
                this.state.ageValid === true &&
                this.state.PasswordValid === true &&
                this.state.PasswordConfirmValid === true
            ) {
                alert(
                    "Фио: " +
                    this.state.full_name +
                    "\nлогин: " +
                    this.state.Login +
                    "\nВозраст: " +
                    this.state.age +
                    "\npassword: " +
                    this.state.Password +
                    "\nPasswordConfirm: " +
                    this.state.PasswordConfirm
                );
            }
        }

        render() {
            let full_nameColor = this.state.full_nameValid === true ? "green" : "red";
            let LoginColor = this.state.LoginValid === true ? "green" : "red";
            let ageColor = this.state.ageValid === true ? "green" : "red";
            let PasswordColor = this.state.PasswordValid === true ? "green" : "red";
            let PasswordConfirmColor = this.state.PasswordConfirmValid === true ? "green" : "red";
            let EmailColor = this.state.EmailValid === true ? "green" : "red";

            return (
                <form onSubmit={this.handleSubmit}>
                    <p>
                        <label>Фио:</label>
                        <input
                            type="text"
                            value={this.state.full_name}
                            onChange={this.onfull_NameChange}
                            style={{ borderColor: full_nameColor }}
                        />
                    </p>
                    <p>
                        <label>Логин:</label>
                        <input
                            type="text"
                            value={this.state.Login}
                            onChange={this.onLoginChange}
                            style={{ borderColor: LoginColor }}
                        />
                    </p>
                    <p>
                        <label>Пароль:</label>
                        <input
                            type="password"
                            value={this.state.Password}
                            onChange={this.onPasswordChange}
                            style={{ borderColor: PasswordColor }}
                        />
                    </p>
                    <p>
                        <label>Подтверждение:</label>
                        <input
                            type="password"
                            value={this.state.PasswordConfirm}
                            onChange={this.onPasswordConfirmChange}
                            style={{ borderColor: PasswordConfirmColor }}
                        />
                    </p>
                    <p>
                        <label>Email:</label>
                        <input
                            type="password"
                            value={this.state.Email}
                            onChange={this.onEmailChange}
                            style={{ borderColor: EmailColor }}
                        />
                    </p>
                    <p>
                        <label>Дата рождения:</label>
                        <input
                            type="number"
                            value={this.state.age}
                            onChange={this.onAgeChange}
                            style={{ borderColor: ageColor }}
                        />
                    </p>
                    {/* <p>
          <label>Полное имя:</label>
          <input
            type="text"
            value={this.state.name}
            onChange={this.onNameChange}
            style={{ borderColor: nameColor }}
          />
        </p>
                <p>
                    <label>Рейтинг:</label>
                    <input
                        type="number"
                        value={this.state.rating}
                        onChange={this.onRatingChange}
                        style={{ borderColor: ratingColor }}
                    />
                </p> */}
                    <input type="submit" value="Отправить" />
                </form>
            );
        }
    }
}
