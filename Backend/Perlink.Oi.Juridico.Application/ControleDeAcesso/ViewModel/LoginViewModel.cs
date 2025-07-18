﻿namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel {
    public class LoginViewModel {
        public string Username { get; }
        public string Password { get; }
        public string GrantType { get; }
        public string RefreshToken { get; }

        public LoginViewModel(string granttype, string username, string password, string refreshtoken) {
            GrantType = granttype;
            Username = username.ToUpper();
            Password = password;
            RefreshToken = refreshtoken;
        }
    }
}